using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Services
{
	public class EmailSender : IEmailSender {

		public EmailSender(string mailServer, string smtpNoReplyUsername, string smtpNoReplyPassword)
		{
			_mailServer = mailServer;
			_replacementEmailAddress = null;
			_smtpNoReplyUsername = smtpNoReplyUsername;
			_smtpNoReplyPassword = smtpNoReplyPassword;
		}

		public EmailSender(string mailServer, string smtpNoReplyUsername, string smtpNoReplyPassword, string replacementEmailAddress)
		{
			_mailServer = mailServer;
			_replacementEmailAddress = replacementEmailAddress;
			_smtpNoReplyUsername = smtpNoReplyUsername;
			_smtpNoReplyPassword = smtpNoReplyPassword;
		}

		public async Task Send(Email mail) {
			string subject = mail.Subject;
			if (_replacementEmailAddress != null && _replacementEmailAddress != string.Empty)
				subject += " originally intended for: " + mail.To;

			MailMessage message = new MailMessage (
				mail.From,
				(_replacementEmailAddress == null || _replacementEmailAddress == string.Empty) ? mail.To : _replacementEmailAddress,
				subject,
				mail.Message
			);

			if (mail.CC != null && mail.CC != string.Empty)
				message.CC.Add(mail.CC);

			message.IsBodyHtml = true;
			SmtpClient client = new SmtpClient(_mailServer)
			{
				Port = 25,
				Credentials = new NetworkCredential(_smtpNoReplyUsername, _smtpNoReplyPassword),
				EnableSsl = false,
			};
			client.Send(message);
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			Email mail = new Email
			{
				From = _smtpNoReplyUsername,
				To = email,
				Subject = subject,
				Message = htmlMessage,
				MailPriority = MailPriority.High,
			};

			await Send(mail);
		}

		#region private

		readonly string _mailServer;
		readonly string _debugNotifications;
		readonly string _replacementEmailAddress;
		readonly string _smtpNoReplyUsername;
		readonly string _smtpNoReplyPassword;

		#endregion
	}
}
