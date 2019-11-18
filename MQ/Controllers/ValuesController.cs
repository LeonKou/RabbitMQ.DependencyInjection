using Microsoft.AspNetCore.Mvc;
using MQMiddleware;
using System.Collections.Generic;

namespace MQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IQueueService _queueService;
        public ValuesController(IQueueService queueService)
        {
            _queueService = queueService;
        }
        // GET api/values
        [HttpGet]
        [Route("Send")]
        public ActionResult<IEnumerable<string>> Send(string mes)
        {
            _queueService.Send(
                                @object: $"{mes}",
                                exchangeName: "exchange1",
                                routingKey: "mini"
                                 );
            return new string[] { "value1", "value2" };
        }

        
    }
}
