using JobApplicationManagement.DB;
using JobApplicationManagement.Exceptions;
using JobApplicationManagement.Models;
using JobApplicationManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplicationManagement_Tests.Repositories
{
    [TestClass]
    public class StatusHistoryRepositoryTests
    {
        JobAppDbContext_Stub _context;
        StatusHistoryRepository _repository;
        JobApplication _application;
        [TestInitialize]
        public async Task Initialize()
        {
            DbContextOptions<JobAppDbContext> option = new DbContextOptionsBuilder<JobAppDbContext>()
    .UseInMemoryDatabase("JobAppDb").Options;
            this._context = new JobAppDbContext_Stub(option);
            _repository = new(_context);
            _context.Users.Add(new User() { UserName = "reza2", Password = "reza", Roles = "Administrator" });
            await _context.SaveChangesAsync();
            _application = new JobApplication()
            {
                CreatedBy="reza",
                JobField = "Developer",
                Title = "C# developper",
                URL = "http://test.com",
                Comment = "test comment"
            };
            _context.JobApplications.Add(_application);
            await _context.SaveChangesAsync();

        }
        [TestMethod]
        public async Task CreateStatusHistory_success()
        {
            StatusHistory his = new StatusHistory("Applied")
            {
                Application = _application,
                Comment = "I found it via an ad"
            };
            Guid historyId = await _repository.CreateStatusHistory(his);
            Assert.IsNotNull(historyId);
            StatusHistory newHistory = _context.StatusHistories.First();
            Assert.IsNotNull(newHistory);
            Assert.AreEqual(historyId, newHistory.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CreateStatusHistory_failure_invalidData()
        {
            StatusHistory his = new StatusHistory("")
            {
                Application = _application,
                Comment = "I found it via an ad"
            };
            Guid historyId = await _repository.CreateStatusHistory(his);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseException))]
        public async Task CreateStatusHistory_failure_DatabaseError()
        {
            StatusHistory his = new StatusHistory("RaiseError")
            {
                Application = _application,
                Comment = "I found it via an ad"
            };
            Guid historyId = await _repository.CreateStatusHistory(his);
        }

        [TestMethod]
        public async Task UpdateStatusHistory_success()
        {
            await CreateStatusHistory_success();
            StatusHistory his = _context.StatusHistories.First();
            his.Status = "Updated Status";
            bool isUpdated = await _repository.UpdateStatusHistory(his);
            Assert.IsTrue(isUpdated);
            StatusHistory his2 = _context.StatusHistories.First();
            Assert.AreEqual("Updated Status", his2.Status);
            Assert.AreEqual(his.Id, his2.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task UpdateStatusHistory_failure_invalidData()
        {
            await CreateStatusHistory_success();
            StatusHistory his = _context.StatusHistories.First();
            his.Status = "";
            bool isUpdated = await _repository.UpdateStatusHistory(his);
            Assert.IsTrue(isUpdated);


        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseException))]
        public async Task UpdateStatusHistory_failure_DatabaseError()
        {
            await CreateStatusHistory_success();
            StatusHistory his = _context.StatusHistories.First();
            his.Status = "RaiseError";
            bool isUpdated = await _repository.UpdateStatusHistory(his);
            Assert.IsTrue(isUpdated);
        }

        [TestMethod]
        public async Task RemoveStatusHistory_success()
        {
            await CreateStatusHistory_success();
            StatusHistory his = _context.StatusHistories.First();
            bool isDeleted = await _repository.DeleteStatusHistory(his);
            Assert.IsTrue(isDeleted);
            Assert.AreEqual(_context.StatusHistories.Count(), 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task RemoveStatusHistory_failure_notFound()
        {
            await CreateStatusHistory_success();
            StatusHistory his = _context.StatusHistories.First();
            his.Id = Guid.NewGuid();
            _ = await _repository.DeleteStatusHistory(his);
        }

        [TestMethod]
        public async Task GetAll_success()
        {
            await CreateStatusHistory_success();
            List<StatusHistory> lst = await _repository.GetAll();
            Assert.IsNotNull(lst);
            Assert.AreEqual(lst.Count, 1);
        }

        [TestMethod]
        public async Task GetById_success()
        {
            StatusHistory his1 = new StatusHistory("Applied")
            {
                Application = _application,
                Comment = "I found it via an ad"
            };
            _context.StatusHistories.Add(his1);
            StatusHistory his2 = new StatusHistory("Applied second")
            {
                Application = _application,
                Comment = "this is second"
            };
            _context.StatusHistories.Add(his2);
            await _context.SaveChangesAsync();

            Guid id = his2.Id;
            StatusHistory result = await _repository.GetById(id);
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetById_failure_notfound()
        {
            StatusHistory his1 = new StatusHistory("Applied")
            {
                Application = _application,
                Comment = "I found it via an ad"
            };
            _context.StatusHistories.Add(his1);
            await _context.SaveChangesAsync();
            StatusHistory his2 = new StatusHistory("Applied second")
            {
                Application = _application,
                Comment = "this is second"
            };
            Guid id = his2.Id;
            StatusHistory result = await _repository.GetById(id);
        }


    }
}
