namespace ASH.Models
{
    public class ChatMessageItem
    {
        public required string Id { get; set; }
        public required object Type { get; set; }
        public required string SequenceId { get; set; }
        public required string Content { get; set; }
        public required string SenderDisplayName { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public required string SenderRawId { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public DateTimeOffset? EditedOn { get; set; }
    }
}