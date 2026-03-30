namespace learning_api.Dto
{
    public class UserQuestionPaperAnswerDto
    {
        public String Answer { get; set; }
        public List<string> AnswerList { set; get; }
        public int QuestionPaperId { get; set; }
        public int QuestionsId { get; set; }
    }
}
