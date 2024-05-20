using SendGrid.Helpers.Mail;
using SendGrid;

namespace RetroCassetteVHS.Services
{
    public class EmailSender
    {
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
    }
}
