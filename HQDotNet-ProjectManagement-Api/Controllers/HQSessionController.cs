using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HQ.Model;

namespace HQ.API.Controller {
    [ApiController]
    //[Route("[controller]")]
    [Route("/v1/beginsession")]
    public class HQSessionController : ControllerBase {

        private readonly ILogger<HQSessionController> _logger;

        public HQSessionController(ILogger<HQSessionController> logger) {
            _logger = logger;
        }

        [HttpGet(Name = "BeginNewSession")]
        public HQSessionConfig Get() {

            var session = new HQSessionConfig() {
                ServerVersion = "v1",
                SessionStartDate = DateTime.Now,
                DevelopmentSession = true
            };
            return session;

            /*var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();*/
        }
    }
}
