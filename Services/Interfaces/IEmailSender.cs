namespace Translator_Project_Management.Services.Interfaces
{
	public interface IEmailSender
	{
		Task SendEmail(string email, string subject, string body);
		Task SendEmail(string email, string subject, string body, string fileContent, string fileName, string fileType);
	}
}
