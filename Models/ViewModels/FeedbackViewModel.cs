namespace hirelink.Models.ViewModels {
    public class FeedbackViewModel {
        public int? FeedbackId { get; set; }
        public int? CandidateId { get; set; }
        public string? Round { get; set; }
        public string? InterviewerEmail { get; set; }
        public string? Comments { get; set; }
        public DateTime FeedbackDate { get; set; }
        public string? InterviewerName { get; set; }
        public List<FeedbackTopicViewModel>? Topics { get; set; }
    }

    public class FeedbackTopicViewModel {
        public int? FeedbackTopicId { get; set; }
        public string? TopicName { get; set; }
        public int? TopicRating { get; set; }
    }
}
