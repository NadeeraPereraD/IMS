using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAL.Models
{
    internal class GRNLine
    {
        public int GRNLineID { get; set; }
        public int GRNID { get; set; }

        [Required(ErrorMessage = "Product Name must be entered")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Quantity must be entered")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit Price must be entered")]
        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Selling Price must be entered")]
        [Range(0.01, double.MaxValue)]
        public decimal SellingPrice { get; set; }

        public decimal Total { get; set; }
    }
}
