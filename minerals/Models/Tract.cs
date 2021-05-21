using System;
using System.Collections.Generic;

namespace Models
{
    public class Tract
    {
        public long Id { get; set; }

        public string TractNum { get; set; }

        public string AltTractNum { get; set; }

        public DateTime? LeaseDate { get; set; }

        public decimal? Acreage { get; set; }

        public decimal? RoyaltyPercent { get; set; }

        public decimal? RoyaltyFloor { get; set; }

        public Single? RoyaltyFlDollar { get; set; }

        public decimal? Rent { get; set; }

        public bool? Administrative { get; set; }

        public string Notes { get; set; }

        public DateTime? TerminatedDate { get; set; }

        public bool? Terminated { get; set; }

        public decimal? IncreasedRentAmnt { get; set; }

        public int? YearIncRentEffec { get; set; }

        public decimal? RentAsOfToday { get; set; }

        public string NotesAdditional { get; set; }

        public bool? RiverTract { get; set; }

        public bool? BondsRequired { get; set; }

        public DateTime? RentalMonthDue { get; set; }

        public DateTime? RentalAnnivDate { get; set; }

        public bool? RentalsRequired { get; set; }

        public string Horizon { get; set; }

        public string StateParkName { get; set; }

        public string LandIncluded { get; set; }

        public IEnumerable<DistrictTractJunction> DistrictTracts { get; set; }

        public long? LesseeId { get; set; }

        public Lessee Lessee { get; set; }

        public DateTime? ReversionDate { get; set; }

        public IEnumerable<TractLesseeJunction> TractLessee { get; set; }

        public IEnumerable<Pad> Pads { get; set; }
        public IEnumerable<TractUnitJunction> TractUnitJunctions { get; set; }

    }
}
