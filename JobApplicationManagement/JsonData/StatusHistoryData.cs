using System.ComponentModel.DataAnnotations;

namespace JobApplicationManagement.JsonData
{
    public class StatusHistoryData
    {
        [Required]
        public string Status { get; set; }
        public string? Comment { get; set; }

    }
}
