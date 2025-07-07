using brevo_csharp.Api;
using brevo_csharp.Client;
using brevo_csharp.Model;
using System.Reflection;

namespace EBest.Services
{
    public class MailSender
    {
        public static void SendMail(string SenderName , string SenderEmail,string SendToMail,string SentToName,string Content) 
        {
            var api = new TransactionalEmailsApi();

            var sender = new SendSmtpEmailSender(SenderName, SenderEmail);
            var recipient = new SendSmtpEmailTo(SendToMail, SentToName);
            var recipients = new List<SendSmtpEmailTo> { recipient };
            var email = new SendSmtpEmail(
           sender: sender,
           to: recipients,
           subject: "Reset password",
           textContent: $"your reset link : {Content}"
       );

           
            try
            {
                var response = api.SendTransacEmail(email);
                Console.WriteLine($"Email sent! Message ID: {response.MessageId}");
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                Console.WriteLine(ex.ErrorContent);
            }
        }
    }
 }

