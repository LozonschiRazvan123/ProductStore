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

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("SendEmail")]
        public IActionResult SendEmail(EmailDTO request)
        {
            _taskQueue.EnqueueAsync(async cancellationToken =>
            {
                     _emailService.SendEmail(request);
            });

            return Ok("Email was successfully sent!");
            /*_emailService.SendEmail(request);
            return Ok();*/
        }


    }
}
