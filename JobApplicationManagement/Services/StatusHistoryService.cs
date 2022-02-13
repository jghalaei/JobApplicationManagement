using JobApplicationManagement.JsonData;
using JobApplicationManagement.Models;
using JobApplicationManagement.Repositories;
using JobApplicationManagement.Views;

namespace JobApplicationManagement.Services
{
    public class StatusHistoryService
    {
        StatusHistoryRepository _statusHistoryRepository;
        JobApplicationRepository _jobApplicationRepository;

        public StatusHistoryService(JobApplicationRepository jobApplicationRepository, StatusHistoryRepository statusHistoryRepositoy)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _statusHistoryRepository = statusHistoryRepositoy;

        }
        public async Task<List<StatusHistoryView>> GetByJobApplicationId(Guid ApplicationId)
        {
            List<StatusHistory> lstHistory = await (_statusHistoryRepository.GetByApplicationId(ApplicationId));
            List<StatusHistoryView> lstView = new();
            foreach (StatusHistory history in lstHistory)
            {
                lstView.Add(new StatusHistoryView()
                {
                    Id = history.Id,
                    Status = history.Status,
                    Date = history.Date,
                    Comment = history.Comment
                });
            }
            return lstView;
        }

        public async Task<(Guid? historyId, Exception? exception)> Create(Guid JobApplicationId, StatusHistoryData historyData)
        {
            try
            {
                JobApplication app=await _jobApplicationRepository.GetById(JobApplicationId);
                if (app==null)
                    return (null,new KeyNotFoundException());
                StatusHistory history = new(historyData.Status)
                {
                    Application = app,
                    JobApplicationId = JobApplicationId,
                    Date = DateTime.Now,
                    Comment = historyData.Comment
                };
                Guid historyId = await _statusHistoryRepository.CreateStatusHistory(history);
                return (historyId, null);
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }
        public async Task<(bool result, Exception? exception)> Update(Guid StatusHistoryId, StatusHistoryData historyData)
        {
            try
            {
                if (historyData == null)
                    return (false, new InvalidDataException());
                StatusHistory history = await _statusHistoryRepository.GetById(StatusHistoryId);
                history.Status = historyData.Status;
                history.Comment = historyData.Comment;
                bool result = await _statusHistoryRepository.UpdateStatusHistory(history);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex);
            }
        }
    }
}
