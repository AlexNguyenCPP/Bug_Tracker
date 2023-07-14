using System.ComponentModel.DataAnnotations;

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


    }
}
