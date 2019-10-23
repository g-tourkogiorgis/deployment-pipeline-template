using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public string MAIL_HOST;
        public int MAIL_PORT;
        
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        public WeatherForecastController(IOptions<MailServerConfig> mailServerConfigAccessor)
        {
            if (mailServerConfigAccessor == null) throw new ArgumentNullException(nameof(mailServerConfigAccessor));

            var config = mailServerConfigAccessor.Value;
            MAIL_HOST = config.Host;
            MAIL_PORT = config.Port;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task EmailMeTheWeather()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("API", "api@api.com"));
            message.To.Add(new MailboxAddress("John", "api@api.com"));
            message.Subject = "The weather today!";

            message.Body = new TextPart("plain")
            {
                Text = "Sunny today! Have fun!"
            };

            using var mailClient = new SmtpClient();
            await mailClient.ConnectAsync(MAIL_HOST, MAIL_PORT, SecureSocketOptions.None);
            await mailClient.SendAsync(message);
            await mailClient.DisconnectAsync(true);
        }
    }
}
