// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// using System.Data.OleDb;
// using Dapper;

// namespace jvPo.dbRepo
// {
//     public class dbRepo
//     {
// private readonly string _connectionString;

//     // Use a constructor to get the config
//     public dbRepo(IConfiguration configuration)
//     {
//         _connectionString = configuration.GetConnectionString("DefCon:"); 
//     }

//         public IEnumerable<Models.DeliveryAddress> getAll()
//         {
// #pragma warning disable CA1416 // Validate platform compatibility
//             using (var connection = new OleDbConnection(_connectionString))
//             {
// #pragma warning disable CA1416 // Validate platform compatibility
//                 var command = new OleDbCommand("SELECT * FROM DeliveryAddress", connection);
// #pragma warning restore CA1416 // Validate platform compatibility
//                 return connection.Query<Models.DeliveryAddress>("SELECT * FROM DeliveryAddress");
//             }
// #pragma warning restore CA1416 // Validate platform compatibility

//         }

//     }
// }