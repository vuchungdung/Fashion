﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Fashion.Library
{
    public class MailHelper
    {
        public void SendMail(String to, String subject, String content)
        {
            String mailAddress = "khoadien.dientu.fee@gmail.com";
            String mailPassword = "0913849547";

            MailMessage msg = new MailMessage(new MailAddress(mailAddress), new MailAddress(to));
            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = content;

            var client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Credentials = new NetworkCredential(mailAddress, mailPassword);
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.Send(msg);
        }
    }
}