using MudBlazor;

namespace GCG.Web.Booking.Services
{
    /// <summary>
    /// Servicio para mantener el estado de la reserva durante todo el flujo
    /// </summary>

    public class BookingStateService
    {
        // Step 1: Dates and guests
        public DateRange? SelectedDates { get; set; }
        public int Adults { get; set; } = 2;
        public int Children { get; set; } = 0;
        public string? PromoCode { get; set; }

        // Step 2: Selected optional services
        public List<string> SelectedOptionalServices { get; set; } = new();

        //current flow step
        public int CurrentStep { get; set; } = 1;

        // Calculated Price
        public decimal BasePrice { get; set; }
        public decimal OptionalServicesTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalPrice { get; set; }

        // Available info
        public bool IsAvailable { get; set; }
        public string? UnavailabilityReason { get; set; }

        // Temporal booking ID (before payment)
        public string? PendingBookingId { get; set; }

        /// <summary>
        /// Validate step 1 is completed
        /// </summary>
        public bool IsStep1Valid()
        {
            return SelectedDates != null
                   && SelectedDates.Start.HasValue
                   && SelectedDates.End.HasValue
                   && Adults > 0
                   && (Adults + Children) <= 12; //TODO: Get max guest qty from backend
        }

        /// <summary>
        /// Validate if we can proceed to step 3
        /// </summary>
        public bool CanProceedToStep3()
        {
            return IsStep1Valid() && IsAvailable;
        }

        /// <summary>
        /// Calculate nights count
        /// </summary>
        public int GetNumberOfNights()
        {
            if (SelectedDates?.Start == null || SelectedDates?.End == null)
                return 0;

            return (SelectedDates.End.Value - SelectedDates.Start.Value).Days;
        }

        /// <summary>
        /// Calculate guests
        /// </summary>
        public int GetTotalGuests()
        {
            return Adults + Children;
        }

        /// <summary>
        /// Clear state
        /// </summary>
        public void Clear()
        {
            SelectedDates = null;
            Adults = 2;
            Children = 0;
            PromoCode = null;
            SelectedOptionalServices.Clear();
            CurrentStep = 1;
            BasePrice = 0;
            OptionalServicesTotal = 0;
            DiscountAmount = 0;
            TotalPrice = 0;
            IsAvailable = false;
            UnavailabilityReason = null;
            PendingBookingId = null;
        }

        /// <summary>
        /// Load records from CTA
        /// /// </summary>
        public void LoadFromFloatingCTA(DateRange? dates, int adults, int children, string? promoCode)
        {
            SelectedDates = dates;
            Adults = adults > 0 ? adults : 2;
            Children = children;
            PromoCode = promoCode;
            CurrentStep = 1;
        }

    }
}
