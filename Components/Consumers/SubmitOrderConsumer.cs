using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Components.Consumers
{
    public class SubmitOrderConsumer : IConsumer<SubmitOrder>
    {
        readonly ILogger<SubmitOrderConsumer> _logger;

        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            if (context.RequestId != null)
            {
#pragma warning disable MCA0001 // Anonymous type does not map to message contract

                if (context.Message.CustomerNumber.Contains("TEST"))
                {
                    await context.RespondAsync<OrderSubmissionsRejected>(new
                    {
                        InVar.Timestamp,
                        context.Message.OrderId,
                        context.Message.CustomerNumber,
                        Reason = $"Test Consumer cannot submit orders: {context.Message.CustomerNumber}"
                    });
                }

                await context.RespondAsync<OrderSubmissionsAccpeted>(new
                {
                    InVar.Timestamp,
                    context.Message.OrderId,
                    context.Message.CustomerNumber
                });
#pragma warning restore MCA0001 // Anonymous type does not map to message contract
            }
        }
    }
}
