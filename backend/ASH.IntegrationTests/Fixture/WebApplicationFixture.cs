using ASH.IntegrationTests.Fakes;

namespace ASH.IntegrationTests.Fixture;

public class WebApplicationFixture : IAsyncLifetime
{
    public CustomWebApplicationFactory CustomWebApplicationFactory { get; init; }
    public HttpClient HttpClient { get; init; }
    public FakeChatClient FakeChatClient { get; init; }
    public FakeChatThreadClient FakeChatThreadClient { get; init; }
    public WebApplicationFixture()
    {
        FakeChatClient = new FakeChatClient();
        FakeChatThreadClient = new FakeChatThreadClient();
        CustomWebApplicationFactory = new CustomWebApplicationFactory(FakeChatClient, FakeChatThreadClient);
        HttpClient = CustomWebApplicationFactory.CreateClient();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}