namespace learning_api.Dto
{
    public class QuestionWithAnswerDto
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Question { get; set; }
        public bool Required { get; set; }
        public bool IsAttended { get; set; }
        public string AnswerText { get; set; }
        public List<string> AnswerList { get; set; } = new List<string>();
        public List<string> Choice { get; set; } = new List<string>();
    }
}