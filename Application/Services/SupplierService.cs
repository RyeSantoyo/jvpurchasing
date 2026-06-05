using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Application.Interface;
using jvPo.Models;
using jvPo.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace jvPo.Application.Services
{
    public class SupplierService : ISupplier
    {
        private readonly ApplicationDbContext _context;
        public SupplierService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<object>> GetSuppliersAsync()
        {
            var supplier = await _context.Suppliers.Select(s => new
            {
                s.Id,
                s.SupplierCode,
                s.SupplierAddress,
                s.SupplierName,
                s.CityAddress,
                s.ContactPerson,
                s.Position,
                s.TelNo,
                s.FaxNo,
                s.MobileNo,

            }).ToListAsync();

            return supplier;
        }
        int a = 0;
        public async Task<(bool Success, string Message)> AddSupplierAsync(SupplierDTO dto)
        {
            if (dto == null)
                return (false, "Supplier cannot be null.");

            var supExist = await _context.Suppliers.AnyAsync(s => s.SupplierCode == dto.SupplierCode);
            if (supExist)
                return (false, "Supplier already exists.");
            try
            {
                var newSupplier = new Suppliers
                {
                    SupplierCode = dto.SupplierCode,
                    SupplierName = dto.SupplierName,
                    SupplierAddress = dto.SupplierAddress,
                    CityAddress = dto.CityAddress,
                    ContactPerson = dto.ContactPerson,
                    Position = dto.Position,
                    TelNo = dto.TelNo,
                    FaxNo = dto.FaxNo,
                    MobileNo = dto.MobileNo,

                };

                _context.Suppliers.Add(newSupplier);
                await _context.SaveChangesAsync();
                return (true, "Supplier added successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred while adding the supplier: {ex.Message}");
            }


        }

        public async Task<(bool Success, string Message)> DeleteSupplierAsync(int id)
        {
            var delSup =  await _context.Suppliers.FindAsync(id);
            if(delSup==null)
                return (false, "Supplier not found.");

            _context.Suppliers.Remove(delSup);
            await _context.SaveChangesAsync();
            return (true, "Supplier deleted successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateSupplierAsync(Suppliers supplier)
        {
            if (supplier == null)
                return (false, "Supplier cannot be null.");

            var existingSupplier = await _context.Suppliers.FindAsync(supplier.Id);
            if (existingSupplier == null)
                return (false, "Supplier not found.");

            // Update the properties of the existing supplier
            //existingSupplier.SupplierCode = supplier.SupplierCode;
            existingSupplier.SupplierName = supplier.SupplierName;
            existingSupplier.SupplierAddress = supplier.SupplierAddress;
            existingSupplier.CityAddress = supplier.CityAddress;
            existingSupplier.ContactPerson = supplier.ContactPerson;
            existingSupplier.Position = supplier.Position;
            existingSupplier.TelNo = supplier.TelNo;
            existingSupplier.FaxNo = supplier.FaxNo;
            existingSupplier.MobileNo = supplier.MobileNo;

            _context.Suppliers.Update(existingSupplier);
            await _context.SaveChangesAsync();
            return (true, "Supplier updated successfully.");
        }
    }
}