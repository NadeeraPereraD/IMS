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
    public class SalesRepository
    {
        private readonly string _connectionString;

        public SalesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Sales>> GetAllInvoicesAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            var invoices = await conn.QueryAsync<Sales>(
                "sp_GetAllSalesInvoices",
                commandType: CommandType.StoredProcedure
            );
            return invoices.ToList();
        }

        public async Task<Sales> GetInvoiceByIdAsync(int salesId)
        {
            using var conn = new SqlConnection(_connectionString);

            using var multi = await conn.QueryMultipleAsync(
                "sp_GetSalesInvoiceById",
                new { SalesID = salesId },
                commandType: CommandType.StoredProcedure
            );

            var invoice = await multi.ReadFirstOrDefaultAsync<Sales>();
            if (invoice != null)
            {
                invoice.SalesLines = (await multi.ReadAsync<SalesLine>()).ToList();
            }

            return invoice;
        }

        public async Task<int> SaveInvoiceWithLinesAsync(Sales sales)
        {
            using var conn = new SqlConnection(_connectionString);

            var salesLinesTable = new DataTable();
            salesLinesTable.Columns.Add("SalesLineID", typeof(int));
            salesLinesTable.Columns.Add("ProductName", typeof(string));
            salesLinesTable.Columns.Add("Quantity", typeof(int));
            salesLinesTable.Columns.Add("UnitPrice", typeof(decimal));
            salesLinesTable.Columns.Add("Total", typeof(decimal));

            foreach (var line in sales.SalesLines)
            {
                salesLinesTable.Rows.Add(
                    line.SalesLineID > 0 ? (object)line.SalesLineID : DBNull.Value,
                    line.ProductName,
                    line.Quantity,
                    line.UnitPrice,
                    line.Total
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@SalesID", sales.SalesID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@InvNo", sales.InvNo);
            parameters.Add("@SalesDate", sales.SalesDate);
            parameters.Add("@Amount", sales.Amount);
            parameters.Add("@CustomerName", sales.CustomerName);
            parameters.Add("@Address", sales.Address);
            parameters.Add("@Tel", sales.Tel);
            parameters.Add("@SalesType", sales.SalesType);
            parameters.Add("@SalesLines", salesLinesTable.AsTableValuedParameter("SalesLineTableType"));

            await conn.ExecuteAsync(
                "sp_SaveSalesInvoiceWithLines",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@SalesID");
        }
    }
}
