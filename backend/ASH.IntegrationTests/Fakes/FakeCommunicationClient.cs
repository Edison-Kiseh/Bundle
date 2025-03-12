using Azure;
using Azure.Communication;
using Azure.Communication.Identity;
using Azure.Core;
using Moq;

namespace ASH.IntegrationTests.Fakes;

public class FakeCommunicationClient : CommunicationIdentityClient
{
    public override Response<AccessToken> GetToken(
        CommunicationUserIdentifier communicationUser,
        IEnumerable<CommunicationTokenScope> scopes,
        CancellationToken cancellationToken = default
    )
    {
        var responseMock = new Mock<Response>();
        responseMock.SetupGet(r => r.Status).Returns(200);

        return Response.FromValue(new AccessToken(), responseMock.Object);
    }
}