using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAL.Models
{
    public class Inventory
    {
        public int InventoryID { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantity { get; set; }
        public decimal? LastUnitPrice { get; set; }
        public decimal? LastSellingPrice { get; set; }
        public DateTime LastUpdated { get; set; }

        // Additional properties for display
        public string GRNNumbers { get; set; }
        public int? GRNCount { get; set; }
    }

    // Detailed Inventory Model for showing individual GRN records
    public class InventoryDetail
    {
        public int InventoryID { get; set; }
        public int GRNID { get; set; }
        public string GRNNo { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime? GRNDate { get; set; }
        public string VendorName { get; set; }
    }
}
