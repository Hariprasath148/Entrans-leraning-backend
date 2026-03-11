using Microsoft.AspNetCore.Mvc;
using learning_api.Dto;
namespace learning_api.Controllers
{

    [Route("/questionPaper")]
    public class QuestionPaperController : Controller
    {

        private static QuestionPaper QuestionPaper = new QuestionPaper
        {
            Questions = new QuestionDto[]{
                new QuestionDto{
                    Id = 1,
                    Type = 1,
                    Question = "What does HTML stand for?",
                    Required = true,
                    IsAttended = false
                },
                new QuestionDto{
                    Id = 2,
                    Type = 3,
                    Question = "Which of the following is a JavaScript framework?",
                    Required = true,
                    IsAttended = false,
                    Choice = new List<string>{ "Laravel", "Django", "Angular", "Flask"}
                },
                new QuestionDto{
                    Id = 3,
                    Type = 2,
                    Question = "Explain the difference between frontend and backend development.",
                    Required = true,
                    IsAttended = false
                },
                new QuestionDto{
                    Id = 4,
                    Type = 4,
                    Question = "Select JavaScript libraries/frameworks.",
                    Required = true,
                    IsAttended = false,
                    Choice = new List<string>{ "React", "Angular", "Laravel", "Vue" }
                },
                new QuestionDto{
                    Id = 5,
                    Type = 1,
                    Question = "What does SQL stand for?",
                    Required = true,
                    IsAttended = false  
                },
                new QuestionDto{
                    Id = 6,
                    Type = 3,
                    Question = "Which of the following is a JavaScript framework?",
                    Required = true,
                    IsAttended = false,
                    Choice = new List<string>{ "Java", "C", "Python", "JavaScript" }
                },
                new QuestionDto{
                    Id = 7,
                    Type = 4,
                    Question = "Select operating systems.",
                    Required = true,
                    IsAttended = false,
                    Choice = new List<string>{ "Windows", "Linux", "SQL", "macOS" }
                },
                new QuestionDto{
                    Id = 8,
                    Type = 2,
                    Question = "Explain the role of CSS in web development.",
                    Required = true,
                    IsAttended = false
                },
                new QuestionDto{
                    Id = 9,
                    Type = 3,
                    Question = "Which data structure uses FIFO?",
                    Required = true,
                    IsAttended = false,
                    Choice = new List<string>{ "Stack", "Queue", "Tree", "Graph" }
                },
                new QuestionDto{
                    Id = 10,
                    Type = 4,
                    Question = "Select backend languages.",
                    Required = true,
                    IsAttended = false,
                    Choice = new List<string>{ "Node.js", "Python", "PHP", "CSS" }
                },
            },
            IsSubmitted = false
        };


        [HttpGet("getAllQuestions")]
        public IActionResult getAllQuestions()
        {
            return Ok(QuestionPaper);
        }

        [HttpPost("editQuestion")]
        public IActionResult setAnswer([FromBody] QuestionDto changeQuesion)
        {

            QuestionDto question = Array.Find(QuestionPaper.Questions, question => question.Id == changeQuesion.Id);

            if (question == null) return BadRequest(new { error = "Quesion Not Found." });

            question.IsAttended = true;

            if (question.Type == 3 || question.Type == 4)
            {
                question.AnswerList = changeQuesion.AnswerList;
            }
            else
            {
                question.AnswerText = changeQuesion.AnswerText;
            }
            return Ok(new { message = "Sucessfully Updated" });
        }

        [HttpPost("submitQuestions")]
        public IActionResult submitQuesitons()
        {
            QuestionPaper.IsSubmitted = true;
            return Ok(QuestionPaper);
        }

    }
}
