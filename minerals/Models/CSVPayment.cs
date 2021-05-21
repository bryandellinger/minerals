using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace  Models
{
    public class CSVPayment
    {
        public long Id { get; set; }
        public long? UploadPaymentId { get; set; }
        public UploadPayment UploadPayment { get; set; }

        public long? WellId { get; set; }
        public Well Well { get; set; }
        public string ApiNum { get; set; }
        public decimal? GasRoyalty { get; set; }
        public decimal? GasProd { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal? NRI { get; set; }
        public DateTime? ProductionDate { get; set; }
        public int? PostMonth { get; set; }

        public int? PostYear { get; set; }


    }
}
