using Microsoft.AspNetCore.Identity;

namespace BugTrackerApp.Models
{
    public class User : IdentityUser
    {
        public ICollection<Project> Projects { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }

}
