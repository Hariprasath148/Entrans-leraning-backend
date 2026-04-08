using learning_api.Dto;
using learning_api.Models;
using learning_api.Repositories;
using System.Globalization;
using learning_api.Mappers;
using System.Text.Json;

namespace learning_api.Services
{
    public class QuestionPaperService : IQuestionPaperService
    {
        public readonly IQuestionPaperRespositories _questionPaperRespositories;

        public QuestionPaperService(IQuestionPaperRespositories QuestionPaperRespositories) { _questionPaperRespositories = QuestionPaperRespositories; }

        public async Task<object> AddQuestionPaper(String QuestionPaperName)
        {
            if(QuestionPaperName == null || QuestionPaperName.Length == 0)
            {
                throw new ArgumentException("Invalid QuestionPaper Name");
            }

            QuestionPaper newQuestionPaper = new QuestionPaper
            {
                Title = QuestionPaperName
            };

            await _questionPaperRespositories.AddQuestionPaper(newQuestionPaper);

            return newQuestionPaper;
        }

        public async Task<object> AddQuestions(int Id, QuestionDto questionDto)
        {
            QuestionPaper questionPaper = await _questionPaperRespositories.GetQuestionPaperById(Id);

            if (questionPaper == null) throw new ArgumentException("Question Paper not found with the give Id " + Id);

            Questions newQuestion = questionDto.ToQuestionsEntity();
            
            await _questionPaperRespositories.AddQuestionToQuestionPaper(questionPaper, newQuestion);
            
            return questionPaper.ToQuestionPaperToDto();
        }

        public async Task<object> GetQuestionPaperById(int Id)
        {
            QuestionPaper questionPaper = await _questionPaperRespositories.GetQuestionPaperById(Id);

            if (questionPaper == null) throw new ArgumentException("Question Paper not found with the give Id " + Id);

            return questionPaper.ToQuestionPaperToDto();
        }

        public async Task<object> AddUserQuestionPaperAnswers(int Id,UserQuestionPaperAnswerDto userQuestionPaperAnswerDto)
        {
            QuestionAttempt questionAttempt = await _questionPaperRespositories.GetQuestionPaperAttemptByUserIdAndQuestionPaperID(Id , userQuestionPaperAnswerDto.QuestionPaperId);

            if(questionAttempt == null)
            {
                questionAttempt = new QuestionAttempt
                {
                    UserId = Id,
                    QuestionPaperId = userQuestionPaperAnswerDto.QuestionPaperId,
                    UserQuestionPaperAnswers = new List<UserQuestionPaperAnswer>()
                };

                await _questionPaperRespositories.AddQuestionPaperAttempt(questionAttempt);
            }

            UserQuestionPaperAnswer answer = questionAttempt.UserQuestionPaperAnswers.FirstOrDefault(u => u.QuestionsId == userQuestionPaperAnswerDto.QuestionsId);
            
            if(answer == null)
            {
                answer = new UserQuestionPaperAnswer {
                    Answer = (userQuestionPaperAnswerDto.AnswerList != null) ? JsonSerializer.Serialize(userQuestionPaperAnswerDto.AnswerList) : userQuestionPaperAnswerDto.Answer,
                    QuestionsId = userQuestionPaperAnswerDto.QuestionsId,
                    QuestionAttemptId = questionAttempt.Id
                };

                await _questionPaperRespositories.AddUserQuestionPaperAnswerToQuestionAttempt(questionAttempt, answer);
            }
            else
            {
                answer.Answer = (userQuestionPaperAnswerDto.AnswerList != null) ? JsonSerializer.Serialize(userQuestionPaperAnswerDto.AnswerList) : userQuestionPaperAnswerDto.Answer;
            }

            await _questionPaperRespositories.SaveChanges();

            return new {message = "Save Successfully"};
        }
        public async Task<object> GetAllQUestionsForUser(int UserId, int QuestionPaperId)
        {
            var questionPaper = await _questionPaperRespositories.GetQuestionPaperById(QuestionPaperId);

            if(questionPaper == null)
            {
                throw new ArgumentException("Cannot find any question in Question ID "+QuestionPaperId);
            }

            var userAttempt = await _questionPaperRespositories.GetQuestionPaperAttemptByUserIdAndQuestionPaperID(UserId , QuestionPaperId);

            var mergedQuesitons = questionPaper.MergerWithQuestions(userAttempt);

            return mergedQuesitons;
        }

        public async Task<object> GetAllQuestionPaper(int UserId)
        {
            var questionPaper = await _questionPaperRespositories.GetAllQuestionPaper();

            int Progress = await _questionPaperRespositories.GetUserProgressByUserId(UserId);

            int QuestionsCount = await _questionPaperRespositories.GetQuestionsCount();

            return new
            {
                questionPaper = questionPaper.Select(q =>
                      new
                      {
                          Id = q.Id,
                          Title = q.Title
                      }).ToList(),
                UserProgress = (QuestionsCount == 0) ? 0 : Math.Floor((double)Progress / QuestionsCount * 100)
            };
        }

        public async Task SubmitTheQuestionPaper(int Id,int QuestionPaperId)
        {
            var userAttempt = await _questionPaperRespositories.GetQuestionPaperAttemptByUserIdAndQuestionPaperID(Id,QuestionPaperId);

            if(userAttempt == null)
            {
                throw new ArgumentException("User Attempt not found");
            }

            userAttempt.IsSubmitted = true;

            await _questionPaperRespositories.SaveChanges();
        }

        public async Task<object> InsterQuestionPaperWithQuestions(int Id, List<QuestionDto> Questions)
        {
            QuestionPaper questionPaper = await _questionPaperRespositories.GetQuestionPaperById(Id);

            if (questionPaper == null) throw new ArgumentException("Question Paper not found with the give Id " + Id);

            if (Questions == null) throw new ArgumentException("Enter the Quesiton in the Array Fromat");

            List<Questions> questions = new List<Questions>();

            foreach(QuestionDto Question in Questions)
            {
                questions.Add(Question.ToQuestionsEntity());
            }

            await _questionPaperRespositories.AddRangeQuestionToQuestionsPaper(questionPaper, questions);

            return questionPaper.ToQuestionPaperToDto();
        }

        public async Task<object> InsertNewQuestionPaper(QuestionPaperBulkDto QuestionPaper)
        {

            if (QuestionPaper.Title == null || QuestionPaper.Title.Length == 0)
            {
                throw new ArgumentException("Invalid QuestionPaper Name");
            }

            QuestionPaper newQuestionPaper = new QuestionPaper
            {
                Title = QuestionPaper.Title
            };

            await _questionPaperRespositories.AddQuestionPaper(newQuestionPaper);

            return await InsterQuestionPaperWithQuestions(newQuestionPaper.Id, QuestionPaper.Questions);
        }
    }
}
