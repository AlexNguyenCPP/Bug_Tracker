﻿using System.ComponentModel.DataAnnotations;
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

        // ? required to make the property nullable. By default, certain types in .net6 are non-nullable 
        // The Project property will be automatically linked to the ProjectID foreign key later on
        // making it nullable prevents "Project field required" errors
        public Project? Project { get; set; } // Navigation property

        public DateTime Created { get; set; } = DateTime.Now;
    }

}
