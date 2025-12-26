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

        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            var inventory = await _inventoryService.GetAllInventoryAsync();
            return View(inventory);
        }
    }
}
