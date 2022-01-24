using JobApplicationManagement.JsonData;
using JobApplicationManagement.Models;
using JobApplicationManagement.Repositories;
using JobApplicationManagement.Views;

namespace JobApplicationManagement.Services
{
    public class JobApplicationService
    {
        private JobApplicationRepository jobAppRepository;
        public JobApplicationService(JobApplicationRepository jobAppRepository)
        {
            this.jobAppRepository = jobAppRepository;
        }

        public async Task<(bool result, Exception? exception)> Create(JobApplicationData appData)
        {
            JobApplication app = CreateJobApplication(appData);
            if (!app.IsValid())
                return (false, new InvalidDataException());
            if (app.StatusHistories.Count == 0)
                addCreatedStatus(app);
            try
            {
                await jobAppRepository.Create(app);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex);
            }

        }

        private JobApplication CreateJobApplication(JobApplicationData appData)
        {
            JobApplication app = new JobApplication()
            {
                Title = appData.Title,
                JobField = appData.JobField,
                URL = appData.Url,
                Comment = appData.Comment,
                CreatedBy = "user1"
            };
            return app;
        }

        private void addCreatedStatus(JobApplication app)
        {
            StatusHistory status = new StatusHistory("Created");
            app.StatusHistories.Add(status);
        }

        public async Task<(bool result, Exception? exception)> Update(Guid appId, JobApplicationData appData)
        {
            try
            {
                JobApplication app = await jobAppRepository.GetById(appId);
                if (app == null)
                    throw new KeyNotFoundException();

                app.Title = appData.Title ?? app.Title;
                app.JobField = appData.JobField ?? app.JobField;
                app.URL = appData.Url ?? app.URL;
                app.Comment = appData.Url ?? app.Comment;
                bool result = await jobAppRepository.Update(app);
                return (true, null);
            }
            catch (KeyNotFoundException exception)
            {
                return (false, exception);
            }
            catch (Exception ex)
            {
                return (false, ex);
            }
        }


        public async Task<(bool result, Exception? exception)> Remove(Guid ApplicationId)
        {
            try
            {
                await jobAppRepository.Remove(ApplicationId);
                return (true, null);

            }
            catch (Exception exception)
            {
                return (false, exception);
            }
        }
        public async IAsyncEnumerable<JobApplicationView> GetAll()
        {
            List<JobApplication> lstApps = await jobAppRepository.GetAll();
            foreach (JobApplication app in lstApps)
            {
                yield return JobAppToView(app);
            }
        }

        public async Task<JobApplicationView> GetById(Guid Id)
        {
            JobApplication app = await jobAppRepository.GetById(Id);

            return JobAppToView(app);

        }

        private JobApplicationView JobAppToView(JobApplication app)
        {
            return new JobApplicationView()
            {
                ApplicationId = app.Id,
                Title = app.Title,
                Jobfield = app.JobField,
                Comment = app.Comment,
                Url = app.URL
            };
        }
    }
}
