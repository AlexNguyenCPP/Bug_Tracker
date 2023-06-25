using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerApp.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        // tickets and users linked to the project
        //public ICollection<Ticket> Tickets { get; set; }
        //public ICollection<IdentityUser> Users { get; set; }
    }
}
