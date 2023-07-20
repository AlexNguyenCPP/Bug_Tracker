using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerApp.Models
{
    public class Project
    {
        // make sure Ticket property is never null 
        public Project()
        {
            Tickets = new List<Ticket>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayName("Created")]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        // tickets and users linked to the project
        public ICollection<Ticket> Tickets { get; set; }

        [ForeignKey("UserId")]                                  // This is optional. Not necessary to specify foreign key because the namining convention is followed.
                                                                // Foreign key has same name as navigation property with Id appended.
        public string UserId { get; set; }
        public User User { get; set; }                  // navigational property to user Model
    }
}
