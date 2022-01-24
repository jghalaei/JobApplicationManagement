using JobApplicationManagement.DB;
using JobApplicationManagement.Exceptions;
using JobApplicationManagement.Models;
using JobApplicationManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplicationManagement_Tests.Repositories
{
    [TestClass]
    public class jobApplicationRepositoryTests
    {
        JobAppDbContext_Stub _context;
        JobApplicationRepository _repository;
        [TestInitialize]
        public async Task Initialize()
        {
            DbContextOptions<JobAppDbContext> option = new DbContextOptionsBuilder<JobAppDbContext>()
                .UseInMemoryDatabase("JobAppDb").Options;
            this._context = new JobAppDbContext_Stub(option);
            _context.Users.Add(new User() { UserName = "reza2", Password = "reza", Roles = "Administrator" });
            await _context.SaveChangesAsync();
            _repository = new JobApplicationRepository(_context);
            Assert.IsNotNull(await _context.Users.FirstAsync());
            Assert.IsNotNull(_repository);
        }
        [TestMethod]
        public async Task CreateJobApplication_success()
        {

            JobApplication app = new JobApplication()
            {
                CreatedBy = "reza",
                JobField = "Developer",
                Title = "C# developper",
                URL = "http://test.com",
                Comment = "test comment",
                StatusHistories = new List<StatusHistory>()
                {
                    new StatusHistory("Tested"),
                    new StatusHistory("DoubleTest")
                }
            };
            Guid AppId = await _repository.Create(app);
            JobApplication? app2 = _context.JobApplications.Find(AppId);

            Assert.IsNotNull(AppId);
            Assert.IsNotNull(app2);
            Assert.AreEqual(2, app.StatusHistories.Count);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task CreateJobApplication_failure_StatusHistoriesNotProvided()
        {
            JobApplication app = new JobApplication()
            {
                CreatedBy = "reza",
                JobField = "Developer",
                Title = "C# developper",
                URL = "http://test.com",
                Comment = "test comment"
            };
            _ = await _repository.Create(app);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task CreateJobApplication_failure_InValidTitle()
        {
            JobApplication app = new JobApplication()
            {
                CreatedBy = "reza",
                JobField = "Developer",
                URL = "http://test.com",
                Comment = "test comment"
            };
            _ = await _repository.Create(app);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseException))]
        public async Task CreateJobApplication_failure_DatabaseError()
        {
            JobApplication app = new JobApplication()
            {
                CreatedBy = "reza",
                JobField = "Developer",
                Title = "RaiseError",
                URL = "http://test.com",
                Comment = "test comment",
                StatusHistories = new List<StatusHistory>()
                {
                    new StatusHistory("Tested")
                }
            };
            Guid AppId = await _repository.Create(app);

            Assert.IsNotNull(AppId);
        }

        [TestMethod]
        public async Task UpdateJobApplication_success()
        {
            await CreateJobApplication_success();
            JobApplication app = _context.JobApplications.First();
            app.Title = "Updated Title";
            bool result = await _repository.Update(app);
            Assert.IsTrue(result);

            JobApplication? app2 = _context.JobApplications.FirstOrDefault(a => a.Title == "Updated Title");
            Assert.IsNotNull(app2);
            Assert.AreEqual(app2.Title, "Updated Title");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task UpdateHobApplication_failure_AppNotFound()
        {
            await CreateJobApplication_success();
            JobApplication app = _context.JobApplications.First();
            app.Id = new Guid();
            app.Title = "Updated Title";
            bool result = await _repository.Update(app);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task UpdateApplication_failure_InvalidData()
        {
            await CreateJobApplication_success();
            JobApplication app = _context.JobApplications.First();
            app.Title = null;
            bool result = await _repository.Update(app);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseException))]
        public async Task UpdateApplication_failure_DatabaseError()
        {
            await CreateJobApplication_success();
            JobApplication app = _context.JobApplications.First();
            app.Title = "RaiseError";
            bool result = await _repository.Update(app);
        }

        [TestMethod]
        public async Task GetAll_success()
        {
            await CreateJobApplication_success();
            List<JobApplication> apps = await _repository.GetAll();
            Assert.AreEqual(1, apps.Count());

        }

        [TestMethod]
        public async Task GetjobApplicationById_success()
        {
            await CreateJobApplication_success();
            JobApplication app = _context.JobApplications.First();
            Guid id = app.Id;
            JobApplication app2 = await _repository.GetById(id);
            Assert.IsNotNull(app2);
            Assert.AreEqual(app2.Id, id);
            Assert.AreEqual(app2.Title, app.Title);

        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetjobApplicationById_failure_NotFound()
        {
            Guid id = new Guid();
            JobApplication app = await _repository.GetById(id);
        }

        [TestMethod]
        public async Task DeleteJobApplication_success()
        {
            await CreateJobApplication_success();
            JobApplication app = _context.JobApplications.First();
            bool isDeleted = await _repository.Remove(app.Id);
            Assert.IsTrue(isDeleted);
            Assert.IsNull(_context.JobApplications.FirstOrDefault(a => a.Id == app.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task RemoveJobApplication_failure_Notfound()
        {
            await CreateJobApplication_success();
            Guid id = new();
            bool isDeleted = await _repository.Remove(id);
        }
    }
}
