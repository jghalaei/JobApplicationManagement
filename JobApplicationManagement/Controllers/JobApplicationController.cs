
using JobApplicationManagement.JsonData;
using JobApplicationManagement.Services;
using JobApplicationManagement.Views;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JobApplicationManagement.Controllers
{
    [Route("JobApplication")]
    public class JobApplicationController :Controller
    {
        private readonly JobApplicationService _appService;
        public JobApplicationController(JobApplicationService appService)
        {
            this._appService = appService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateJobApplication([FromBody] JobApplicationData AppData)
        {
            if (ModelState.IsValid)
            {
                (bool result, Exception? exception) = await  _appService.Create(AppData);
                if (result)
                    return StatusCode((int)HttpStatusCode.OK);

                return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);

            }
            return BadRequest(ModelState);
            

        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobApplications()
        {
            try
            {
                List<JobApplicationView> apps = new List<JobApplicationView>();
                await foreach (JobApplicationView job in _appService.GetAll())
                {
                    apps.Add(job);
                }
                if (apps.Count == 0)
                    return StatusCode((int)HttpStatusCode.NotFound, "No Job Application Found");
            return StatusCode((int)HttpStatusCode.OK,apps);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "internal error");
            }
            
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            try
            {

                JobApplicationView view = await _appService.GetById(Id);
            return StatusCode((int)HttpStatusCode.OK,view);
            }
            catch (KeyNotFoundException)
            {
                return BadRequest("Application Not found");
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Something Went Wrong");
            }
        }

        [HttpPatch("{Id}")]
        public async Task<IActionResult> UpdateJobApplication([FromBody] JobApplicationData appData, Guid Id)
        {
            (bool result, Exception? ex) = await _appService.Update(Id, appData);

            if (result)
                return StatusCode((int)HttpStatusCode.OK, "Application Updated Successfully");
            else if (ex != null && ex is KeyNotFoundException)
                return BadRequest("Application Not found");
            else
                return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong");
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteJobApplication(Guid Id)
        {
            (bool result, Exception? ex)= await _appService.Remove(Id);
            if (result)
                return StatusCode((int)HttpStatusCode.OK, "Application Deleted Successfully");
            else if (ex != null && ex is KeyNotFoundException)
            {
                return StatusCode((int)HttpStatusCode.NotFound, "Application not found");

            }
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                
            
        }
    }
}
