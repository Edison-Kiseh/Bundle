using Azure.Communication.Identity;
using Microsoft.Extensions.Options;
using ASH.Models;

namespace ASH.Services
{
    public class UserService(IOptions<CommunicationServices> communicationServices) : IUserService
    {
        private readonly CommunicationIdentityClient _identityClient = new(communicationServices.Value.ResourceConnectionString);

        public async Task<(string UserId, string Token)> CreateUserAsync()
        {
            var userResponse = await _identityClient.CreateUserAsync();
            var tokenResponse = await _identityClient.GetTokenAsync(userResponse.Value, [CommunicationTokenScope.Chat]);

            var userId = userResponse.Value.Id;
            var token = tokenResponse.Value.Token;

            return (userId, token);
        }
    }
}