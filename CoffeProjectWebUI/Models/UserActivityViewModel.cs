namespace CoffeProjectWebUI.Models
{
    public class UserActivityViewModel
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? ActivityType { get; set; }
        public string? EntityName { get; set; }
        public int? EntityId { get; set; }
        public string? Description { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? IpAddress { get; set; }
        public DateTime ActivityDate { get; set; }
    }
}
