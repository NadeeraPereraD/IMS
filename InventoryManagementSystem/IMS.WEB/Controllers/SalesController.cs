using IMS.BAL.Services;
using IMS.DAL.Models;
using IMS.WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WEB.Controllers
{
    public class SalesController : Controller
    {
        private readonly SalesService _salesService;
        private readonly ReportService _reportService;

        public SalesController(SalesService salesService, ReportService reportService)
        {
            _salesService = salesService;
            _reportService = reportService;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            var invoices = await _salesService.GetAllInvoicesAsync();
            return View(invoices);
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            var model = new Sales
            {
                SalesDate = DateTime.Now,
                InvNo = _salesService.GenerateInvoiceNumber()
            };
            return View(model);
        }

        // POST: Sales/Save
        [HttpPost]
        public async Task<IActionResult> Save(Sales model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.CustomerName) ||
                    string.IsNullOrWhiteSpace(model.SalesType))
                {
                    TempData["Error"] = "Please fill in all required fields.";
                    return View("Create", model);
                }

                if (model.Amount <= 0)
                {
                    TempData["Error"] = "Amount cannot be zero while saving the sales invoice.";
                    return View("Create", model);
                }

                if (model.SalesLines == null || !model.SalesLines.Any())
                {
                    TempData["Error"] = "Please add at least one product line.";
                    return View("Create", model);
                }

                var salesId = await _salesService.SaveInvoiceAsync(model);

                TempData["Success"] = model.SalesID == 0
                    ? "Invoice saved successfully!"
                    : "Invoice updated successfully!";

                return RedirectToAction("View", new { id = salesId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error saving invoice: {ex.Message}";
                return View("Create", model);
            }
        }

        // GET: Sales/View/5
        public async Task<IActionResult> View(int id)
        {
            var invoice = await _salesService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var invoice = await _salesService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View("Create", invoice);
        }

        // GET: Sales/PrintPdf/5
        public async Task<IActionResult> PrintPdf(int id)
        {
            try
            {
                var invoice = await _salesService.GetInvoiceByIdAsync(id);
                if (invoice == null)
                {
                    return NotFound();
                }

                var pdfBytes = _reportService.GenerateSalesInvoicePdf(invoice);

                return File(pdfBytes, "application/pdf", $"Invoice_{invoice.InvNo}.pdf");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error generating PDF: {ex.Message}";
                return RedirectToAction("View", new { id });
            }
        }
    }
}
