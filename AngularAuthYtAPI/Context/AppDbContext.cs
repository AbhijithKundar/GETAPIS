using AngularAuthYtAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AngularAuthYtAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<PlanTypes> PlanTypes { get; set; }
        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("users");
            builder.Entity<PlanTypes>().ToTable("plantypes");
            builder.Entity<Member>().ToTable("members");
        }
    }
}
