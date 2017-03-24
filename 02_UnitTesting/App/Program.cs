using System;

namespace App
{
    public interface IEmailService
    {
        bool Send(string address, string subject, string body);
    }

    public class WorkshopService
    {
        private readonly IEmailService _emailService;

        public WorkshopService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public void GiveFeedback(bool isPositive)
        {
            if (isPositive)
            {
                if (!_emailService.Send(
                    "orientman{at}gmail-dot-com",
                    "Workshop",
                    "Awesome work! Thank You!"))
                    throw new Exception("Notification failed!");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}