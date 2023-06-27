using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;


namespace hirelink.Models {
    public class CandidateTimeLine {

        public CandidateTimeLine() {
            // Initialize nullable properties here
            ActivityDoneBy = string.Empty;
            ActivityPerformed = string.Empty;
        }

        public int JobId { get; set; }
        public int CandidateId { get; set; }
        public string ActivityDoneBy { get; set; }
        public DateTime? ActivityDate { get; set; }
        public string ActivityPerformed { get; set; }

    }

}
