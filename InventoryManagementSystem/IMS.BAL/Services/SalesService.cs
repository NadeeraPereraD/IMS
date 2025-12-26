using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.DAL.Models;
using IMS.DAL.Repositories;

namespace IMS.BAL.Services
{
    public class SalesService
    {
        private readonly SalesRepository _repository;

        public SalesService(string connectionString)
        {
            _repository = new SalesRepository(connectionString);
        }

        public async Task<List<Sales>> GetAllInvoicesAsync()
        {
            return await _repository.GetAllInvoicesAsync();
        }

        public async Task<Sales> GetInvoiceByIdAsync(int salesId)
        {
            return await _repository.GetInvoiceByIdAsync(salesId);
        }

        public async Task<int> SaveInvoiceAsync(Sales sales)
        {
            // Business validation
            if (string.IsNullOrWhiteSpace(sales.CustomerName))
                throw new ArgumentException("Customer name is required");

            if (sales.Amount <= 0)
                throw new ArgumentException("Amount must be greater than zero");

            if (sales.SalesLines == null || !sales.SalesLines.Any())
                throw new ArgumentException("At least one product line is required");

            return await _repository.SaveInvoiceWithLinesAsync(sales);
        }

        public string GenerateInvoiceNumber()
        {
            return $"INV-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";
        }
    }
}
