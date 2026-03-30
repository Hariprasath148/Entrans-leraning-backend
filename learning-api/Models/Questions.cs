using System.ComponentModel.DataAnnotations;

namespace learning_api.Models
{
    public class Questions
    {
        [Key]
        public int Id { get; set; }
        public int Type { set; get; }
        public string? Question { set; get; }
        public bool Required { set; get; }
        public string? ChoiceJson { set; get; }
        public List<QuestionPaper> QuestionPapers { get; set; }
    }
}