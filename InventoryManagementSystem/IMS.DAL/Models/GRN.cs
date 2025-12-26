using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAL.Models
{
    internal class GRN
    {
        public int GRNID { get; set; }

        [Required]
        public string GRNNo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime GRNDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount cannot be zero")]
        public decimal Amount { get; set; }

        [Required]
        public string VendorName { get; set; }

        public string Address { get; set; }
        public string Tel { get; set; }

        public List<GRNLine> GRNLines { get; set; } = new List<GRNLine>();
    }
}
