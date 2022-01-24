using JobApplicationManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace JobApplicationManagement.JsonData
{
    public class JobApplicationData : IValidatableObject
    {
        private string? _title, _jobField;
        public string Title 
        {
            get=>_title; 
            set=>_title=ValidateProperty(value,nameof(Title)); 
        }
        public string JobField 
        {
            get=>_jobField; 
            set=>_jobField=ValidateProperty(value,nameof(JobField)); 
        }
        public string Url { get; set; }
        public string Comment { get; set; }

        private string ValidateProperty(string name, string propertyName)
        {
            return string.IsNullOrEmpty(name) ? throw new InvalidOperationException($"Could Not Set {propertyName}") : name;
                }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new();
            if (string.IsNullOrEmpty(Title))
                results.Add(new ValidationResult("Please provice Title"));
            if (string.IsNullOrEmpty(JobField))
                results.Add(new ValidationResult("Please Provide JobField"));
            return results;
        }
    }
}
