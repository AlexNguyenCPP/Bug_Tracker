using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerApp.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public int ProjectId { get; set; } // Foreign key property

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        public string Developer { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }

        [ForeignKey("ProjectId")] // Foreign key for 'Project'
        public Project Project { get; set; } // Navigation property

        public DateTime Created { get; set; }
    }

}
