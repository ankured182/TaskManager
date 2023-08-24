using System.ComponentModel;

namespace TaskManager.Models
{
    public class RptTask
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public int Assigned { get; set; }

        public int Pending { get; set; }
        public int Completed { get; set; }

        [DisplayName("Started")]
        public int StartedNum { get; set; }
        [DisplayName("Tentative Duration(C)")]
        public string TentDurationHrsC { get; set; }
        [DisplayName("Actual Duration(C)")]
        public string ActualDuration { get; set; }

        [DisplayName("Tentative Duration(P)")]
        public string TentDurationHrsP { get; set; }

    }
}
