using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SisOdonto.Domain.Interfaces.Services;
using SisOdonto.Infrastructure.CrossCutting.Services.Models;
using System;

namespace SisOdonto.Infrastructure.CrossCutting.Services
{
    public class EmailService : IEmailService
    {
        #region Fields

        private readonly EmailSettings _emailSettings;

        #endregion Fields

        #region Constructors

        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        #endregion Constructors

        #region Methods

        public string BuildHtmlEmailBody(string shortTitle, string bodyMessage)
        {
            var emailHtml = $"<html><head><title>{shortTitle}</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"> " +
                            $"<link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\"> " +
                            $"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"> " +
                            $"<meta name=\"viewport\" content=\"width=device-width\"> " +
                            $"<style type=\"text/css\"> @-ms-viewport {{ width: device-width; }} @media screen and (max-width: 600px) {{ #total {{ background: #fff !important; }} }} </style> " +
                            $"</head> " +
                            $"<body leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\"> " +
                            $"<table id=\"total\" style=\"max-height:100%; padding: 80px 0; background: #f1f1f1;\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"100%\" width=\"100%\"> " +
                            $"<tr><td align=\"center\" valign=\"top\"><table bgcolor=\"#ffffff\" align=\"center\" width=\"631\" height=\"auto\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"> " +
                            $"<tr> <td style=\"border-bottom: 1px solid; background: #fff;\" colspan=\"2\"></td> " +
                            $"<td style=\"border-bottom: 1px solid; background: #fff;\" width=\"406\" height=\"76\" alt=\"\" colspan=\"2\"> " +
                            $"<h2 style=\"text-align: right; font-family: 'open sans'; margin-right: 32px; font-weight: 100; margin-bottom: -9px; color: ##4b545c;\">  " +
                            $"{shortTitle}  " +
                            $"</h2> " +
                            $"</td></tr><tr><td width=\"631\" height=\"37\" alt=\"\" colspan=\"4\"></td> " +
                            $"</tr><tr><td bgcolor=\"#fff\" width=\"33\" height=\"auto\" alt=\"\"></td> " +
                            $"<td bgcolor=\"#fff\" width=\"564\" height=\"auto\" alt=\"\" colspan=\"2\" style=\"padding-bottom: 10px;\"> " +
                            $"{ bodyMessage } " +
                            $"</td><td bgcolor=\"#fff\" width=\"34\" height=\"auto\" alt=\"\"></td></tr><tr> " +
                            $"<td bgcolor=\"##4b545c\" width=\"34\" height=\"50\" alt=\"\"></td> " +
                            $"<td bgcolor=\"##4b545c\" width=\"563\" height=\"50\" alt=\"\" colspan=\"2\"> " +
                            $"<p style=\" width: 80%; display: block; margin: 0 auto;  text-align: center; font-family: 'open sans'; font-weight: 100; color: #677e89; font-size: 11px; color:white;\"> " +
                            $"&copy; Copyright - SisOdonto - 2021 </p> " +
                            $"</td> <td bgcolor=\"##4b545c\" width=\"34\" height=\"50\" alt=\"\"></td></tr><tr> " +
                            $"<td width=\"33\" height=\"0\" alt=\"\" bgcolor=\"#f1f1f1\"></td> " +
                            $"<td width=\"192\" height=\"0\" alt=\"\" bgcolor=\"#f1f1f1\"></td> " +
                            $"<td width=\"372\" height=\"0\" alt=\"\" bgcolor=\"#f1f1f1\"></td> " +
                            $"<td width=\"34\" height=\"0\" alt=\"\" bgcolor=\"#f1f1f1\"></td></tr> " +
                            $"</table></td></tr></table></body></html>";

            return emailHtml;
        }

        public void SendEmail(string to, string subject, string shortTitle, string message)
        {
            var htmlContent = BuildHtmlEmailBody(shortTitle, message);

            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_emailSettings.Email));

            email.To.Add(MailboxAddress.Parse(to));

            email.Subject = subject;

            email.Body = new TextPart(TextFormat.Html) { Text = htmlContent };

            try
            {
                using var smtp = new SmtpClient();
                smtp.Connect(_emailSettings.Host, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion Methods
    }
}