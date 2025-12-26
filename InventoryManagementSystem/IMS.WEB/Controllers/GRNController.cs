using IMS.BAL.Services;
using IMS.DAL.Models;
using IMS.WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WEB.Controllers
{
    public class GRNController : Controller
    {
        private readonly GRNService _grnService;
        private readonly ReportService _reportService;

        public GRNController(GRNService grnService, ReportService reportService)
        {
            _grnService = grnService;
            _reportService = reportService;
        }

        // GET: GRN
        public async Task<IActionResult> Index()
        {
            var grns = await _grnService.GetAllGRNsAsync();
            return View(grns);
        }

        // GET: GRN/Create
        public IActionResult Create()
        {
            var model = new GRN
            {
                GRNDate = DateTime.Now,
                GRNNo = _grnService.GenerateGRNNumber()
            };
            return View(model);
        }

        // POST: GRN/Save
        [HttpPost]
        public async Task<IActionResult> Save(GRN model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.VendorName))
                {
                    TempData["Error"] = "Please fill in all required fields.";
                    return View("Create", model);
                }

                if (model.Amount <= 0)
                {
                    TempData["Error"] = "Amount cannot be zero while saving the GRN.";
                    return View("Create", model);
                }

                if (model.GRNLines == null || !model.GRNLines.Any())
                {
                    TempData["Error"] = "Please add at least one product line.";
                    return View("Create", model);
                }

                var grnId = await _grnService.SaveGRNAsync(model);

                TempData["Success"] = model.GRNID == 0
                    ? "GRN saved successfully!"
                    : "GRN updated successfully!";

                return RedirectToAction("View", new { id = grnId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error saving GRN: {ex.Message}";
                return View("Create", model);
            }
        }

        // GET: GRN/View/5
        public async Task<IActionResult> View(int id)
        {
            var grn = await _grnService.GetGRNByIdAsync(id);
            if (grn == null)
            {
                return NotFound();
            }
            return View(grn);
        }

        // GET: GRN/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var grn = await _grnService.GetGRNByIdAsync(id);
            if (grn == null)
            {
                return NotFound();
            }
            return View("Create", grn);
        }

        // GET: GRN/PrintPdf/5
        public async Task<IActionResult> PrintPdf(int id)
        {
            try
            {
                var grn = await _grnService.GetGRNByIdAsync(id);
                if (grn == null)
                {
                    return NotFound();
                }

                var pdfBytes = _reportService.GenerateGRNPdf(grn);

                return File(pdfBytes, "application/pdf", $"GRN_{grn.GRNNo}.pdf");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error generating PDF: {ex.Message}";
                return RedirectToAction("View", new { id });
            }
        }
    }
}
