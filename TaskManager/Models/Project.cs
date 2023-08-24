using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        [DisplayName("Project")]
        public string ProjectName { get; set; }

        public string ActiveStatus { get; set; }
    }
}
