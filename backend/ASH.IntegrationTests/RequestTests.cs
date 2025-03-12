using Azure.Communication.Chat;

namespace ASH.IntegrationTests;

public record CreateChatThreadRequestTest(string Topic);

public record AddUserToChatRequestTest(string DisplayName);

public record CreateChatThreadResponse(string ThreadId);

public record SendMessageRequestTest(string DisplayName, string Message);

public record GetChatThreadParticipantsResult(IReadOnlyList<ChatParticipant> participants);