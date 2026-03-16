namespace learning_api.Dto
{
    public class QuestionDto
    {
        public int Id { set; get; }
        public int Type { set; get; }
        public string? Question { set; get; }
        public bool Required { set; get; }
        public bool IsAttended { set; get; }
        public string ? AnswerText { set; get; }
        public List<string> AnswerList { set; get; } = new List<string>();
        public List<string>? Choice { set; get; }
    }
}
