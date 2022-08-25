using System;

namespace Contracts
{
    public interface OrderSubmissionsAccpeted
    {
        Guid OrderId { get; set; }
        DateTime TimeStamp { get; set; }
        string CustomerNumber { get; set; }
    }
}
