using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace RetroCassetteVHS.Services
{
    public class EmailSender
    {
        private readonly UserManager<IdentityUser> _userManager;

        public EmailSender(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task SendEmail(string subject, string toEmail, string userName, string message)
        {
            var apiKey = "***REMOVED***";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("retrocassette@onet.pl", "Retro Cassette");
            var to = new EmailAddress(toEmail, "Example User");
            var plainTextContent = message;
            var htmlContent = plainTextContent;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        public async Task SendConfirmationEmail(string userId, string callbackUrl)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var email = user.Email;
            var subject = "Confirm your email - Retro Cassette";
            var body = $@"
                <h2>Welcome to Retro Cassette!</h2>
                <p>Hi {user.Email},</p>
                <p>Thank you for registering an account with Retro Cassette. Please confirm your account by clicking the link below:</p>
                <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='display: inline-block; padding: 10px 20px; font-size: 16px; color: #fff; background-color: #007bff; border-radius: 5px; text-decoration: none;'>Confirm your email</a>
                <p>If you did not register an account, please ignore this email.</p>
                <p>Best regards,<br/>The Retro Cassette Team</p>
            ";

            await SendEmail(subject, email, user.UserName, body);
        }
    }
}
