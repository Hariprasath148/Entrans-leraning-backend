namespace learning_api.Dto
{
    public class QuestionPaperDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<QuestionReturnDto> Questions { get; set; }
    }
}
