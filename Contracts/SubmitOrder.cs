using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface SubmitOrder
    {
        Guid OrderId { get; set; }
        DateTime TimeStamp { get; set; }
        string CustomerNumber { get; set; }
    }
}
