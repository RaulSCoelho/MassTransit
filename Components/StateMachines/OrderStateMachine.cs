using Contracts;
using MassTransit;
using MassTransit.RedisIntegration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Components.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public OrderStateMachine()
        {
            Event(() => OrderSubmitted, x => x.CorrelateById(m => m.Message.OrderId));

            InstanceState(x => x.CurrentState);

            Initially(
                When(OrderSubmitted)
                    .Then(context =>
                    {
                        context.Instance.SubmitDate = context.Data.Timestamp;
                        context.Instance.CustomerNumber = context.Data.CustomerNumber;
                        context.Instance.Updated = DateTime.UtcNow;
                    })
                    .TransitionTo(Submitted)
                );

            During(Submitted,
                Ignore(OrderSubmitted));

            DuringAny(
                When(OrderSubmitted)
                    .Then(context =>
                    {
                        context.Instance.SubmitDate ??= context.Data.Timestamp;
                        context.Instance.CustomerNumber ??= context.Data.CustomerNumber;
                    })
                );
        }

        public State Submitted { get; private set; }

        public Event<OrderSubmitted> OrderSubmitted { get; private set; }
    }

    public class OrderState : SagaStateMachineInstance, ISagaVersion
    {
        public Guid CorrelationId { get; set; }
        public int Version { get; set; }

        public string CurrentState { get; set; }

        public string CustomerNumber { get; set; }

        public DateTime? SubmitDate { get; set; }
        public DateTime? Updated { get; set; }
    }
}
