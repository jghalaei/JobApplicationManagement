using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobApplicationManagement.Models
{
    public class StatusHistory
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid JobApplicationId{ get; set; }
        
        [ForeignKey("JobApplicationId")]
        public JobApplication? Application { get; set; }
               
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public string? Comment { get; set; }

public StatusHistory(string status="Created", string comment="")
        {
            this.Status = status;
            this.Date = DateTime.Now;
            this.Comment= comment;
        }

        public bool IsValid()
        {
            return !(Application == null || String.IsNullOrWhiteSpace(Status) || Date.Year < 2000);
                
        }
    }
}
