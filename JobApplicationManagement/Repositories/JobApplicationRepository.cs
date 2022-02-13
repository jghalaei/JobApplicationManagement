using JobApplicationManagement.DB;
using JobApplicationManagement.Exceptions;
using JobApplicationManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace JobApplicationManagement.Repositories
{
    public class JobApplicationRepository
    {
        private JobAppDbContext _context;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public JobApplicationRepository()
        {

        }
        public JobApplicationRepository(JobAppDbContext _context)
        {
            this._context = _context;
        }

        public virtual async Task<Guid> Create(JobApplication jobApplication)
        {
                if (!jobApplication.IsValid() || jobApplication.StatusHistories.Count == 0)
                    throw new InvalidDataException();
            try
            {
                _context.JobApplications.Add(jobApplication);
           
                int result = await _context.SaveChangesAsync();
                return jobApplication.Id;
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }

        }

        public virtual async Task<bool> Update(JobApplication application)
        {
            if (_context.JobApplications.FirstOrDefault(a => a.Id == application.Id) == null)
            {
                throw new ArgumentException();
            }
            if (!application.IsValid()) throw new InvalidDataException();
            try
            {
                _context.JobApplications.Update(application);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                throw new DatabaseException();
            }

        }

        public virtual async Task<List<JobApplication>> GetAll()
        {

            return await _context.JobApplications.ToListAsync();
        }

        public virtual async Task<JobApplication> GetById(Guid id)
        {
            return await _context.JobApplications.FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new KeyNotFoundException();
        }

        public virtual async Task<bool> Remove(Guid id)
        {
            try
            {
                JobApplication app = await GetById(id);
                
                _context.JobApplications.Remove(app);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
