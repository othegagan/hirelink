using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace hirelink.Models {
    public class Job {

        public Job() {
            //    By initializing  with string.Empty (or any other non-null value) in the constructor, you indicate that it should never be null
            JobTitle = string.Empty;
            Department = string.Empty;
            Location = string.Empty;
            Skills = string.Empty;
            ProjectName = string.Empty;
            ClientName = string.Empty;
            OwnerName = string.Empty;
            JobType = string.Empty;
            JobStatus = string.Empty;
            PostedBy = string.Empty;
        }

        [Key]
        [Display(Name = "Job ID")]
        public int JobId { get; set; }

        [Display(Name = "Job Title")]
        [Required(ErrorMessage = "Please enter the job title.")]
        public string JobTitle { get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "Please enter the department.")]
        public string Department { get; set; }

        [Display(Name = "Location")]
        [Required(ErrorMessage = "Please enter the location.")]
        [RegularExpression(@"^[a-zA-Z\s\p{P}]+$", ErrorMessage = "Location can't have special characters.")]
        public string Location { get; set; }

        [Display(Name = "Skills")]
        [Required(ErrorMessage = "Please enter some skills.")]
        public string Skills { get; set; }

        [Display(Name = "Number of Openings")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Number of Openings must be a atleast 1.")]
        public int NumberOfOpenings { get; set; }

        [Display(Name = "Project Name")]
        [Required(ErrorMessage = "Please enter the project name.")]
        public string ProjectName { get; set; }

        [Display(Name = "Client Name")]
        [Required(ErrorMessage = "Please enter the client name.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Client Name can only contain alphanumeric characters and spaces.")]
        public string ClientName { get; set; }

        [Display(Name = "Owner Name")]
        [Required(ErrorMessage = "Please enter the owner name.")]
        [RegularExpression(@"^[a-zA-Z\s\p{P}]+$", ErrorMessage = "Owner Name can only contain alphabetic characters and spaces.")]
        public string OwnerName { get; set; }

        [Display(Name = "Job Type")]
        [RegularExpression(@"^(Full-time|Intern|Contract)$", ErrorMessage = "Select type of job.")]
        public string JobType { get; set; }

        [Display(Name = "Job Status")]
        public string JobStatus { get; set; }

        [Display(Name = "Posted On")]
        public DateTime PostedOn { get; set; }

        [Display(Name = "Posted By")]
        public string PostedBy { get; set; }

        [Display(Name = "Closed On")]
        public DateTime? ClosedOn { get; set; }

        [Display(Name = "Closed By")]
        public string? ClosedBy { get; set; }

        [Display(Name = "Candidate Count")]
        public int CandidateCount { get; set; }

        [Display(Name = "Hired Count")]
        public int HiredCount { get; set; }

        [Display(Name = "Rejected Count")]
        public int RejectedCount { get; set; }

    }
}