using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAL.Models
{
    internal class Sales
    {
        public int SalesID { get; set; }

        [Required]
        public string InvNo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime SalesDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount cannot be zero")]
        public decimal Amount { get; set; }

        [Required]
        public string CustomerName { get; set; }

        public string Address { get; set; }
        public string Tel { get; set; }

        [Required]
        public string SalesType { get; set; }

        public List<SalesLine> SalesLines { get; set; } = new List<SalesLine>();
    }
}
