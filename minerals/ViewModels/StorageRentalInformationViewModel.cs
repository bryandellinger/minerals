using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.ViewModels
{
    public class StorageRentalInformationViewModel
    {
      public  long Id { get; set; }
      public decimal LeasedTractAcreage { get; set; }
      public DateTime? EffectiveDate { get; set; }
      public decimal? SecondThroughFourthYearDelayRental { get; set; }
      public decimal? FifthYearOnwardDelayRental { get; set; }

    }
}
