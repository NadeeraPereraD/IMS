using IMS.DAL.Models;
using IMS.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.BAL.Services
{
    public class GRNService
    {
        private readonly GRNRepository _repository;

        public GRNService(string connectionString)
        {
            _repository = new GRNRepository(connectionString);
        }

        public async Task<List<GRN>> GetAllGRNsAsync()
        {
            return await _repository.GetAllGRNsAsync();
        }

        public async Task<GRN> GetGRNByIdAsync(int grnId)
        {
            return await _repository.GetGRNByIdAsync(grnId);
        }

        public async Task<int> SaveGRNAsync(GRN grn)
        {
            // Business validation
            if (string.IsNullOrWhiteSpace(grn.VendorName))
                throw new ArgumentException("Vendor name is required");

            if (grn.Amount <= 0)
                throw new ArgumentException("Amount must be greater than zero");

            if (grn.GRNLines == null || !grn.GRNLines.Any())
                throw new ArgumentException("At least one product line is required");

            return await _repository.SaveGRNWithLinesAsync(grn);
        }

        public string GenerateGRNNumber()
        {
            return $"GRN-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";
        }
    }
}
