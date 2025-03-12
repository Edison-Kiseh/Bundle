namespace ASH.Models
{
    public class CommunicationServices
    {
        public required string ResourceConnectionString { get; init; }
        public required string EndpointUrl { get; init; }
        public required string AdminUserId { get; init; }
    }
}
