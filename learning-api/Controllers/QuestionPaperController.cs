using learning_api.Dto;
using learning_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace learning_api.Controllers
{

    [Route("/questionPaper")]
    public class QuestionPaperController : Controller
    {

        public readonly IQuestionPaperService _questionPaperService;

        public QuestionPaperController(IQuestionPaperService QuestionPaperservice) { _questionPaperService = QuestionPaperservice; }

        [Authorize]
        [HttpPost("addQuestionPaper/{QuestionPaperText}")]
        public async Task<IActionResult> AddQuestionPaper(string QuestionPaperText)
        {
            var newQuestionPaper = await _questionPaperService.AddQuestionPaper(QuestionPaperText);

            return Ok(newQuestionPaper);
        }

        [Authorize]
        [HttpPost("addNewQuestion/{id}")]
        public async Task<IActionResult> AddNewQuestion(int id,[FromBody] QuestionDto questionDto)
        {
            var questionPaper = await _questionPaperService.AddQuestions(id, questionDto);

            return Ok(questionPaper);
        }

        [Authorize]
        [HttpGet("getQuestionPaperById/{id}")]
        public async Task<IActionResult> GetQuestionPaperById(int id)
        {
            var questionPaper = await _questionPaperService.GetQuestionPaperById(id);

            return Ok(questionPaper);
        }

        [Authorize]
        [HttpPost("setQuestionPaperAnswer")]
        public async Task<IActionResult> SetQuestionPaperAnswer([FromBody] UserQuestionPaperAnswerDto userQuestionPaperAnswerDto)
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var quesitionAttempt = await _questionPaperService.AddUserQuestionPaperAnswers(id, userQuestionPaperAnswerDto);

            return Ok(quesitionAttempt);
        }

        [Authorize]
        [HttpGet("getAllQuestions/{id}")]
        public async Task<IActionResult> GetAllQuestions(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var mergedQuestionPaper = await _questionPaperService.GetAllQUestionsForUser(userId, id);

            return Ok(mergedQuestionPaper);
        }

        [Authorize]
        [HttpGet("getAllQuestionPaper")]
        public async Task<IActionResult> getAllQuestionPaper()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var questionPaper = await _questionPaperService.GetAllQuestionPaper(userId);

            return Ok(questionPaper);
        }

        [Authorize]
        [HttpPost("submitQuestions/{id}")]
        public async Task<IActionResult> submitQuesitons(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await _questionPaperService.SubmitTheQuestionPaper(userId,id);

            return Ok(new {message = "Question Paper Submitted"});
        }

        [Authorize]
        [HttpPost("insertQuestionPaperWithQuestions/{id}")]
        public async Task<IActionResult> insterQuestionPaperWithQuestions(int id, [FromBody] List<QuestionDto> Questions)
        {
            var questionPaper = await _questionPaperService.InsterQuestionPaperWithQuestions(id, Questions);

            return Ok(questionPaper);
        }

        [Authorize]
        [HttpPost("insertNewQuesitonPaper")]
        public async Task<IActionResult> insetNewQuesitonPaper([FromBody] QuestionPaperBulkDto quesitonPaperDto)
        {
            var questionPaper = await _questionPaperService.InsertNewQuestionPaper(quesitonPaperDto);

            return Ok(questionPaper);
        }
    }
}
