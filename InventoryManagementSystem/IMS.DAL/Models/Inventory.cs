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
    }
}
