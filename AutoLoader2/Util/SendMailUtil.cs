using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace AutoLoader2.Util
{
    class SendMailUtil
    {
        static SmtpClient client = null;
        static MailAddress from = new MailAddress("AutoLoader2@lextar.com");
        static SendMailUtil()
        {
            client = new SmtpClient(AppConfigFactory.StmpServer, AppConfigFactory.StmpPort);

        }

        public static void SendMail(string[] mailToList, string subject, string mailBody)
        {
            MailMessage message = GetMailMessage(mailToList, subject, mailBody);
            client.Send(message);
        }

        private static MailMessage GetMailMessage(string[] mailToList, string subject, string mailBody)
        {
            MailMessage message = new MailMessage();
            message.From = from;
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.Subject = subject;
            message.Body = mailBody;
            foreach (string mailTo in mailToList)
            {
                message.To.Add(mailTo);
            }

            return message;
        }
    }
}
