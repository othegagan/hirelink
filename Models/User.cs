using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace hirelink.Models {
    public class User {

        public User() {
            //    By initializing  with string.Empty (or any other non-null value) in the constructor, you indicate that it should never be null
            Username = string.Empty;
            Password = string.Empty;
            Name = string.Empty;
            Role = string.Empty;
        }

        [Key]
        public int SuperId { get; set; }

        [Required]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }
    }
}
