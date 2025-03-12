using Azure.Communication.Chat;
using Azure.Communication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ASH.Models;
using ASH.Exceptions;
using Microsoft.AspNetCore.SignalR;

namespace ASH.Services
{
    public class ChatService(ChatClient chatClient, ILogger<ChatService> logger, IOptions<CommunicationServices> communicationServices, IHubContext<ChatHub> hubContext, IUserService userService) : IChatService
    {
        private readonly ChatClient _chatClient = chatClient;
        private readonly ILogger<ChatService> _logger = logger;
        private readonly CommunicationServices _communicationServices = communicationServices.Value;
        private readonly IHubContext<ChatHub> _hubContext = hubContext;
        private readonly IUserService _userService = userService;
        private static readonly Dictionary<string, UserInfo> _userDictionary = new();
        public static string? ThreadId { get; private set; }
        public static IReadOnlyDictionary<string, UserInfo> UserDictionary => _userDictionary;

        public async Task<IActionResult> CreateChatThread(string topic)
        {
            try
            {
                if (ThreadId != null)
                {
                    return new OkObjectResult(new { ThreadId = ThreadId });
                }

                var createChatThreadResponse = await _chatClient.CreateChatThreadAsync(
                    topic: topic,
                    participants: []
                );

                ThreadId = createChatThreadResponse.Value.ChatThread.Id;
                return new OkObjectResult(new { ThreadId = ThreadId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the chat thread.");
                return new StatusCodeResult(500);
            }
        }

        public async Task SendMessageAsync(string message, string displayName)
        {
            try
            {
                if (!UserDictionary.TryGetValue(displayName, out var user))
                {
                    throw new InvalidOperationException("User not found");
                }
                if (ThreadId == null)
                {
                    throw new InvalidOperationException("ThreadId is null");
                }
                using var communicationTokenCredential = new CommunicationTokenCredential(user.Token);
                var chatUserClient = new ChatClient(new Uri(_communicationServices.EndpointUrl), communicationTokenCredential);
                ChatThreadClient chatThreadClient = chatUserClient.GetChatThreadClient(threadId: ThreadId);

                var sendMessageRequest = new SendChatMessageOptions
                {
                    Content = message,
                    SenderDisplayName = displayName
                };

                await chatThreadClient.SendMessageAsync(sendMessageRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending the message.");
            }
        }

        public async Task<IEnumerable<ChatMessageItem>> GetMessagesAsync()
        {
            var chatThreadClient = _chatClient.GetChatThreadClient(ThreadId);
            var messages = new List<ChatMessageItem>();

            await foreach (var message in chatThreadClient.GetMessagesAsync())
            {
                if (!string.IsNullOrEmpty(message.SenderDisplayName))
                {
                    var customMessage = new ChatMessageItem
                    {
                        Id = message.Id,
                        Type = message.Type,
                        SequenceId = message.SequenceId,
                        Content = message.Content?.Message ?? string.Empty,
                        SenderDisplayName = message.SenderDisplayName,
                        CreatedOn = message.CreatedOn,
                        SenderRawId = message.Sender?.RawId ?? string.Empty,
                        DeletedOn = message.DeletedOn,
                        EditedOn = message.EditedOn
                    };
                    messages.Add(customMessage);
                }
            }

            return messages;
        }

        public async Task<string> AddUserToChatAsync(AddUserToChatRequest request)
        {
            string? threadId = ThreadId ?? throw new InvalidOperationException("Thread Id is not set");

            if (await IsDisplayNamePresentInThreadAsync(threadId, request.DisplayName))
            {
                throw new ConflictException("DisplayName is already present in the thread.");
            }
            var (userId, token) = await _userService.CreateUserAsync();
            _userDictionary[request.DisplayName] = new UserInfo
            {
                UserId = userId,
                Token = token,
                DisplayName = request.DisplayName
            };

            ChatThreadClient chatThreadClient = _chatClient.GetChatThreadClient(threadId: threadId);

            var userIdentifier = new CommunicationUserIdentifier(id: userId);
            var participant = new ChatParticipant(userIdentifier) { DisplayName = request.DisplayName };

            await chatThreadClient.AddParticipantsAsync([participant]);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "System", $"{request.DisplayName} has joined the chat.", true);
            return userId;
        }

        public async Task<Result<bool>> RemoveUserFromChatThreadAsync(string displayName)
        {
            if (ThreadId == null)
            {
                return Result<bool>.Failure("Thread Id is not set");
            }

            if (!UserDictionary.TryGetValue(displayName, out var user))
            {
                throw new NotFoundException("The user is not present in the thread.");
            }

            if (_userDictionary.ContainsKey(displayName))
            {
                _userDictionary.Remove(displayName);
            }

            var chatThreadClient = chatClient.GetChatThreadClient(ThreadId);
            var userIdentifier = new CommunicationUserIdentifier(user.UserId);

            await chatThreadClient.RemoveParticipantAsync(userIdentifier);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "System", $"{displayName} has left the chat.", true);
            return Result<bool>.Success(true);
        }

        public async Task<bool> IsDisplayNamePresentInThreadAsync(string threadId, string displayName)
        {
            ChatThreadClient chatThreadClient = _chatClient.GetChatThreadClient(threadId);
            var participants = new List<ChatParticipant>();
            await foreach (var participant in chatThreadClient.GetParticipantsAsync())
            {
                participants.Add(participant);
            }

            return participants.Any(p => p.DisplayName == displayName);
        }
    }
}