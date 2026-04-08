using learning_api.Data;
using learning_api.Models;
using Microsoft.EntityFrameworkCore;

namespace learning_api.Repositories
{
    public class QuestionPaperRespositories : IQuestionPaperRespositories
    {
        public readonly AppDbContext _context;
        public QuestionPaperRespositories(AppDbContext context) { _context = context; }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddQuestionPaper(QuestionPaper QuestionPaper)
        {
            _context.QuestionPaper.Add(QuestionPaper);
            await _context.SaveChangesAsync();
        }

        public async Task<QuestionPaper> GetQuestionPaperById(int Id)
        {
            return await _context.QuestionPaper.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == Id);
        }

        public async Task AddQuestionToQuestionPaper(QuestionPaper QuestionPaper, Questions Questions)
        {
            QuestionPaper.Questions.Add(Questions);
            await _context.SaveChangesAsync();
        }

        public async Task<QuestionAttempt> GetQuestionPaperAttemptByUserIdAndQuestionPaperID(int Id, int QuestionPaperId)
        {
            return await _context.QuestionAttempts.Include(qa => qa.UserQuestionPaperAnswers).FirstOrDefaultAsync(qa => qa.UserId == Id && qa.QuestionPaperId == QuestionPaperId);
        }

        public async Task AddQuestionPaperAttempt(QuestionAttempt QuestionAttempt)
        {
            _context.QuestionAttempts.Add(QuestionAttempt);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserQuestionPaperAnswerToQuestionAttempt(QuestionAttempt QuestionAttempt, UserQuestionPaperAnswer UserQuestionPaperAnswer)
        {
            QuestionAttempt.UserQuestionPaperAnswers.Add(UserQuestionPaperAnswer);
        }

        public async Task<List<QuestionPaper>> GetAllQuestionPaper()
        {
            return await _context.QuestionPaper.ToListAsync();
        }

        public async Task<int> GetUserProgressByUserId(int Id) {
            return await _context.QuestionAttempts
                                  .Where(QA => QA.UserId == Id)
                                  .SumAsync(QA => QA.UserQuestionPaperAnswers.Count());
        }

        public async Task<int> GetQuestionsCount()
        {
            return await _context.Questions.CountAsync();
        }

        public async Task AddRangeQuestionToQuestionsPaper(QuestionPaper QuestionPaper, List<Questions> Questions)
        {
            QuestionPaper.Questions.AddRange(Questions);
            await _context.SaveChangesAsync();
        }
    }
}
