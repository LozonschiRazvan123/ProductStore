using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Core.DTO;
using ProductStore.Core.Interface;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] EmailDTO request)
        {

                     _emailService.SendEmail(request);
            return Ok("Email was successfully sent!");
            /*_emailService.SendEmail(request);
            return Ok();*/
        }


    }
}
