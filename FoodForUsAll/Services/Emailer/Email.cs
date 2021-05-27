using System.Net.Mail;

namespace Services
{
	public class Email
	{
		public string From { get; set; }
		public string To { get; set; }
		public string CC { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
		public MailPriority MailPriority { get; set; }
	}
}
