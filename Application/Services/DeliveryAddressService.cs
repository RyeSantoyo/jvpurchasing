using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Application.Interface;
using jvPo.Models;
using Microsoft.EntityFrameworkCore;

namespace jvPo.Application.Services
{
    public class DeliveryAddressService : IDeliveryAddress
    {

        private readonly ApplicationDbContext _context;

        public DeliveryAddressService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetDeliveryAddressAsync()
        {
            var delAdd = await _context.DeliveryAddresses.Select(x => new { x.Id, x.Address }).ToListAsync();

            return delAdd;
        }
        public async Task<(bool Success, string Message)> AddDeliveryAddressAsync(DeliveryAddress deliveryAddress)
        {
            if (deliveryAddress == null)
                return (false, "Delivery address cannot be null.");

            var delExist = await _context.DeliveryAddresses.AnyAsync(x => x.Address == deliveryAddress.Address);
            if (delExist)                
                return (false, "Delivery address already exists.");
            throw new NotImplementedException();
        }

    }
}