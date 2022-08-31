using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface OrderStatus
    {
        Guid OrderId { get; set; }
        string State { get; set; }
    }
}
