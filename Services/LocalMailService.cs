using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailTo = "admin@company.com";
        private string _mailFrom = "noreply@company.com";

        public void Send(string subject, string message)
        {
            System.Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {nameof(LocalMailService)}.");
            System.Console.WriteLine($"Subject: {subject}");
            System.Console.WriteLine($"Message: {message}");
        }
    }
}