using Microsoft.Extensions.Logging;
using MQMiddleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReceiveA.Service
{
    public class CustomMessageHandler : IMessageHandler
    {
        readonly ILogger<CustomMessageHandler> _logger;
        public CustomMessageHandler(ILogger<CustomMessageHandler> logger)
        {
            _logger = logger;
        }

        public void Handle(string message, string routingKey)
        {
            // Do whatever you want!
            Console.WriteLine($"信息为{message}routekey为{ routingKey}");
            _logger.LogInformation("Ho-ho-hoooo");
        }
    }
}
