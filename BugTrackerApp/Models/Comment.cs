using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerApp.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        public int TicketId { get; set; }                       // foreign key to Ticket Model
        public Ticket Ticket { get; set; }                      // navigational property to Ticket Model
        public DateTime Created { get; set; } = DateTime.Now;
        public string UserId { get; set; }                      // foreign key to User Model
        public IdentityUser User { get; set; }                  // navigational property to user Model

    }
}
