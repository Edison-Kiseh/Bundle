using ASH.Models;
using ASH.Services;
using Azure.Communication;
using Azure.Communication.Chat;
using Azure.Communication.Identity;
using Microsoft.Extensions.Options;

namespace ASH
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                var frontendUrl = builder.Configuration.GetSection("FrontendUrl").Value;
                var origins = string.IsNullOrEmpty(frontendUrl) ? [] : new[] { frontendUrl };
                options.AddPolicy("AllowSpecificOrigins",
                    policyBuilder =>
                    {
                        policyBuilder.WithOrigins(origins)
                                     .AllowAnyMethod()
                                     .AllowAnyHeader()
                                     .AllowCredentials();
                    });
            });

            builder
                .Configuration.AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: true
                )
                .AddJsonFile(
                    $"appsettings.{builder.Environment.EnvironmentName}.json",
                    optional: true,
                    reloadOnChange: true
                )
                .AddEnvironmentVariables();

            builder.Services.Configure<CommunicationServices>(
                builder.Configuration.GetSection("CommunicationServices")
            );

            builder.Services.AddSingleton(sp =>
            {
                var communicationServices = sp.GetRequiredService<
                    IOptions<CommunicationServices>
                >().Value;
                return new CommunicationIdentityClient(
                    communicationServices.ResourceConnectionString
                );
            });

            builder.Services.AddSingleton<TokenService>();
            builder.Services.AddSingleton<IChatService, ChatService>();

            builder.Services.AddSingleton<ChatClient>(sp =>
            {
                var communicationServices = sp.GetRequiredService<
                    IOptions<CommunicationServices>
                >().Value;
                var tokenService = sp.GetRequiredService<TokenService>();
                var token = tokenService.GetOrRefreshToken(communicationServices.AdminUserId);
                var communicationTokenCredential = new CommunicationTokenCredential(token);
                return new ChatClient(
                    new Uri(communicationServices.EndpointUrl),
                    communicationTokenCredential
                );
            });

            builder.Services.AddSingleton<ChatService>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSignalR();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigins");
            app.UseAuthorization();
            app.MapControllers();
            app.MapHub<ChatHub>("/chatHub");
            app.Run();
        }
    }
}
