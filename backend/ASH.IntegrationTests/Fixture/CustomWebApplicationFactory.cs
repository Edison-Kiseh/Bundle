using ASH.IntegrationTests.Fakes;
using Azure.Communication.Chat;
using Azure.Communication.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASH.IntegrationTests.Fixture;

public class CustomWebApplicationFactory(FakeChatClient fakeChatClient, FakeChatThreadClient fakeChatThreadClient) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configuration =>
        {
            var path = Path.Combine(Environment.CurrentDirectory, "local.appsettings.json");
            configuration.AddJsonFile(path, false);
        });

        builder.ConfigureServices(configureServices =>
        {
            configureServices.AddSingleton<CommunicationIdentityClient>(new FakeCommunicationClient());

            configureServices.AddSingleton<ChatClient>(fakeChatClient);
            configureServices.AddSingleton<ChatThreadClient>(fakeChatThreadClient);
        });

        base.ConfigureWebHost(builder);
    }
}