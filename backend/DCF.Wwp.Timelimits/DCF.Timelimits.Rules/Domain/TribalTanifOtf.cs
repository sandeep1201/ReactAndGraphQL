using System;

namespace DCF.Timelimits.Rules.Domain
{
    public class TribalTanifOtf : ServiceBenfit
    {
        public Int32 ReservationPopulation { get; }
        public Int32 UnemploymentPercentage { get; }

        public TribalTanifOtf(Int32 reservationPopulation, Int32 unemploymentPercentage) : base("OTF")
        {
            this.ReservationPopulation = reservationPopulation;
            this.UnemploymentPercentage = unemploymentPercentage;
        }
    }
}