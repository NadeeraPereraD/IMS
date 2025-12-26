using IMS.DAL.Models;
using IMS.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.BAL.Services
{
    public class InventoryService
    {
        private readonly InventoryRepository _repository;

        public InventoryService(string connectionString)
        {
            _repository = new InventoryRepository(connectionString);
        }

        public async Task<List<Inventory>> GetAllInventoryAsync()
        {
            return await _repository.GetAllInventoryAsync();
        }

        public async Task<List<InventoryDetail>> GetInventoryDetailAsync()
        {
            return await _repository.GetInventoryDetailAsync();
        }

        public async Task<List<Inventory>> GetAvailableInventoryAsync()
        {
            return await _repository.GetAvailableInventoryAsync();
        }

        public async Task<Inventory> GetInventoryByProductNameAsync(string productName)
        {
            return await _repository.GetInventoryByProductNameAsync(productName);
        }

        public async Task<List<Inventory>> GetAllInventoryVariationsByProductNameAsync(string productName)
        {
            return await _repository.GetAllInventoryVariationsByProductNameAsync(productName);
        }
    }
}
