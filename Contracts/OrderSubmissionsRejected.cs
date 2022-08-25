using System;

namespace Contracts
{
    public interface OrderSubmissionsRejected
    {
        Guid OrderId { get; set; }
        DateTime TimeStamp { get; set; }
        string CustomerNumber { get; set; }
        string Reason { get; set; }
    }
}
