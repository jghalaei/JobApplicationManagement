using JobApplicationManagement.JsonData;
using JobApplicationManagement.Services;
using JobApplicationManagement.Views;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JobApplicationManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusHistoriesController : ControllerBase
    {
        private StatusHistoryService _statusHistoryService;
        public StatusHistoriesController(StatusHistoryService statusHistoryService)
        {
            _statusHistoryService = statusHistoryService;
        }

        [HttpGet("{applicationId}")]
        public async Task<IActionResult> GetHistoryByApplicationId(Guid applicationId)
        {
            try
            {
                List<StatusHistoryView> lstView = await _statusHistoryService.GetByJobApplicationId(applicationId);
                if (lstView.Count == 0)
                    return StatusCode((int)HttpStatusCode.NotFound, "No History Found");
                return StatusCode((int)HttpStatusCode.OK, lstView);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "internal error");
            }
        }
        [HttpPost("{applicationId}")]
        public async Task<IActionResult> CreateStatus([FromBody] StatusHistoryData historyData, Guid applicationId)
        {
            if (!ModelState.IsValid)
              return BadRequest(ModelState);
            (Guid? id, Exception? ex) = await _statusHistoryService.Create(applicationId, historyData);
            if (id != null)
                return StatusCode((int)HttpStatusCode.OK);
            else
                return BadRequest(ex);
            
        }
    }
}

