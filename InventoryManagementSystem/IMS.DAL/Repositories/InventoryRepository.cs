using Dapper;
using IMS.DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAL.Repositories
{
    public class InventoryRepository
    {
        private readonly string _connectionString;

        public InventoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Inventory>> GetAllInventoryAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            var inventory = await conn.QueryAsync<Inventory>(
                "sp_GetAllInventory",
                commandType: CommandType.StoredProcedure
            );
            return inventory.ToList();
        }
    }
}
