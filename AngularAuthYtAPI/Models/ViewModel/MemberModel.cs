namespace AngularAuthYtAPI.Models.ViewModel
{
    public class MemberModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? PlanType { get; set; }
        public DateTime JoiningDate { get; set; }
        public string? MemberFullName { get; set; }
    }
}
