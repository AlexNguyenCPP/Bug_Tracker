using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerApp.Models
{
    public class Ticket
    {
        // types of statuses for the Ticket
        public enum StatusType
        {
            New,
            Open,
            InProgress,
            Resolved,
            AdditionalInfoRequired
        }

       // A better method that doesn't repeat code is to use the [Description] tag to name the enum fields what you want them to be called,
       // then in the views, access those fields with the use of a helper method that obtains the description instead of the original name.
       // We didn't do it here because we don't expect to change the Ticket types ever, but if you were to change the types, you'd have to
       // change it in the views as well as the enum
        public enum TicketType
        {
            BugsErrors,
            FeatureRequests,
            OtherComments,
            TrainingDocumentRequests
        }

        public enum PriorityType
        {
            None,
            Low,
            Medium,
            High
        }

		[Key]
        public int Id { get; set; }

        [DisplayName("Project")]
        public int ProjectId { get; set; } // Foreign key property

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        public string Developer { get; set; }
        public PriorityType Priority { get; set; }
        public StatusType Status { get; set; }
        public TicketType Type { get; set; }

        [ForeignKey("ProjectId")] // Foreign key for 'Project'

        // ? required to make the property nullable. By default, certain types in .net6 are non-nullable 
        // The Project property will be automatically linked to the ProjectID foreign key later on
        // making it nullable prevents "Project field required" errors
        public Project Project { get; set; } // Navigation property

        [DisplayName("Created")]
        public DateTime Created { get; set; } = DateTime.Now;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }

}
