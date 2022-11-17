using Contracts;
using MassTransit;

namespace Components.StateMachines
{
    public class OrderStateMachineDefinition : SagaDefinition<OrderState>
    {
        public OrderStateMachineDefinition()
        {
            ConcurrentMessageLimit = 4;
        }

        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OrderState> sagaConfigurator)
        {
            //var partition = endpointConfigurator.CreatePartitioner(8);

            //sagaConfigurator.Message<OrderSubmitted>(m => m.UsePartitioner(partition, m => m.Message.CustomerNumber));

            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 5000, 10000));
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
