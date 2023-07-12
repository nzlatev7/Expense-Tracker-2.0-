using Expense_Tracker_2._0.Models.Request;
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

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(EmailSendAsyncRequest request)
        {
            try
            {
                string senderEmail = _configuration["Email:Address"];
                string senderPassword = _configuration["Email:Password"];

                // Create a MailMessage object
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(new MailAddress(request.RecipientEmail));
                mail.Subject = request.Subject;
                mail.Body = request.Body;

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
