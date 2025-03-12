using ASH.Controllers;
using ASH.Exceptions;
using ASH.Models;
using ASH.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ASH.Tests
{
    public class ChatControllerUnitTest
    {
        private readonly ChatController _controller;
        private readonly Mock<IChatService> _mockChatService;
        private readonly Mock<ILogger<ChatController>> _mockLogger;

        public ChatControllerUnitTest()
        {
            _mockChatService = new Mock<IChatService>();
            _mockLogger = new Mock<ILogger<ChatController>>();
            _controller = new ChatController(
                _mockChatService.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task CreateChat_ReturnsOkResult()
        {
            var request = new CreateChatThreadRequest { Topic = "TestTopic" };
            _mockChatService
                .Setup(service => service.CreateChatThread(It.IsAny<string>()))
                .ReturnsAsync(new OkObjectResult(new { ThreadId = "test-thread-id" }));

            var result = await _controller.CreateChat(request);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateChat_ReturnsInternalServerError_OnException()
        {
            var request = new CreateChatThreadRequest { Topic = "TestTopic" };
            _mockChatService
                .Setup(service => service.CreateChatThread(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _controller.CreateChat(request);

            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult?.StatusCode ?? 0);
        }

        [Fact]
        public async Task AddUserToChat_ReturnsOkResult()
        {
            var request = new AddUserToChatRequest { DisplayName = "UserTest" };
            _mockChatService
                .Setup(service => service.AddUserToChatAsync(It.IsAny<AddUserToChatRequest>()))
                .ReturnsAsync("test-user-id");

            var result = await _controller.AddUserToChat(request);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public async Task AddUserToChat_ReturnsConflict_OnConflictException()
        {
            var request = new AddUserToChatRequest { DisplayName = "UserTest" };
            _mockChatService
                .Setup(service => service.AddUserToChatAsync(It.IsAny<AddUserToChatRequest>()))
                .ThrowsAsync(new ConflictException("Test conflict"));

            var result = await _controller.AddUserToChat(request);

            var objectResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, objectResult.StatusCode);
        }

        [Fact]
        public async Task AddUserToChat_ReturnsInternalServerError_OnException()
        {
            var request = new AddUserToChatRequest { DisplayName = "UserTest" };
            _mockChatService
                .Setup(service => service.AddUserToChatAsync(It.IsAny<AddUserToChatRequest>()))
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _controller.AddUserToChat(request);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task SendMessage_ReturnsOkResult()
        {
            var request = new SendMessageRequest { Message = "Hello", DisplayName = "UserTest" };
            _mockChatService
                .Setup(service => service.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.SendMessage(request);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task SendMessage_ReturnsInternalServerError_OnException()
        {
            var request = new SendMessageRequest { Message = "Hello", DisplayName = "UserTest" };
            _mockChatService
                .Setup(service => service.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _controller.SendMessage(request);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetMessages_ReturnsOkResult()
        {
            var messages = new List<ChatMessageItem>
            {
                new() { Id = "1", Content = "Hello", SenderDisplayName = "UserTest", Type = "MessageType", SequenceId = "1", SenderRawId = "raw-id" }
            };
            _mockChatService
                .Setup(service => service.GetMessagesAsync())
                .ReturnsAsync(messages);

            var result = await _controller.GetMessages();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value as List<ChatMessageItem>;
            Assert.NotNull(value);
            Assert.Single(value);
            Assert.Equal("Hello", value[0].Content);
        }

        [Fact]
        public async Task GetMessages_ReturnsInternalServerError_OnException()
        {
            _mockChatService
                .Setup(service => service.GetMessagesAsync())
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _controller.GetMessages();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task RemoveUserFromChat_ReturnsOkResult()
        {
            var displayName = "UserTest";

            _mockChatService
                .Setup(service => service.RemoveUserFromChatThreadAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<bool>.Success(true));

            var result = await _controller.RemoveUserFromChat(displayName);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public async Task RemoveUserFromChat_ReturnsBadRequest_WhenNoUserNameGiven()
        {
            var displayName = "";
            _mockChatService
                .Setup(service => service.RemoveUserFromChatThreadAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<bool>.Failure("No username has been given"));

            var result = await _controller.RemoveUserFromChat(displayName);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
        }
    }
}