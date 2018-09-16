using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Testbot
{
    public static class EmailServise
    {
        public static void SendMessage(string title, string text)
        {
            string pass = "20012653200";

             SmtpClient Smtp = new SmtpClient("smtp.gmail.com", 587);
            Smtp.Credentials = new NetworkCredential("moskovets1965@gmail.com", pass);
            Smtp.EnableSsl = true;
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress("moskovets1965@gmail.com");
            Message.To.Add(new MailAddress("kvark-1@ukr.net"));
           // Message.To.Add(new MailAddress("moskovets1965@gmail.com "));
            Message.Subject = title;
            Message.Body = text;
            Smtp.Send(Message);  
        }
    }
}
