using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace MissingLinkPro.Models
{
    public class Emailer
    {
        public Emailer(NetworkCredential credential)
        {
            this.Credential = credential;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddresses">An array of strings representing destination addresses.  Each string may optionally be a comma separated list.</param>
        /// <param name="ccAddresses"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isHTML"></param>
        public void SendEmail(string fromAddress, string toAddress, string[] ccAddresses, string subject, string body, bool isHTML)
        {
            MailMessage message = new MailMessage
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = isHTML,
                From = new MailAddress(fromAddress),
            };
                message.To.Add(toAddress);

            if (ccAddresses != null)
            {
                foreach (var ccAddress in ccAddresses.SelectMany(c => c.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)))
                {
                    message.CC.Add(ccAddress);
                }
            }

            var client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Timeout = 50000,
                EnableSsl = true,
                Credentials = this.Credential
            };

            client.Send(message);
        }

        public NetworkCredential Credential { get; set; }
    }
}