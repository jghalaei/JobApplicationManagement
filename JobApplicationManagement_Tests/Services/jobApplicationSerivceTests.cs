using JobApplicationManagement.JsonData;
using JobApplicationManagement.Models;
using JobApplicationManagement.Repositories;
using JobApplicationManagement.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplicationManagement_Tests.Services
{
    [TestClass]

    public class jobApplicationSerivceTests
    {
        Mock<JobApplicationRepository> mockAppRepo;
        JobApplicationService jobAppService;

        [TestInitialize]
        public void initialize()
        {
            mockAppRepo = new();
            jobAppService = new JobApplicationService(mockAppRepo.Object);
            jobAppService = new JobApplicationService(mockAppRepo.Object);
        }
        [TestMethod]
        public async Task Create_success()
        {
            //1. createjob application
            //2. create first status history
            JobApplicationData appData = new JobApplicationData()
            {
                Title = "Job1",
                JobField = "Developer",
                Url = "http://url.com"
            };
            JobApplication app = new JobApplication()
            {
                Title = "Job1",
                JobField = "Developer",
                CreatedBy = "reza",
                URL = "http://url.com"
            };
            mockAppRepo.Setup(r => r.Create(new JobApplication())).ReturnsAsync(new Guid());
            (bool result, Exception? exception) = await jobAppService.Create(appData);
            Assert.IsTrue(result);
            Assert.IsNull(exception);
            //Assert.IsNotNull(app.StatusHistories.First());
            //Assert.AreEqual(app.StatusHistories.First().Status, "Created");
        }

        //[TestMethod]
        public async Task Create_failure_RaiseError()
        {
            JobApplication app = new()
            {
                Title = "Job1",
                JobField = "Developer",
                URL = "http://url.com"
            };
            JobApplicationData appData = new()
            {
                Title = "Job1",
                JobField = "Developer",
                Url = "http://url.com"
            };
            mockAppRepo.Setup(r => r.Create(app)).Throws(new DbUpdateException());
            (bool result, Exception? exception) = await jobAppService.Create(appData);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public async Task Update_success()
        {
           Guid guid= Guid.NewGuid(); 
            JobApplicationData appData = new JobApplicationData()
            {
                Title = "Job2",
                JobField = "Developer",
            };
            JobApplication app = new()
            {
                Id = guid
            };
            mockAppRepo.Setup(r=> r.Update(app)).ReturnsAsync(true);
            mockAppRepo.Setup(r=>r.GetById(guid)).ReturnsAsync(new JobApplication());
            (bool result, Exception? exception) = await jobAppService.Update(guid, appData);
            Assert.IsTrue(result);
            Assert.IsNull(exception);
        }

        [TestMethod]
        public async Task Update_failure_AppNotFound()
        {
            Guid guid = Guid.NewGuid();
            JobApplicationData appData = new JobApplicationData()
            {
                Title = "Job2",
                JobField = "Developer",
            };
            JobApplication app = new()
            {
                Id = guid
            };
            mockAppRepo.Setup(r => r.Update(app)).ReturnsAsync(true);
            mockAppRepo.Setup(r => r.GetById(guid)).Throws(new KeyNotFoundException());
            (bool result, Exception? exception) = await jobAppService.Update(guid, appData);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception,typeof(KeyNotFoundException));
        }

        [TestMethod]
        public async Task Update_failure_ThrowError()
        {
            Guid guid = Guid.NewGuid();
            JobApplicationData appData = new JobApplicationData()
            {
                Title = "Job2",
                JobField = "Developer",
            };
            JobApplication app = new()
            {
                Id = guid
            };
            mockAppRepo.Setup(r => r.Update(app)).ReturnsAsync(true);
            mockAppRepo.Setup(r => r.GetById(guid)).Throws(new Exception());
            (bool result, Exception? exception) = await jobAppService.Update(guid, appData);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(Exception));
        }


    }
}
