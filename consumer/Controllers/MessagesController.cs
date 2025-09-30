using Consumer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Consumer.Controllers
{
    [ApiController]
    [Route("messages")]
    public class MessagesController : ControllerBase
    {
        private readonly RabbitMqConsumerService _consumer;

        public MessagesController(RabbitMqConsumerService consumer)
        {
            _consumer = consumer;
        }

        [HttpGet]
        public IActionResult GetMessages()
        {
            var msgs = _consumer.GetMessages();
            return Ok(msgs);
        }
    }
}
