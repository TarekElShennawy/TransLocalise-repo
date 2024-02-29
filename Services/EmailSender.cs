using Humanizer;
using System.Net;
using System.Net.Mail;
using System.Text;
using Translator_Project_Management.Services.Interfaces;

namespace Translator_Project_Management.Services
{
	public class EmailSender : IEmailSender
	{
		private readonly string _senderEmail;
		private readonly string _senderPassword;
		private readonly SmtpClient _client;

        public EmailSender(string senderEmail, string senderPassword)
        {
            _senderEmail = senderEmail;
			_senderPassword = senderPassword;
			_client = new SmtpClient("smtp.office365.com", 587)
			{
				EnableSsl = true,
				Credentials = new NetworkCredential(_senderEmail, _senderPassword)
			};
        }

        public async Task SendEmail(string email, string subject, string body)
		{
			await SendEmailAsync(email, subject, body, null, null, null);
		}

		public async Task SendEmail(string email, string subject, string body, string fileContent, string fileName, string fileType)
		{
			await SendEmailAsync(email, subject, body, fileContent, fileName, fileType);
		}

		private async Task SendEmailAsync(string email, string subject, string body, string fileContent, string fileName, string fileType)
		{
			var message = new MailMessage
			{
				From = new MailAddress(_senderEmail),
				To = { new MailAddress(email) },
				Subject = subject,
				Body = body
			};

			if (fileContent != null )
			{
				var contentAttachment = new Attachment(new MemoryStream(Encoding.UTF8.GetBytes(fileContent)), $"{fileName}.{fileType}", $"application/{fileType}");
				message.Attachments.Add(contentAttachment);
			}

			await _client.SendMailAsync(message);
		}
	}
}
