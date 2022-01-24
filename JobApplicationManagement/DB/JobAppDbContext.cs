using JobApplicationManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationManagement.DB
{
    public class JobAppDbContext : DbContext
    {
        public JobAppDbContext(DbContextOptions<JobAppDbContext> options) : base(options)
        {

        }


        
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<StatusHistory> StatusHistories { get; set; }
        public DbSet<User> Users { get; set; }


    }
}
