namespace CoffeProjectWebUI.Models
{
    public class RoleAssignmentViewModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public List<string> CurrentRoles { get; set; } = new List<string>();
        public string? NewRole { get; set; }
    }
}
