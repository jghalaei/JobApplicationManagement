namespace JobApplicationManagement.Views
{
    public record JobApplicationView
    {
        public Guid ApplicationId { get; set; }
        public string Title { get; set; }
        public string Jobfield { get; set; }
        public string? Url { get; set; }
        public string? Comment { get; set; }

    }
}
