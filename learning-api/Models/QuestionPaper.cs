using System.ComponentModel.DataAnnotations;

namespace learning_api.Models
{
    public class QuestionPaper
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Questions> Questions { get; set; }
    }
}
