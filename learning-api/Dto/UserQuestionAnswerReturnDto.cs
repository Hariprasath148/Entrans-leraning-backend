using learning_api.Models;

namespace learning_api.Dto
{
    public class UserQuestionAnswerReturnDto
    {
        public int Id { get; set; }
        public String Answer { get; set; }
        public List<string> AnswerList { get; set; }
        public int QuestionsId { get; set; }
        public int QuestionAttemptId { get; set; }
    }
}
