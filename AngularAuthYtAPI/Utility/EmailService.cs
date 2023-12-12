using AngularAuthYtAPI.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace AngularAuthYtAPI.Utility
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration configuration)
        {

            _config = configuration;

        }
        public void SendEmail(EmailModel model)
        {
            var emailMessage = new MimeMessage();
            var from = _config["EmailSetting:From"];
            emailMessage.From.Add(new MailboxAddress("Greetings", from));
            emailMessage.To.Add(new MailboxAddress(model.To, model.To));
            emailMessage.Subject = model.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = model.Content };

            using var client = new SmtpClient();
            try
            {
                client.Connect(_config["EmailSetting:SmtpServer"], 465, true);
                client.Authenticate(from, _config["EmailSetting:Password"]);
                client.Send(emailMessage);
            }
            catch (Exception)
            {

                throw;
            }
            finally {
                client.Disconnect(true);
                client.Dispose(); }
           
        }
    }
}
