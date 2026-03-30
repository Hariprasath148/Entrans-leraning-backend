using learning_api.Dto;
using learning_api.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace learning_api.Mappers
{
    public static class QuestionPaperMapper
    {
        public static Questions ToQuestionsEntity(this QuestionDto QuestionDto)
        {
            Questions newQuestion = new Questions
            {
                Type = QuestionDto.Type,
                Question = QuestionDto.Question,
                Required = QuestionDto.Required,
            };

            if (QuestionDto.Choices != null) newQuestion.ChoiceJson = JsonSerializer.Serialize(QuestionDto.Choices);
            Console.WriteLine(JsonSerializer.Serialize(QuestionDto.Choices));
            return newQuestion;
        }

        public static QuestionPaperDto ToQuestionPaperToDto(this QuestionPaper QuestionPaper)
        {
            return new QuestionPaperDto
            {
                Id = QuestionPaper.Id,
                Title = QuestionPaper.Title,
                Questions = QuestionPaper.Questions.Select(q => new QuestionReturnDto
                {
                    Id = q.Id,
                    Type = q.Type,
                    Question = q.Question,
                    Required = q.Required,
                    Choices = string.IsNullOrEmpty(q.ChoiceJson) ? null : JsonSerializer.Deserialize<List<string>>(q.ChoiceJson)
                }).ToList()
            };
        }

        public static UserQuestionPaperAttemptReturnDto ToUserQuestionPaperAttemptDto(this QuestionAttempt QuestionAttempt)
        {
            return new UserQuestionPaperAttemptReturnDto
            {
                Id = QuestionAttempt.Id,
                UserId = QuestionAttempt.UserId,
                QuestionPaperId = QuestionAttempt.QuestionPaperId,
                IsSubmitted = QuestionAttempt.IsSubmitted,
                UserQuestionPaperAnswers = QuestionAttempt.UserQuestionPaperAnswers.Select(a => new UserQuestionAnswerReturnDto
                {
                    Id = a.Id,
                    AnswerList = (a.Answer.Trim().StartsWith("[")) ? JsonSerializer.Deserialize<List<string>>(a.Answer) : null,
                    Answer = (a.Answer.Trim().StartsWith("[")) ? null : a.Answer,
                    QuestionsId = a.QuestionsId,
                    QuestionAttemptId = a.QuestionAttemptId
                }).ToList()
            };
        }

        //    public static QuestionPaperWithAttemptDto MergerWithQuestions(
        //this QuestionPaper QuestionPaper,
        //QuestionAttempt QuestionAttempt)
        //    {
        //        return new QuestionPaperWithAttemptDto
        //        {
        //            Questions = QuestionPaper.Questions.Select(q =>
        //            {
        //                var userAnswer = QuestionAttempt?.UserQuestionPaperAnswers
        //                    .FirstOrDefault(u => u.QuestionsId == q.Id);

        //                List<string> answerList = new();

        //                if (!string.IsNullOrEmpty(userAnswer?.Answer))
        //                {
        //                    if (userAnswer.Answer.Trim().StartsWith("["))
        //                    {
        //                        answerList = JsonSerializer.Deserialize<List<string>>(userAnswer.Answer);
        //                    }
        //                    else
        //                    {
        //                        answerList = new List<string> { userAnswer.Answer };
        //                    }
        //                }

        //                return new QuestionWithAnswerDto
        //                {
        //                    Id = q.Id,
        //                    Type = q.Type,
        //                    Question = q.Question,
        //                    Required = q.Required,

        //                    IsAttended = userAnswer != null,

        //                    AnswerText = (q.Type == 1 || q.Type == 2)
        //                        ? userAnswer?.Answer
        //                        : null,

        //                    AnswerList = (q.Type == 3 || q.Type == 4)
        //                        ? answerList
        //                        : new List<string>(),

        //                    Choice = q.ChoiceJson == null
        //                        ? null
        //                        : JsonSerializer.Deserialize<List<string>>(q.ChoiceJson)
        //                };
        //            }).ToList(),

        //            IsSubmitted = QuestionAttempt?.IsSubmitted ?? false,
        //            Id = QuestionPaper.Id
        //        };
        //    }
        public static QuestionPaperWithAttemptDto MergerWithQuestions(this QuestionPaper QuestionPaper, QuestionAttempt QuestionAttempt)
        {
            if (QuestionAttempt == null)
            {
                return new QuestionPaperWithAttemptDto
                {
                    Questions = QuestionPaper.Questions.Select(q => new QuestionWithAnswerDto
                    {
                        Id = q.Id,
                        Type = q.Type,
                        Question = q.Question,
                        Required = q.Required,
                        IsAttended = false,
                        AnswerText = null,
                        AnswerList = new List<string>(),
                        Choice = (q.ChoiceJson == null) ? null : JsonSerializer.Deserialize<List<string>>(q.ChoiceJson)
                    }).ToList(),
                    IsSubmitted = false,
                    Id = QuestionPaper.Id,
                    Title = QuestionPaper.Title
                };
            }

            return new QuestionPaperWithAttemptDto
            {
                Questions = QuestionPaper.Questions.Select(q => new QuestionWithAnswerDto
                {
                    Id = q.Id,
                    Type = q.Type,
                    Question = q.Question,
                    Required = q.Required,
                    IsAttended = (QuestionAttempt.UserQuestionPaperAnswers.FirstOrDefault(u => u.QuestionsId == q.Id) == null) ? false : true,
                    AnswerText = (q.Type == 1 || q.Type == 2) ? (QuestionAttempt.UserQuestionPaperAnswers.FirstOrDefault(u => u.QuestionsId == q.Id) == null) ? null : QuestionAttempt.UserQuestionPaperAnswers.FirstOrDefault(u => u.QuestionsId == q.Id).Answer : null,
                    AnswerList = (q.Type == 3 || q.Type == 4) ? (QuestionAttempt.UserQuestionPaperAnswers.FirstOrDefault(u => u.QuestionsId == q.Id) == null) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(QuestionAttempt.UserQuestionPaperAnswers.FirstOrDefault(u => u.QuestionsId == q.Id).Answer) : new List<string>(),
                    Choice = (q.ChoiceJson == null) ? null : JsonSerializer.Deserialize<List<string>>(q.ChoiceJson)
                }).ToList(),
                IsSubmitted = QuestionAttempt.IsSubmitted,
                Id = QuestionPaper.Id,
                Title = QuestionPaper.Title
            };
        }
    }
}
