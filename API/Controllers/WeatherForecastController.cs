using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public int MAIL_PORT = 1025;
        public string MAIL_HOST = "mail";

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
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
