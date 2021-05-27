using System.Threading.Tasks;

namespace Services
{
	public interface IEmailSender
	{
		Task Send(Email mail);
		Task SendEmailAsync(string email, string subject, string htmlMessage);
	}
}
