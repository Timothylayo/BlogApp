using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSenderController(IEmailSender emailSender) : ControllerBase
    {
        private readonly IEmailSender emailSender = emailSender;

        [HttpPost]
        public async Task<ActionResult> SendConfirmationEmail(string email, string confirmationLink)
        {
            await emailSender.SendEmailAsync(email, "Confirm Your Account",
                $"Please confirm your account by clicking < a href = '{confirmationLink}' > here </ a >.");
            return Ok("Confirmation email sent");
        }
    }
}
