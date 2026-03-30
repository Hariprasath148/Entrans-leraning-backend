using System.ComponentModel.DataAnnotations;

namespace learning_api.Models
{
    public class QuestionAttempt
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int QuestionPaperId { get; set; }
        public QuestionPaper QuestionPaper { get; set; }
        public bool IsSubmitted { get; set; }
        public List<UserQuestionPaperAnswer> UserQuestionPaperAnswers { get; set; }
    }
}
