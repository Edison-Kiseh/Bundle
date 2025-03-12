namespace ASH.Services
{
    public interface IUserService
    {
        Task<(string UserId, string Token)> CreateUserAsync();
    }
}