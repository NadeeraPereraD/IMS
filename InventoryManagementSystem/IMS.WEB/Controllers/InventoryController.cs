using IMS.BAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WEB.Controllers
{
    public class InventoryController : Controller
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // GET: Inventory (Summary View)
        public async Task<IActionResult> Index()
        {
            var inventory = await _inventoryService.GetAllInventoryAsync();
            return View(inventory);
        }

        // GET: Inventory/Detail (Detailed View - Shows each GRN separately)
        public async Task<IActionResult> Detail()
        {
            var inventoryDetail = await _inventoryService.GetInventoryDetailAsync();
            return View(inventoryDetail);
        }
    }
}
