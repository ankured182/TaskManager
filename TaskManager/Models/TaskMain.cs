using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TaskManager.Models
{
   
    public class TaskMain
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Task")]
        public string TaskName { get; set; }

        [DisplayName("Description")]
        public string? TaskDescrp { get; set; }

        //public string? Priority { get; set; }


        [ForeignKey("PriorityId")]
        [DisplayName("Priority")]
        public virtual TaskPriority TaskPriorities { get; set; }
        public int PriorityId { get; set; }


        [ForeignKey("ProjectId")]
        [DisplayName("Project")]
        public virtual Project Projects { get; set; }
        public int ProjectId { get; set; }


        [ForeignKey("ModuleId")]
        [DisplayName("Module")]
        public virtual Module Modules { get; set; }
        public int ModuleId { get; set; }

        [DisplayName("Created By")]
        public string CreateBy { get; set; }

        [DisplayName("Assigned To")]
        public string AssignedTo { get; set; }
        [DisplayName("Status")]
        public string CurrentStatus { get; set; }

        [DisplayName("Create Date")]
        public DateTime DateCreated { get; set; }

        [DisplayName("Intended Start Date")]
        public DateTime? IntendedStartDate { get; set; }

        [DisplayName("Est. Duration(Hrs)")]
        public double? DurationHrs { get; set; }
        [DisplayName("Actual Start Date")]
        public DateTime? ActualDateStarted { get; set; }
        [DisplayName("Actual End Date")]
        public DateTime? ActualDateEnded { get; set; }

        //[DisplayName("User")]
        //public virtual MyUser MyUsers { get; set; }
        //public string MyUserId { get; set; }
    }

    

    

   
}
