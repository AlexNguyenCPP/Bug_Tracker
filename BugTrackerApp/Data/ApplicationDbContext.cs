using BugTrackerApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Project model
        public DbSet<Project> Projects { get; set; }

        // Project model
        //public DbSet<BugTrackerApp.Models.Ticket>? Ticket { get; set; }
        public DbSet<Ticket> Ticket {  get; set; }
    }
}