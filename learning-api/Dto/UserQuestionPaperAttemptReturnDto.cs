using learning_api.Models;

namespace learning_api.Dto
{
    public class UserQuestionPaperAttemptReturnDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuestionPaperId { get; set; }
        public bool IsSubmitted { get; set; }
        public List<UserQuestionAnswerReturnDto> UserQuestionPaperAnswers { get; set; }
    }
}
