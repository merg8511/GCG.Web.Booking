using GCG.Web.Booking.Models;

namespace GCG.Web.Booking.Services.Interfaces
{
    /// <summary>
    /// Servicio para gestionar disponibilidad del rancho
    /// </summary>
    public interface IAvailabilityService
    {
        /// <summary>
        /// Verifica disponibilidad para un rango de fechas
        /// </summary>
        Task<AvailabilityResult> CheckAvailabilityAsync(DateTime checkIn, DateTime checkOut);

        /// <summary>
        /// Obtiene fechas bloqueadas/reservadas
        /// </summary>
        Task<List<DateTime>> GetUnavailableDatesAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Calcula precio estimado para fechas específicas
        /// </summary>
        Task<decimal> CalculateBasePriceAsync(DateTime checkIn, DateTime checkOut);

        /// <summary>
        /// Validar el promo code
        /// </summary>
        Task<(bool IsValid, decimal DiscountPercentage)> ValidatePromoCodeAsync(string promoCode);
    }

    /// <summary>
    /// Servicio para gestionar reservas
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Crea una reserva pendiente (antes del pago)
        /// </summary>
        Task<BookingResult> CreatePendingBookingAsync(BookingRequest request);

        /// <summary>
        /// Confirma y actualiza una reserva después del pago
        /// </summary>
        Task<BookingResult> ConfirmBookingAsync(string bookingId, string paymentId);

        /// <summary>
        /// Obtiene resumen de una reserva
        /// </summary>
        Task<BookingSummary?> GetBookingSummaryAsync(string bookingId);

        /// <summary>
        /// Cancela una reserva
        /// </summary>
        Task<bool> CancelBookingAsync(string bookingId, string reason);

        /// <summary>
        /// Calcula desglose completo de precios
        /// </summary>
        Task<PriceBreakdown> CalculatePriceBreakdownAsync(
            DateTime checkIn,
            DateTime checkOut,
            List<string> optionalServiceIds,
            string? promoCode = null);

        /// <summary>
        /// Obtiene servicios opcionales disponibles
        /// </summary>
        Task<List<OptionalBookingService>> GetOptionalServicesAsync();

        /// <summary>
        /// Valida código promocional
        /// </summary>
        Task<(bool IsValid, decimal DiscountPercentage)> ValidatePromoCodeAsync(string promoCode);
    }
}
