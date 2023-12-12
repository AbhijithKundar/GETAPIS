using System.ComponentModel.DataAnnotations.Schema;

namespace AngularAuthYtAPI.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public  int? ParentId { get; set; }
        [ForeignKey("PlanTypes")]
        public int PlanId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime JoiningDate { get; set; }
        public virtual PlanTypes? PlanTypes { get; set; }
        public virtual User? User { get; set; }

    }
}
