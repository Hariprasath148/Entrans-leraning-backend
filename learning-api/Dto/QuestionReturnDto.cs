namespace learning_api.Dto
{
    public class QuestionReturnDto
    {
        public int Id { get; set; }
        public int Type { set; get; }
        public string? Question { set; get; }
        public bool Required { set; get; }
        public List<string> Choices { set; get; }
    }
}