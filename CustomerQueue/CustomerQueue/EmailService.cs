using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CustomerQueue
{
    public class EmailService
    {
        public async Task<bool> SendSimpleTextEmail(string body, string subject, string toAddress, System.Net.NetworkCredential fromAccount, string smtpAddress, int port)
        {
            var isSuccessful = true;

            var mail = new MailMessage();
            mail.From = new MailAddress(fromAccount.UserName);
            mail.To.Add(toAddress);
            mail.Subject = subject;
            mail.Body = body;

            SmtpClient server = new SmtpClient(smtpAddress);
            server.Port = port;
            server.Credentials = fromAccount;
            server.EnableSsl = true;

            try
            {
                await server.SendMailAsync(mail);
            }
            catch(SmtpException se)
            {
                isSuccessful = false;
            }
            finally
            {
                mail.Dispose();
                server.Dispose();
            }

            return isSuccessful;
        }
    }
}
