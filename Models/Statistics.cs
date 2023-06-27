using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace hirelink.Models {
    public class Statistics {
        public Statistics() {
            JobsCount = 0;
            CandidatesCount = 0;
            HiredCount = 0;
            RejectedCount = 0;
            OpenJobsCount = 0;
            ClosedJobsCount = 0;
            OtherProgressCount = 0;
        }

        public int JobsCount { get; set; }
        public int CandidatesCount { get; set; }
        public int HiredCount { get; set; }
        public int RejectedCount { get; set; }
        public int OpenJobsCount { get; set; }
        public int ClosedJobsCount { get; set; }
        public int OtherProgressCount { get; set; }
    }
}
