using System.ComponentModel.DataAnnotations;

namespace JobApplicationManagement.Views
{
    public class StatusHistoryView
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public string Status { get; set; }


        public string? Comment { get; set; }
    }
}
