using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace hirelink.Models {

    public class Candidate {
        public Candidate() {
             //    By initializing  with string.Empty (or any other non-null value) in the constructor, you indicate that it should never be null
            Name = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Address = string.Empty;
            Gender = string.Empty;
            Education = string.Empty;
            InstituteName = string.Empty;
            Designation = string.Empty;
            Skills = string.Empty;
            PresentCompany = string.Empty;
            Progress = string.Empty;
            ResumeFileLink = string.Empty;
        }

        [Key]
        [Required]
        public int? CandidateId { get; set; }

        public int? JobID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number.")]
        [DisplayName("Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [DisplayName("Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [DisplayName("Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Education is required.")]
        [DisplayName("Education")]
        public string Education { get; set; }

        [Required(ErrorMessage = "Institute name is required.")]
        [DisplayName("Institute Name")]
        public string InstituteName { get; set; }

        [Required(ErrorMessage = "Passing year is required.")]
        [DisplayName("Passing Year")]
        public int PassingYear { get; set; }

        [Required(ErrorMessage = "Experience is required.")]
        [DisplayName("Experience (in years)")]
        public int Experience { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        [DisplayName("Designation")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Skills are required.")]
        [DisplayName("Skills")]
        public string Skills { get; set; }

        [Required(ErrorMessage = "Current CTC is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid current CTC.")]
        [DisplayName("Current CTC (in LPA)")]
        public decimal CurrentCTC { get; set; }

        [Required(ErrorMessage = "Expected CTC is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid expected CTC.")]
        [DisplayName("Expected CTC (in LPA)")]
        public decimal ExpectedCTC { get; set; }

        [Required(ErrorMessage = "Present company is required.")]
        [DisplayName("Present Company")]
        public string PresentCompany { get; set; }

        [Required(ErrorMessage = "Progress is required.")]
        [DisplayName("Progress")]
        public string Progress { get; set; }

        [Required(ErrorMessage = "Resume file link is required.")]
        [DisplayName("Resume File Link")]
        public string ResumeFileLink { get; set; }

        [DisplayName("Status")]
        public string? Status { get; set; }

        [DisplayName("Details")]
        public string? Details { get; set; }

        [DisplayName("Comments")]
        public string? Comments { get; set; }

    }


}
