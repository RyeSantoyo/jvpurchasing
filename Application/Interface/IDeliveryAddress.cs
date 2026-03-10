using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Models;

namespace jvPo.Application.Interface
{
    public interface IDeliveryAddress
    {
        Task<IEnumerable<object>> GetDeliveryAddressAsync();
        Task<(bool Success, string Message)> AddDeliveryAddressAsync(DeliveryAddress deliveryAddress);
    }
}