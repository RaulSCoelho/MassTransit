using System;

namespace Contracts
{
    public interface OrderNotFound
    {
        Guid OrderId { get; set; }
    }
}