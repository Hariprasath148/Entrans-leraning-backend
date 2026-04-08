namespace learning_api.Dto
{
    public class QuestionPaperBulkDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<QuestionDto> Questions { get; set; }
    }
}
