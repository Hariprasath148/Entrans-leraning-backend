using learning_api.Models;
using learning_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System.Security.Claims;

namespace learning_api.Controllers
{
    [Route("[Controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService) { _chatService = chatService; }

        [HttpGet("users")]
        [Authorize]
        public async Task<IActionResult> GetChatUsers()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var chatList = await _chatService.GetChatUsers(userId);

            return Ok(chatList);
        }
    }
}
