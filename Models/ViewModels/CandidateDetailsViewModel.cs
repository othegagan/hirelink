using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace hirelink.Models.ViewModels {
    public class CandidateDetailsViewModel {
        public Candidate? Candidate { get; set; }
        public List<FeedbackViewModel>? Feedbacks { get; set; }
        public List<CandidateTimeLine>? TimeLines { get; set; }
    }
}

