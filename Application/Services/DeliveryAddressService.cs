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

            try
            {
                var newDeladd = new DeliveryAddress
                {
                    Address = deliveryAddress.Address
                };

                _context.DeliveryAddresses.Add(newDeladd);
                await _context.SaveChangesAsync();
                return (true, "Delivery address added successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred while adding the delivery address: {ex.Message}");
            }

        }

        public async Task<(bool Success, string Message)> UpdateDeliveryAddressAsync(DeliveryAddress deliveryAddress)
        {
            if (deliveryAddress == null)
                return (false, "Delivery address cannot be null.");

            var delAdd = await _context.DeliveryAddresses.FindAsync(deliveryAddress.Id);
            if (delAdd == null)
                return (false, "Delivery address not found.");

            delAdd.Address = deliveryAddress.Address;

            await _context.SaveChangesAsync();
            return (true, "Delivery address updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteDeliveryAddressAsync(int id)
        {
            var delAdd = await _context.DeliveryAddresses.FindAsync(id);
            if (delAdd == null)
                return (false, "Delivery address not found.");
            _context.DeliveryAddresses.Remove(delAdd);
            await _context.SaveChangesAsync();
            return (true, "Delivery address deleted successfully.");
        }
    }
}