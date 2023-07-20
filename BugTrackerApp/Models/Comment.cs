using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerApp.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        [DisplayName("Ticket")]
        public int TicketId { get; set; }                       // foreign key to Ticket Model
        public Ticket Ticket { get; set; }                      // navigational property to Ticket Model
        public DateTime Created { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]                                  // This is optional. Not necessary to specify foreign key because the namining convention is followed.
                                                                // Foreign key has same name as navigation property with Id appended.
        public string UserId { get; set; }                     
        public User User { get; set; }                  // navigational property to user Model

    }
}
