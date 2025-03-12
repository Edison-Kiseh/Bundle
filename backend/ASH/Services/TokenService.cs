using Azure.Communication;
using Azure.Communication.Identity;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;

namespace ASH.Services
{
    public class TokenService(CommunicationIdentityClient identityClient)
    {
        private readonly CommunicationIdentityClient _identityClient = identityClient;
        private static readonly ConcurrentDictionary<string, string> UserTokens = new();

        public string GetOrRefreshToken(string userId)
        {
            if (UserTokens.TryGetValue(userId, out var token) && !IsTokenExpired(token))
            {
                return token;
            }

            var tokenResponse = _identityClient.GetToken(new CommunicationUserIdentifier(userId), [CommunicationTokenScope.Chat]);
            token = tokenResponse.Value.Token;

            UserTokens[userId] = token;

            return token;
        }

        private static bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (handler.ReadToken(token) is not JwtSecurityToken jwtToken)
            {
                return true;
            }

            return jwtToken.ValidTo < DateTime.UtcNow;
        }
    }
}