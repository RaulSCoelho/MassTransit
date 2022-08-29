using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface OrderSubmitted
    {
        Guid OrderId { get; set; }
        DateTime Timestamp { get; set; }
        string CustomerNumber { get; set; }
    }
}
