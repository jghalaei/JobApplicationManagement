using System.ComponentModel.DataAnnotations;

namespace JobApplicationManagement.Models
{
    public class User
    {
        [Key]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Roles { get; set; }
    }
}
