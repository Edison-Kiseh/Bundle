using System.Collections.Concurrent;
using Azure;
using Azure.Communication.Chat;
using Azure.Communication;
using Moq;

namespace ASH.IntegrationTests.Fakes;

public class FakeChatThreadClient : ChatThreadClient
{
    private readonly ConcurrentDictionary<string, ChatParticipant> _participants = new();

    public IReadOnlyDictionary<string, ChatParticipant> Participants => _participants.ToDictionary(kv => kv.Key, kv => kv.Value);
    private Dictionary<string, FakeChatClientError> Errors { get; init; } = new Dictionary<string, FakeChatClientError>();

    public FakeChatThreadClient(IEnumerable<ChatParticipant> initialParticipants = null)
    {
        if (initialParticipants != null)
        {
            foreach (var participant in initialParticipants)
            {
                _participants.TryAdd(participant.User.RawId, participant);
            }
        }
    }

    public override Task<Response<AddChatParticipantsResult>> AddParticipantsAsync(
        IEnumerable<ChatParticipant> participants,
        CancellationToken cancellationToken = default
    )
    {
        FakeChatClientError? error = Errors.GetErrorForMethod(nameof(AddParticipantsAsync));

        if (error != null && error.IsException)
        {
            throw new Exception(error.Message);
        }

        foreach (var participant in participants)
        {
            _participants.TryAdd(participant.User.RawId, participant);
        }

        var responseMock = new Mock<Response>();
        responseMock.SetupGet(r => r.Status).Returns(error?.StatusCode ?? 200);

        var addChatParticipantsResult = ChatModelFactory.AddChatParticipantsResult(
            Array.Empty<ChatError>()
        );

        return Task.FromResult(Response.FromValue(addChatParticipantsResult, responseMock.Object));
    }


    public override Task<Response> RemoveParticipantAsync(CommunicationIdentifier identifier, CancellationToken cancellationToken = default)
    {
        FakeChatClientError? error = Errors.GetErrorForMethod(nameof(RemoveParticipantAsync));

        if (error != null && error.IsException)
        {
            throw new Exception(error.Message);
        }

        _participants.TryRemove(identifier.RawId, out _);

        var responseMock = new Mock<Response>();
        responseMock.SetupGet(r => r.Status).Returns(error?.StatusCode ?? 200);

        return Task.FromResult(responseMock.Object);
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

