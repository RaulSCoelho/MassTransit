using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "MCA0001:Anonymous type does not map to message contract", Justification = "<Pendente>")]
        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            if (context.RequestId != null)
            {
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
            }

            await context.Publish<OrderSubmitted>(new
            {
                OrderId = context.Message.OrderId,
                InVar.Timestamp,
                CustomerNumber = context.Message.CustomerNumber
            });
        }
    }
}
