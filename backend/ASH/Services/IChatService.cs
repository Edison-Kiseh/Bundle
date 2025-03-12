using ASH.Models;
using ASH.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ASH.Services
{
    public interface IChatService
    {
        Task<IActionResult> CreateChatThread(string topic);
        Task SendMessageAsync(string message, string displayName);
        Task<IEnumerable<ChatMessageItem>> GetMessagesAsync();
        Task<string> AddUserToChatAsync(AddUserToChatRequest request);
        Task<bool> IsDisplayNamePresentInThreadAsync(string threadId, string displayName);
        Task<Result<bool>> RemoveUserFromChatThreadAsync(string displayName);
    }
}