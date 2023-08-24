using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class Module
    {
        [Key]
        public int ModuleId { get; set; }
        [Required]
        [DisplayName("Module")]
        public string ModuleName { get; set; }
    }
}
