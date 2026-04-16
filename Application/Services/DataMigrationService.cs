using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using jvPo.Application.DatabaseMigration;
using jvPo.Application.Interface;
using jvPo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace jvPo.Application.Services
{
    public class DataMigrationService : IDataMigration
    {
        private readonly string _legacyConnString;
        private readonly ApplicationDbContext _context;
        public DataMigrationService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            //_legacyConnString = config.GetConnectionString("LegacyDb");
            _legacyConnString = config.GetConnectionString("LegacyDb") ?? throw new InvalidOperationException("LegacyDb connection string not found.");
        }


        public async Task<int> MigratePOAsync()
        {
            using var legacyConn = new SqlConnection(_legacyConnString);
            // var legacyRows = await legacyConn.QueryAsync<LegacyPO>(@"SELECT id, CompId, PONO, date1, suppid, 
            //             suppname, address, terms, requestedby, ronum, delto, date2, totalamount, remarks, orderby, preparedby");
            var legacyRows = await legacyConn.QueryAsync<LegacyPO>(@"SELECT * From PO");
            var companyDict = await _context.Companies.ToDictionaryAsync(c => c.CompanyCode, c => c.Id);
            
            int processedCount = 0;
            int batchSize = 500;
            foreach (var oldItem in legacyRows)
            {
                string searchPONO = oldItem.PONO.ToString("G0");
                string legacyCompId = oldItem.CompID.ToString();
                //int legacySupid = (int)oldItem.suppid;

                if (companyDict.TryGetValue(legacyCompId, out int newCompanyId))
                {
                    bool exists = await _context.POs.AnyAsync(x => x.PONumber == searchPONO);
                    if (!exists)
                    {
                        var newItem = new PO
                        {
                            PONumber = searchPONO,
                            PODate = oldItem.date1,
                            SupplierId = 1,
                            SupplierName = oldItem.suppname,
                            SupplierAddress = oldItem.address,
                            AgreedTerms = oldItem.terms,
                            TermsId = 1,
                            RequestedBy = oldItem.requestedby,
                            RONumber = (int)oldItem.ronum,
                            DeliveryAddress = oldItem.delto,
                            DeliveryAddressID = 1,
                            RODate = oldItem.date2,
                            TotalAmount = oldItem.totalamount,
                            Remarks = oldItem.remarks,
                            OrderBy = oldItem.orderby,
                            CompanyCode = newCompanyId.ToString(),

                            CompanyId = newCompanyId

                        };
                        _context.POs.Add(newItem);
                        processedCount++;

                        if (processedCount % batchSize == 0)
                        {
                            await _context.SaveChangesAsync();
                            _context.ChangeTracker.Clear();
                        }
                    }

                }
                else
                {
                    Console.WriteLine($"Company Code {legacyCompId} not found in new database. Skipping PO with PONumber {searchPONO}.");
                }
                // var parentHeader = await _context.POs.FirstOrDefaultAsync(x => x.PONumber == searchPONO);

            }

            await _context.SaveChangesAsync();
            return processedCount;
        }
    }
}
