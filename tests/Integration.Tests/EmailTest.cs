using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Services;

namespace Integration.Tests
{
    public class EmailTest
    {
        public EmailTest()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            var root = configurationBuilder.Build();
            _emailer =
                new EmailSender(
                    root.GetSection("AppSettings").GetSection("MailServer").Value,
                    root.GetSection("AppSettings").GetSection("SmtpNoReplyUsername").Value,
                    root.GetSection("AppSettings").GetSection("SmtpNoReplyPassword").Value
                );
        }

        [Test]
        public void VerifyOutgoingEmail()
        {
            //Arrange
            string emailAddress = "bfwissel@yahoo.com";

            //Arrange
            Email email = new Email
            {
                From = "NoReply@FoodForUsAll.com",
                To = emailAddress,
                Subject = "Integration Test",
                Message = "Testing the emailing from Food For Us All",
            };

            //Act
            _emailer.Send(email);
        }

        #region private

        readonly IEmailSender _emailer;

        #endregion
    }
}
