using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerApp.Models
{
    public class Attachment
    {
        [Key]
        public int Id { get; set; } 
        public int TicketId { get; set; } // foreign key to Ticket
        public string FileName { get; set; }
        public byte[] FileData { get; set; }    
        public string ContentType { get; set; }
        public Ticket Ticket { get; set; }

        [ForeignKey("UserId")]                                  // This is optional. Not necessary to specify foreign key because the namining convention is followed.
                                                                // Foreign key has same name as navigation property with Id appended.
        public string UserId { get; set; }
        public User User { get; set; }                  // navigational property to user Model


    }
}
