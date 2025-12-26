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
        private readonly InventoryService _inventoryService;

        public SalesController(SalesService salesService, ReportService reportService, InventoryService inventoryService)
        {
            _salesService = salesService;
            _reportService = reportService;
            _inventoryService = inventoryService;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            var invoices = await _salesService.GetAllInvoicesAsync();
            return View(invoices);
        }

        // GET: Sales/Create
        public async Task<IActionResult> Create()
        {
            var model = new Sales
            {
                SalesDate = DateTime.Now,
                InvNo = _salesService.GenerateInvoiceNumber()
            };

            // Load available inventory
            ViewBag.Inventory = await _inventoryService.GetAvailableInventoryAsync();

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
                    ViewBag.Inventory = await _inventoryService.GetAvailableInventoryAsync();
                    return View("Create", model);
                }

                if (model.Amount <= 0)
                {
                    TempData["Error"] = "Amount cannot be zero while saving the sales invoice.";
                    ViewBag.Inventory = await _inventoryService.GetAvailableInventoryAsync();
                    return View("Create", model);
                }

                if (model.SalesLines == null || !model.SalesLines.Any())
                {
                    TempData["Error"] = "Please add at least one product line.";
                    ViewBag.Inventory = await _inventoryService.GetAvailableInventoryAsync();
                    return View("Create", model);
                }

                // Validate inventory availability
                foreach (var line in model.SalesLines)
                {
                    var inventory = await _inventoryService.GetInventoryByProductNameAsync(line.ProductName);
                    if (inventory == null || inventory.TotalQuantity < line.Quantity)
                    {
                        TempData["Error"] = $"Insufficient inventory for product: {line.ProductName}. Available: {inventory?.TotalQuantity ?? 0}";
                        ViewBag.Inventory = await _inventoryService.GetAvailableInventoryAsync();
                        return View("Create", model);
                    }
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
                ViewBag.Inventory = await _inventoryService.GetAvailableInventoryAsync();
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

            ViewBag.Inventory = await _inventoryService.GetAllInventoryAsync();
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

        // API endpoint to get inventory item details
        [HttpGet]
        public async Task<IActionResult> GetInventoryItem(string productName)
        {
            var inventory = await _inventoryService.GetInventoryByProductNameAsync(productName);
            if (inventory == null)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            return Json(new
            {
                success = true,
                productName = inventory.ProductName,
                availableQuantity = inventory.TotalQuantity,
                sellingPrice = inventory.LastSellingPrice ?? 0
            });
        }
    }
}
