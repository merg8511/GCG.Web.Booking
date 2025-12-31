namespace GCG.Web.Booking.Models
{
    /// <summary>
    /// Request para crear una reserva
    /// </summary>
    public class BookingRequest
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public string? PromoCode { get; set; }
        public List<string> OptionalServiceIds { get; set; } = new();

        // Información del usuario (auto-llenado desde auth)
        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public string GuestPhone { get; set; } = string.Empty;
        public string? SpecialRequests { get; set; }
    }

    /// <summary>
    /// Respuesta del backend al crear una reserva
    /// </summary>
    public class BookingResult
    {
        public bool Success { get; set; }
        public string? BookingId { get; set; }
        public string? ConfirmationCode { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DepositAmount { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    /// <summary>
    /// Desglose detallado de precios
    /// </summary>
    public class PriceBreakdown
    {
        public int Nights { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal BasePrice { get; set; }

        public List<OptionalServicePrice> OptionalServices { get; set; } = new();
        public decimal OptionalServicesTotal { get; set; }

        public decimal Subtotal { get; set; }

        public string? PromoCode { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }

        public decimal Total { get; set; }
        public decimal DepositRequired { get; set; } // 50%
        public decimal BalanceDue { get; set; }
    }

    /// <summary>
    /// Precio de un servicio opcional
    /// </summary>
    public class OptionalServicePrice
    {
        public string ServiceId { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
        public int Days { get; set; }
        public decimal Total { get; set; }
    }

    /// <summary>
    /// Información de disponibilidad
    /// </summary>
    public class AvailabilityResult
    {
        public bool IsAvailable { get; set; }
        public string? Message { get; set; }
        public List<DateTime> UnavailableDates { get; set; } = new();
        public decimal EstimatedPrice { get; set; }
    }

    /// <summary>
    /// Servicio opcional disponible para reserva
    /// </summary>
    public class OptionalBookingService
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPopular { get; set; }
    }

    /// <summary>
    /// Resumen de reserva confirmada
    /// </summary>
    public class BookingSummary
    {
        public string BookingId { get; set; } = string.Empty;
        public string ConfirmationCode { get; set; } = string.Empty;
        public BookingStatus Status { get; set; }

        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Nights { get; set; }

        public int Adults { get; set; }
        public int Children { get; set; }

        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public string GuestPhone { get; set; } = string.Empty;

        public PriceBreakdown PriceBreakdown { get; set; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }

    /// <summary>
    /// Estados de una reserva
    /// </summary>
    public enum BookingStatus
    {
        Pending = 0,        // Creada pero no pagada
        Confirmed = 1,      // Pagada y confirmada
        Cancelled = 2,      // Cancelada
        Completed = 3       // Estadía completada
    }
}
