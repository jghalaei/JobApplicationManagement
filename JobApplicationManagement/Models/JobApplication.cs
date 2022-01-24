using System.ComponentModel.DataAnnotations;

namespace JobApplicationManagement.Models
{
    public class JobApplication
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string  CreatedBy { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string JobField { get; set; }

        [Required(AllowEmptyStrings =false)]
        public string Title { get; set; }
        public string URL { get; set; }
        public string Comment { get; set; }


       public List<StatusHistory> StatusHistories { get; set; }

        public JobApplication()
        {
            StatusHistories = new();
        }
        public bool IsValid()
        {
            return !(CreatedBy == null || JobField == null || Title == null);

        }
    }
}
