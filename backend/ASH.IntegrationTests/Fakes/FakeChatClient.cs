using Azure;
using Azure.Communication.Chat;
using Moq;

namespace ASH.IntegrationTests.Fakes;

public class FakeChatClient : ChatClient
{
    private Dictionary<string, FakeChatClientError> Errors { get; init; } = new Dictionary<string, FakeChatClientError>();

    public override Task<Response<CreateChatThreadResult>> CreateChatThreadAsync(
        string topic,
        IEnumerable<ChatParticipant> participants = null,
        string idempotencyToken = null,
        CancellationToken cancellationToken = default
    )
    {
        FakeChatClientError? error = Errors.GetErrorForMethod(nameof(CreateChatThreadAsync));

        if (error != null && error.IsException)
        {
            throw new Exception(error.Message);
        }

        var responseMock = new Mock<Response>();
        responseMock.SetupGet(r => r.Status).Returns(error?.StatusCode ?? 200);

        var identifier = new FakeCommunicationIdentifier();
        var chatProps = ChatModelFactory.ChatThreadProperties(
            "test-id",
            "topic",
            DateTimeOffset.UtcNow,
            identifier,
            DateTimeOffset.UtcNow.AddHours(1)
        );
        var chatThreadResult = ChatModelFactory.CreateChatThreadResult(
            chatProps,
            Array.Empty<ChatError>()
        );

        return Task.FromResult(Response.FromValue(chatThreadResult, responseMock.Object));
    }

    public void SeedError(
        string method,
        string message,
        bool isException = false,
        int statusCode = 200
    )
    {
        Errors.Add(method, new FakeChatClientError(method, message, isException, statusCode));
    }
}

public record FakeChatClientError(string Method, string Message, bool IsException, int StatusCode);