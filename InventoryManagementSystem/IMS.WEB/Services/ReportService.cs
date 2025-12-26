using AspNetCore.Reporting;
using IMS.DAL.Models;
using System.Data;
using System.Text;

namespace IMS.WEB.Services
{
    public class ReportService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public byte[] GenerateSalesInvoicePdf(Sales invoice)
        {
            try
            {
                string reportPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "SalesInvoice.rdlc");

                if (!File.Exists(reportPath))
                {
                    throw new FileNotFoundException($"Report file not found at: {reportPath}");
                }

                var localReport = new LocalReport(reportPath);

                var headerData = CreateSalesHeaderDataTable(invoice);
                var linesData = CreateSalesLinesDataTable(invoice.SalesLines);

                localReport.AddDataSource("InvoiceHeader", headerData);
                localReport.AddDataSource("InvoiceLines", linesData);

                var result = localReport.Execute(RenderType.Pdf, 1);

                return result.MainStream;
            }
            catch (Exception ex)
            {
                throw new Exception($"PDF Generation Error: {ex.Message}", ex);
            }
        }

        public byte[] GenerateGRNPdf(GRN grn)
        {
            try
            {
                string reportPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "GRN.rdlc");

                if (!File.Exists(reportPath))
                {
                    throw new FileNotFoundException($"Report file not found at: {reportPath}");
                }

                var localReport = new LocalReport(reportPath);

                var headerData = CreateGRNHeaderDataTable(grn);
                var linesData = CreateGRNLinesDataTable(grn.GRNLines);

                localReport.AddDataSource("GRNHeader", headerData);
                localReport.AddDataSource("GRNLines", linesData);

                var result = localReport.Execute(RenderType.Pdf, 1);

                return result.MainStream;
            }
            catch (Exception ex)
            {
                throw new Exception($"GRN PDF Generation Error: {ex.Message}", ex);
            }
        }

        private DataTable CreateSalesHeaderDataTable(Sales invoice)
        {
            var dt = new DataTable("InvoiceHeader");

            dt.Columns.Add("InvNo", typeof(string));
            dt.Columns.Add("SalesDate", typeof(DateTime));
            dt.Columns.Add("CustomerName", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("Tel", typeof(string));
            dt.Columns.Add("SalesType", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));

            var row = dt.NewRow();
            row["InvNo"] = invoice.InvNo ?? "";
            row["SalesDate"] = invoice.SalesDate;
            row["CustomerName"] = invoice.CustomerName ?? "";
            row["Address"] = invoice.Address ?? "";
            row["Tel"] = invoice.Tel ?? "";
            row["SalesType"] = invoice.SalesType ?? "";
            row["Amount"] = invoice.Amount;

            dt.Rows.Add(row);

            return dt;
        }

        private DataTable CreateSalesLinesDataTable(List<SalesLine> lines)
        {
            var dt = new DataTable("InvoiceLines");

            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("UnitPrice", typeof(decimal));
            dt.Columns.Add("Total", typeof(decimal));

            if (lines != null)
            {
                foreach (var line in lines)
                {
                    var row = dt.NewRow();
                    row["ProductName"] = line.ProductName ?? "";
                    row["Quantity"] = line.Quantity;
                    row["UnitPrice"] = line.UnitPrice;
                    row["Total"] = line.Total;

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        private DataTable CreateGRNHeaderDataTable(GRN grn)
        {
            var dt = new DataTable("GRNHeader");

            dt.Columns.Add("GRNNo", typeof(string));
            dt.Columns.Add("GRNDate", typeof(DateTime));
            dt.Columns.Add("VendorName", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("Tel", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));

            var row = dt.NewRow();
            row["GRNNo"] = grn.GRNNo ?? "";
            row["GRNDate"] = grn.GRNDate;
            row["VendorName"] = grn.VendorName ?? "";
            row["Address"] = grn.Address ?? "";
            row["Tel"] = grn.Tel ?? "";
            row["Amount"] = grn.Amount;

            dt.Rows.Add(row);

            return dt;
        }

        private DataTable CreateGRNLinesDataTable(List<GRNLine> lines)
        {
            var dt = new DataTable("GRNLines");

            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("UnitPrice", typeof(decimal));
            dt.Columns.Add("SellingPrice", typeof(decimal));
            dt.Columns.Add("Total", typeof(decimal));

            if (lines != null)
            {
                foreach (var line in lines)
                {
                    var row = dt.NewRow();
                    row["ProductName"] = line.ProductName ?? "";
                    row["Quantity"] = line.Quantity;
                    row["UnitPrice"] = line.UnitPrice;
                    row["SellingPrice"] = line.SellingPrice;
                    row["Total"] = line.Total;

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }
    }
}
