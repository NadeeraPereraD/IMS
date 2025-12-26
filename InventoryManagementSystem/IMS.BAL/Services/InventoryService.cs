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
    }
}
