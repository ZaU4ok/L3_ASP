using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace lr3
{
    public class CalcService
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Subtract(int a, int b)
        {
            return a - b;
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class CalcController : ControllerBase
    {
        private readonly CalcService _calcService;

        public CalcController(CalcService calcService)
        {
            _calcService = calcService;
        }

        [HttpGet("add")]
        public IActionResult Add(int a, int b)
        {
            var result = _calcService.Add(a, b);
            return Ok(result);
        }

        [HttpGet("sub")]
        public IActionResult Subtract(int a, int b)
        {
            var result = _calcService.Subtract(a, b);
            return Ok(result);
        }
    }

    public interface ITimeOfDayService
    {
        string GetTimeOfDay();
    }

    public class TimeOfDayService : ITimeOfDayService
    {
        public string GetTimeOfDay()
        {
            var now = DateTime.Now;
            if (now.Hour >= 6 && now.Hour < 12)
                return "зараз ранок";
            else if (now.Hour >= 12 && now.Hour < 18)
                return "зараз день";
            else if (now.Hour >= 18 && now.Hour < 24)
                return "зараз вечір";
            else
                return "зараз ніч";
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class TimeOfDayController : ControllerBase
    {
        private readonly ITimeOfDayService _timeOfDayService;

        public TimeOfDayController(ITimeOfDayService timeOfDayService)
        {
            _timeOfDayService = timeOfDayService;
        }

        [HttpGet]
        public IActionResult GetTimeOfDay()
        {
            var timeOfDay = _timeOfDayService.GetTimeOfDay();
            return Ok(timeOfDay);
        }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<CalcService>();
            services.AddTransient<ITimeOfDayService, TimeOfDayService>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
