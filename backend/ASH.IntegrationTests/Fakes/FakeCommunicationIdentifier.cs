using Azure.Communication;

namespace ASH.IntegrationTests.Fakes;

public class FakeCommunicationIdentifier : CommunicationIdentifier
{
    public override bool Equals(CommunicationIdentifier other)
    {
        return true;
    }
}