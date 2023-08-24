using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskPriority
    {

        [Key]
        public int PriorityId { get; set; }

        [Required]
        [DisplayName("Priority")]
        public string PriorityName { get; set; }

    }
}
