using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly ILogger<OrderController> _logger;
        readonly IRequestClient<SubmitOrder> _submitOrderRequestClient;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IRequestClient<CheckOrder> _checkOrderRequestClient;

        public OrderController(ILogger<OrderController> logger, IRequestClient<SubmitOrder> submitOrderRequestClient, ISendEndpointProvider sendEndpointProvider, IRequestClient<CheckOrder> checkOrderRequestClient)
        {
            _logger = logger;
            _submitOrderRequestClient = submitOrderRequestClient;
            _sendEndpointProvider = sendEndpointProvider;
            _checkOrderRequestClient = checkOrderRequestClient;
        }

        // GET api/<OrderController>
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var (status, notFound) = await _checkOrderRequestClient.GetResponse<OrderStatus, OrderNotFound>(new { OrderId = id });

            if (status.IsCompletedSuccessfully)
            {
                var response = await status;
                return Ok(response.Message);
            }
            else
            {
                var response = await notFound;
                return NotFound(response.Message);
            }

        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post(Guid id, string customerNumber)
        {
            var (accepted, rejected) = await _submitOrderRequestClient.GetResponse<OrderSubmissionsAccpeted, OrderSubmissionsRejected>(new
            {
                OrderId = id,
                TimeStamp = InVar.Timestamp,
                CustomerNumber = customerNumber
            });

            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;
                return Ok(response.Message);
            }
            else
            {
                var response = await rejected;
                return BadRequest(response.Message);
            }
        }

        // PUT api/<OrderController>
        [HttpPut]
        public async Task<IActionResult> Put(Guid id, string customerNumber)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("exchange:submit-order"));

            await endpoint.Send<SubmitOrder>(new
            {
                OrderId = id,
                TimeStamp = InVar.Timestamp,
                CustomerNumber = customerNumber
            });

            return Accepted();
        }
    }
}
