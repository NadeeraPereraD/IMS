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

        public async Task<List<InventoryDetail>> GetInventoryDetailAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            var inventory = await conn.QueryAsync<InventoryDetail>(
                "sp_GetInventoryDetail",
                commandType: CommandType.StoredProcedure
            );
            return inventory.ToList();
        }

        public async Task<Inventory> GetInventoryByProductNameAsync(string productName)
        {
            using var conn = new SqlConnection(_connectionString);

            // Get the first matching inventory (highest selling price)
            var inventory = await conn.QueryFirstOrDefaultAsync<Inventory>(
                "sp_GetInventoryByProductName",
                new { ProductName = productName },
                commandType: CommandType.StoredProcedure
            );
            return inventory;
        }

        public async Task<List<Inventory>> GetAllInventoryVariationsByProductNameAsync(string productName)
        {
            using var conn = new SqlConnection(_connectionString);

            // Get all price variations for a product
            var inventoryList = await conn.QueryAsync<Inventory>(
                "sp_GetInventoryByProductName",
                new { ProductName = productName },
                commandType: CommandType.StoredProcedure
            );
            return inventoryList.ToList();
        }

        public async Task<List<Inventory>> GetAvailableInventoryAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            var inventory = await conn.QueryAsync<Inventory>(
                "sp_GetAvailableInventory",
                commandType: CommandType.StoredProcedure
            );
            return inventory.ToList();
        }
    }
}
