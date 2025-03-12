using ASH.IntegrationTests.Fixture;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace ASH.IntegrationTests;

[Collection("Web Application Fixture Collection")]
public class IntegrationTests(WebApplicationFixture Fixture)
{
    [Fact]
    public async Task CreateChat_Returns200_WhenChatClientReturnsThread()
    {
        var response = await Fixture.HttpClient.PostAsJsonAsync("/chat", new CreateChatThreadRequestTest("my-test-topic"));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RemoveParticipantAsync_Returns404_WhenParticipantDoesNotExist()
    {
        await CreateChatThread("remove-nonExistentUser-thread");

        var response = await Fixture.HttpClient.DeleteAsync("/chat/user/nonExistentUser");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SendMessageAsync_Returns200_WhenMessageIsSent()
    {
        await CreateChatThread("my-test-topic");

        var user = new AddUserToChatRequestTest("user2");
        await Fixture.HttpClient.PostAsJsonAsync("/chat/user", user);

        var message = new SendMessageRequestTest("Hello, World!", user.DisplayName);

        var response = await Fixture.HttpClient.PostAsJsonAsync("/chat/message", message);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    private async Task<string> CreateChatThread(string topic)
    {
        var response = await Fixture.HttpClient.PostAsJsonAsync("/chat", new CreateChatThreadRequestTest(topic));

        response.StatusCode.Should().Be(HttpStatusCode.OK,
            $"Failed to create thread. Response: {await response.Content.ReadAsStringAsync()}");

        var responseContent = await response.Content.ReadFromJsonAsync<CreateChatThreadResponse>();

        return responseContent.ThreadId;
    }
}