using ASH.Exceptions;
using ASH.Models;
using ASH.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASH.Controllers
{
    [ApiController]
    [Route("chat")]
    public class ChatController(
        IChatService chatService,
        ILogger<ChatController> logger
    ) : ControllerBase
    {
        private readonly IChatService _chatService = chatService;
        private readonly ILogger<ChatController> _logger = logger;

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatThreadRequest request)
        {
            try
            {
                return await _chatService.CreateChatThread(request.Topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while starting the chat thread.");
                return StatusCode(
                    500,
                    "An internal server error occurred. Please try again later."
                );
            }
        }

        [HttpPost("user")]
        public async Task<IActionResult> AddUserToChat([FromBody] AddUserToChatRequest request)
        {
            try
            {
                var userId = await _chatService.AddUserToChatAsync(request);
                return Ok(new { UserId = userId });
            }
            catch (ConflictException ex)
            {
                _logger.LogError(ex, "An error occurred, the username must be unique.");
                return Conflict(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the user to the chat.");
                return StatusCode(
                    500,
                    new
                    {
                        Message = "An error occurred while adding the user to the chat",
                        Error = ex.Message,
                    }
                );
            }
        }

        [HttpDelete("user/{displayName}")]
        public async Task<IActionResult> RemoveUserFromChat(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                _logger.LogWarning("Attempted to remove a user with an invalid displayName.");
                return BadRequest(new { Message = "Display name cannot be empty or null." });
            }

            try
            {
                await _chatService.RemoveUserFromChatThreadAsync(displayName);

                return Ok(new { Message = "User successfully removed from the chat." });
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "An error occurred, the user was not found.");
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing the user from the chat.");
                return StatusCode(
                    500,
                    new
                    {
                        Message = "An error occurred while removing the user from the chat.",
                        Error = ex.Message,
                    }
                );
            }
        }

        [HttpPost("message")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            try
            {
                await _chatService.SendMessageAsync(request.Message, request.DisplayName);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending the message.");
                return StatusCode(
                    500,
                    "An internal server error occurred. Please try again later."
                );
            }
        }

        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages()
        {
            try
            {
                var messages = await _chatService.GetMessagesAsync();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving messages.");
                return StatusCode(
                    500,
                    "An internal server error occurred. Please try again later."
                );
            }
        }
    }
}
