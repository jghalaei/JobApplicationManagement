using JobApplicationManagement.DB;
using JobApplicationManagement.Exceptions;
using JobApplicationManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationManagement.Repositories
{
    public class StatusHistoryRepository
    {
         JobAppDbContext _context;

        public StatusHistoryRepository(JobAppDbContext _context)
        {
            this._context = _context;
        }

        public async Task<Guid> CreateStatusHistory(StatusHistory history)
        {
            
            if (history == null || !history.IsValid())
                throw new ArgumentException();
            try
            {

                _context.StatusHistories.Add(history);
                await _context.SaveChangesAsync();
                return history.Id;
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

        public async Task<bool> UpdateStatusHistory(StatusHistory history)
        {
            if (history == null || !history.IsValid())
                throw new ArgumentException();
            try
            {
                _context.StatusHistories.Update(history);
                _= await _context.SaveChangesAsync();
                return true;
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

        public async Task<bool> DeleteStatusHistory(StatusHistory history)
        {
            StatusHistory his = _context.StatusHistories.FirstOrDefault(h => h.Id == history.Id)
                ?? throw new ArgumentException();
            _context.StatusHistories.Remove(his);
            _ = await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<StatusHistory>> GetAll()
        {
            return await _context.StatusHistories.ToListAsync();
        }

        public async Task<StatusHistory> GetById(Guid id)
        {
            return await _context.StatusHistories.FindAsync(id) 
                ?? throw new KeyNotFoundException();
        }
    }
}
