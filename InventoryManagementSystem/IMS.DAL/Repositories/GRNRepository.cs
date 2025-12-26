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
    public class GRNRepository
    {
        private readonly string _connectionString;

        public GRNRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<GRN>> GetAllGRNsAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            var grns = await conn.QueryAsync<GRN>(
                "sp_GetAllGRNs",
                commandType: CommandType.StoredProcedure
            );
            return grns.ToList();
        }

        public async Task<GRN> GetGRNByIdAsync(int grnId)
        {
            using var conn = new SqlConnection(_connectionString);

            using var multi = await conn.QueryMultipleAsync(
                "sp_GetGRNById",
                new { GRNID = grnId },
                commandType: CommandType.StoredProcedure
            );

            var grn = await multi.ReadFirstOrDefaultAsync<GRN>();
            if (grn != null)
            {
                grn.GRNLines = (await multi.ReadAsync<GRNLine>()).ToList();
            }

            return grn;
        }

        public async Task<int> SaveGRNWithLinesAsync(GRN grn)
        {
            using var conn = new SqlConnection(_connectionString);

            var grnLinesTable = new DataTable();
            grnLinesTable.Columns.Add("GRNLineID", typeof(int));
            grnLinesTable.Columns.Add("ProductName", typeof(string));
            grnLinesTable.Columns.Add("Quantity", typeof(int));
            grnLinesTable.Columns.Add("UnitPrice", typeof(decimal));
            grnLinesTable.Columns.Add("SellingPrice", typeof(decimal));
            grnLinesTable.Columns.Add("Total", typeof(decimal));

            foreach (var line in grn.GRNLines)
            {
                grnLinesTable.Rows.Add(
                    line.GRNLineID > 0 ? (object)line.GRNLineID : DBNull.Value,
                    line.ProductName,
                    line.Quantity,
                    line.UnitPrice,
                    line.SellingPrice,
                    line.Total
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@GRNID", grn.GRNID, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@GRNNo", grn.GRNNo);
            parameters.Add("@GRNDate", grn.GRNDate);
            parameters.Add("@Amount", grn.Amount);
            parameters.Add("@VendorName", grn.VendorName);
            parameters.Add("@Address", grn.Address);
            parameters.Add("@Tel", grn.Tel);
            parameters.Add("@GRNLines", grnLinesTable.AsTableValuedParameter("GRNLineTableType"));

            await conn.ExecuteAsync(
                "sp_SaveGRNWithLines",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@GRNID");
        }
    }
}
