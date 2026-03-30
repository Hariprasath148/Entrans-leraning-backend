namespace learning_api.Dto
{
    public class QuestionPaperWithAttemptDto
    {
        public int Id { get; set; }
        public List<QuestionWithAnswerDto> Questions { set; get; }
        public bool IsSubmitted { set; get; }
        public string Title { get; set; }
    }
}
