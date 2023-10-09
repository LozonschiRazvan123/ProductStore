using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Core.DTO;
using ProductStore.Core.Interface;
using WorkerService.Interface;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IBackgroundTaskQueue _taskQueue;

        public EmailController(IEmailService emailService, IBackgroundTaskQueue taskQueue)
        {
            _emailService = emailService;
            _taskQueue = taskQueue;
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] EmailDTO request)
        {
            await _taskQueue.EnqueueAsync(async cancellationToken =>
            {
                     _emailService.SendEmail(request);
            });

            return Ok("Email was successfully sent!");
            /*_emailService.SendEmail(request);
            return Ok();*/
        }


    }
}
