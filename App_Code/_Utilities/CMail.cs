using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

public class CMail
{

    public static void EnviarCorreo(string From, string To, string Cc, string Bcc, string Subject, string Message)
	{
        MailMessage Mail = new MailMessage();

        char[] delimit = new char[] { ';' };
        if (Cc != "")
        {
            foreach (string splitTo in Cc.Split(delimit))
            {
                Mail.CC.Add(splitTo);
            }
        }

        if (Bcc != "")
        {
            foreach (string splitTo in Bcc.Split(delimit))
            {
                Mail.Bcc.Add(splitTo);
            }
        }

		
		Mail.From = new MailAddress(From);
		foreach (string email in To.Split(delimit))
		Mail.To.Add(new MailAddress(email));
		Mail.Subject = Subject;
		Mail.Body = Message;
		Mail.IsBodyHtml = true;
		Mail.Priority = MailPriority.Normal;
		SmtpClient Smtp = new SmtpClient();
		Smtp.Send(Mail);
	}
}