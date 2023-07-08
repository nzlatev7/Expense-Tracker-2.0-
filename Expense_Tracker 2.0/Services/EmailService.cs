using Expense_Tracker_2._0.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Expense_Tracker_2._0.Services
{
    public class EmailService : IEmailService
    {

        // Email configuration parameters
        private const string SmtpServer = "smtp.gmail.com";
        private const int PortNumber = 587;

        private readonly IConfiguration _configuration;
        private readonly IValidationToken _validationToken;

        public EmailService(
            IConfiguration configuration,
            IValidationToken validationToken)
        {
            _configuration = configuration;
            _validationToken = validationToken;
        }

        public async Task SendConfirmationEmailAsync(string recipientEmail, int userId)
        {
            try
            {
                string senderEmail = _configuration["Email:Address"];
                string senderPassword = _configuration["Email:Password"];

                // Create a MailMessage object
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(new MailAddress(recipientEmail));
                mail.Subject = "Verify your Email Address!";

                //generate the token - 4 digits
                var token = _validationToken.GenerateEmailToken(userId);
                mail.Body = $"This is the token: {token.Token}"; //??? token

                // Set up the SmtpClient
                SmtpClient smtpClient = new SmtpClient(SmtpServer, PortNumber);
                smtpClient.EnableSsl = true; // Enable SSL if required by the SMTP server
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                // Send the email
                await smtpClient.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while sending the email: " + ex.Message);
            }
        }
    }
}
