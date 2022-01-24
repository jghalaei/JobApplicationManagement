using JobApplicationManagement.DB;
using JobApplicationManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobApplicationManagement_Tests
{
    internal class JobAppDbContext_Stub : JobAppDbContext
    {
        public JobAppDbContext_Stub(DbContextOptions<JobAppDbContext> options) : base(options)
        {
            base.Database.EnsureDeleted();
           
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cansellationToken = default)
        {
            IEnumerable<EntityEntry> pendingChanges = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State==EntityState.Modified);
            IEnumerable<JobApplication> apps = pendingChanges.Select(e => e.Entity).OfType<JobApplication>();
            if (apps.Any(a=> a.Title == "RaiseError"))
            {
                throw new Exception("Database Error");
            }
            
            IEnumerable<StatusHistory> histories = pendingChanges.Select(e => e.Entity).OfType<StatusHistory>();
            if (histories.Any(h => h.Status == "RaiseError"))
            {
                throw new Exception("DataBase Error");
            }
            return await base.SaveChangesAsync(cansellationToken); 
        }
    }
}
