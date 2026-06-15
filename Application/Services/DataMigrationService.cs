// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Dapper;
// using jvPo.Application.DatabaseMigration;
// using jvPo.Application.Interface;
// using jvPo.Models;
// using Microsoft.Data.SqlClient;
// using Microsoft.EntityFrameworkCore;

// namespace jvPo.Application.Services
// {
//     public class DataMigrationService : IDataMigration
//     {
//         private readonly string _legacyConnString;
//         private readonly ApplicationDbContext _context;
//         public DataMigrationService(ApplicationDbContext context, IConfiguration config)
//         {
//             _context = context;
//             //_legacyConnString = config.GetConnectionString("LegacyDb");
//             _legacyConnString = config.GetConnectionString("LegacyDb") ?? throw new InvalidOperationException("LegacyDb connection string not found.");
//         }


//         public async Task<int> MigratePOAsync()
//         {
//             using var legacyConn = new SqlConnection(_legacyConnString);
//             // var legacyRows = await legacyConn.QueryAsync<LegacyPO>(@"SELECT id, CompId, PONO, date1, suppid, 
//             //             suppname, address, terms, requestedby, ronum, delto, date2, totalamount, remarks, orderby, preparedby");
//             var legacyRows = await legacyConn.QueryAsync<LegacyPO>(@"SELECT * From PO");
//             var companyDict = await _context.Companies.ToDictionaryAsync(c => c.CompanyCode.Trim(), c => c.Id);
//             var suppDict = await _context.Suppliers.ToDictionaryAsync(s => s.SupplierName.Trim().ToLower(), s => s.Id);
//             var termsDict = await _context.Terms.ToDictionaryAsync(t => t.Term.Trim().ToLower(), t => t.Id);
//             var addressDict = await _context.DeliveryAddresses.ToDictionaryAsync(t => t.Address.Trim().ToLower(), t => t.Id);

//             int processedCount = 0;
//             int batchSize = 500;
//             foreach (var oldItem in legacyRows)
//             {
//                 string searchPONO = oldItem.PONO.ToString("G0");
//                 string legacyCompId = oldItem.CompID.ToString();
//                 string legacySuppName = oldItem.suppname?.Trim().ToLower() ?? "";
//                 string legacyTerms = oldItem.terms?.Trim().ToLower() ?? "";
//                 string legacyAddress = oldItem.delto?.Trim().ToLower() ?? "";
//                 //int legacySupid = (int)oldItem.suppid;

//                 if (companyDict.TryGetValue(legacyCompId, out int newCompanyId) && suppDict.TryGetValue(legacySuppName, out int newSuppId))
//                 {
//                     if (addressDict.TryGetValue(legacyAddress, out int newAddressId))
//                     {
//                         bool exists = await _context.POs.AnyAsync(x => x.PONumber == searchPONO);
//                         if (!exists)
//                         {
//                             termsDict.TryGetValue(legacyTerms, out int newTermsId);
//                             var newItem = new PO
//                             {
//                                 PONumber = searchPONO,
//                                 PODate = oldItem.date1,
//                                 SupplierId = newSuppId,
//                                 SupplierName = oldItem?.suppname,
//                                 SupplierAddress = oldItem.address,
//                                 AgreedTerms = oldItem.terms,
//                                 TermsId = newTermsId != 0 ? newTermsId : 1,
//                                 RequestedBy = oldItem.requestedby,
//                                 RONumber = (int)oldItem.ronum,
//                                 DeliveryAddress = oldItem.delto,
//                                 DeliveryAddressID = newAddressId,
//                                 RODate = oldItem.date2,
//                                 TotalAmount = oldItem.totalamount,
//                                 Remarks = oldItem.remarks,
//                                 OrderBy = oldItem.orderby,
//                                 CompanyCode = newCompanyId.ToString(),

//                                 CompanyId = newCompanyId

//                             };
//                             _context.POs.Add(newItem);
//                             processedCount++;

//                             if (processedCount % batchSize == 0)
//                             {
//                                 await _context.SaveChangesAsync();
//                                 _context.ChangeTracker.Clear();
//                             }
//                         }
//                     }

//                 }
//                 else
//                 {
//                     Console.WriteLine($"Company Code {legacyCompId} not found in new database. Skipping PO with PONumber {searchPONO}.");
//                 }
//                 // var parentHeader = await _context.POs.FirstOrDefaultAsync(x => x.PONumber == searchPONO);

//             }

//             await _context.SaveChangesAsync();
//             return processedCount;
//         }

//         public async Task<int> MigratePODetailsAsync()
//         {
//             using var legacyConn = new SqlConnection(_legacyConnString);

//             var legacyData = await legacyConn.QueryAsync<LegacyPODetails>(@"SELECT * FROM PODetails");
//             var companyDict = await _context.Companies.ToDictionaryAsync(c => c.CompanyCode.Trim(), c => c.Id);
//             var poDict = await _context.POs.ToDictionaryAsync(p=> p.PONumber.Trim(), p=> p.Id);

//             int processedCount = 0;
//             int batchSize = 500;

//             foreach(var oldPoDetails in legacyData)
//             {
//                 string searchPONO = oldPoDetails.PONO.ToString("G0");
//                 if(poDict.TryGetValue(searchPONO, out int newPoId))
//                 {
//                     if(companyDict.TryGetValue(legacyConn.QueryFirstOrDefault<string>("SELECT CompID FROM PO WHERE PONO = @PONO", new { PONO = oldPoDetails.PONO }).Trim(), out int newCompanyId))
//                     {
//                         var newPoDetails = new PODetails
//                         {
//                             POId = newPoId,
//                             PONumber = searchPONO,
//                             CompanyId = newCompanyId,
//                             CompanyCode = newCompanyId.ToString(),
//                             Quantity = oldPoDetails.qty,
//                             Unit = oldPoDetails.unit,
//                             Description = oldPoDetails.xdesc,
//                             Price = oldPoDetails.price,
//                             Total = oldPoDetails.total
//                         };
//                         _context.PODetails.Add(newPoDetails);
//                         processedCount++;

//                         if (processedCount % batchSize == 0)
//                         {
//                             await _context.SaveChangesAsync();
//                             _context.ChangeTracker.Clear();
//                         }
//                     }
//                     else
//                     {
//                         Console.WriteLine($"Company ID for PO with PONumber {searchPONO} not found. Skipping PODetails.");
//                     }
//                 }
//                 else
//                 {
//                     Console.WriteLine($"Parent PO with PONumber {searchPONO} not found for PODetails. Skipping.");
//                 }
//             }

//             await _context.SaveChangesAsync();
//             return processedCount;
//         }
//     }
// }
