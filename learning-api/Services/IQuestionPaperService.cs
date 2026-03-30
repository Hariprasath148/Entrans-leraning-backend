using learning_api.Dto;
using learning_api.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace learning_api.Services
{
    public interface IQuestionPaperService
    {
        Task<object> AddQuestionPaper(string QuestionPaperName);
        Task<object> AddQuestions(int Id, QuestionDto questionDto);
        Task<object> GetQuestionPaperById(int Id);
        Task<object> GetAllQuestionPaper();
        Task<object> AddUserQuestionPaperAnswers(int Id,UserQuestionPaperAnswerDto userQuestionPaperAnswerDto);
        Task<object> GetAllQUestionsForUser(int UserId,int QuestionPaperId);
        Task SubmitTheQuestionPaper(int Id,int QuestionPaperId);
    }
}
