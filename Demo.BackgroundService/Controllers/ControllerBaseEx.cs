using Demo.BackgroundService.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Demo.BackgroundService.Controllers
{
    public class ControllerBaseEx : ControllerBase
    {
        private readonly ILogger _logger;

        public ControllerBaseEx(ILogger logger)
        {
            _logger = logger;
        }
        protected void LogInfo(LogEventMap eventInfo, string id = null, string message = null)
        {
            _logger.Information($"{nameof(LogEventMap) } = {eventInfo}, Id = {id}, Message = {message}");
        }
    }
}
