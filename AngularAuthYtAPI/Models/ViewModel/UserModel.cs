namespace AngularAuthYtAPI.Models.ViewModel
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public int PlanType { get; set; }
        public string? Referal { get; set; }
    }
}
