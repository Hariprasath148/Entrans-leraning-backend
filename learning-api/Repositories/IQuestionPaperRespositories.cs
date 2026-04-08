using learning_api.Models;

namespace learning_api.Repositories
{
    public interface IQuestionPaperRespositories
    {
        Task SaveChanges();
        Task AddQuestionPaper(QuestionPaper QuestionPaper);
        Task<QuestionPaper> GetQuestionPaperById(int Id);
        Task AddQuestionToQuestionPaper(QuestionPaper QuestionPaper, Questions Questions);
        Task<QuestionAttempt> GetQuestionPaperAttemptByUserIdAndQuestionPaperID(int Id ,int QuestionPaperId);
        Task AddQuestionPaperAttempt(QuestionAttempt QuestionAttempt);
        Task<List<QuestionPaper>> GetAllQuestionPaper();
        Task AddUserQuestionPaperAnswerToQuestionAttempt(QuestionAttempt QuestionAttempt, UserQuestionPaperAnswer UserQuestionPaperAnswer);
        Task<int> GetUserProgressByUserId(int Id);
        Task<int> GetQuestionsCount();
        Task AddRangeQuestionToQuestionsPaper(QuestionPaper QuestionPaper, List<Questions> Questions);
    }
}
