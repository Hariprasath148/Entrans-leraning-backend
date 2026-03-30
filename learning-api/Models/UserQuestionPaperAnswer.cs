using System.ComponentModel.DataAnnotations;

namespace learning_api.Models
{
    public class UserQuestionPaperAnswer
    {
        [Key]
        public int Id { get; set; }
        public String Answer { get; set; }
        public int QuestionsId { get; set; }
        public Questions Questions { get; set; }
        public int QuestionAttemptId { get; set; }
        public QuestionAttempt QuestionAttempt { get; set; }
    }
}
