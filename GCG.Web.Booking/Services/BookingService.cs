using GCG.Web.Booking.Models;
using GCG.Web.Booking.Services.Interfaces;
using MudBlazor;

namespace GCG.Web.Booking.Services;

/// <summary>
/// Implementación mock del servicio de disponibilidad
/// </summary>
public class AvailabilityServiceMock : IAvailabilityService
{
    // Fechas mock bloqueadas
    private readonly List<DateTime> _unavailableDates = new()
    {
        new DateTime(2025, 1, 5),
        new DateTime(2025, 1, 6),
        new DateTime(2025, 1, 7),
        new DateTime(2025, 1, 15),
        new DateTime(2025, 1, 16),
        new DateTime(2025, 1, 17),
        new DateTime(2025, 2, 1),
        new DateTime(2025, 2, 2),
        new DateTime(2025, 2, 3),
    };

    public Task<AvailabilityResult> CheckAvailabilityAsync(DateTime checkIn, DateTime checkOut)
    {
        var result = new AvailabilityResult();

        // Validaciones básicas
        if (checkIn < DateTime.Today)
        {
            result.IsAvailable = false;
            result.Message = "La fecha de entrada no puede ser anterior a hoy.";
            return Task.FromResult(result);
        }

        if (checkOut <= checkIn)
        {
            result.IsAvailable = false;
            result.Message = "La fecha de salida debe ser posterior a la fecha de entrada.";
            return Task.FromResult(result);
        }

        var nights = (checkOut - checkIn).Days;
        if (nights < 1)
        {
            result.IsAvailable = false;
            result.Message = "La estadía mínima es de 1 noches.";
            return Task.FromResult(result);
        }

        // Verificar si hay conflicto con fechas bloqueadas
        var requestedDates = Enumerable.Range(0, nights)
            .Select(offset => checkIn.AddDays(offset))
            .ToList();

        var blockedDates = requestedDates
            .Where(d => _unavailableDates.Contains(d.Date))
            .ToList();

        if (blockedDates.Any())
        {
            result.IsAvailable = false;
            result.Message = $"Las siguientes fechas no están disponibles: {string.Join(", ", blockedDates.Select(d => d.ToString("dd/MM/yyyy")))}";
            result.UnavailableDates = blockedDates;
            return Task.FromResult(result);
        }

        result.IsAvailable = true;
        result.Message = "¡El rancho está disponible para tus fechas!";
        result.EstimatedPrice = 250m * nights; // $250 por noche

        return Task.FromResult(result);
    }

    public Task<List<DateTime>> GetUnavailableDatesAsync(DateTime startDate, DateTime endDate)
    {
        var unavailable = _unavailableDates
            .Where(d => d >= startDate && d <= endDate)
            .ToList();

        return Task.FromResult(unavailable);
    }

    public Task<decimal> CalculateBasePriceAsync(DateTime checkIn, DateTime checkOut)
    {
        var nights = (checkOut - checkIn).Days;
        var pricePerNight = 250m;
        var total = pricePerNight * nights;

        return Task.FromResult(total);
    }

    public Task<(bool IsValid, decimal DiscountPercentage)> ValidatePromoCodeAsync(string promoCode)
    {
        return Task.FromResult((false, 0m));
    }
}

/// <summary>
/// Implementación mock del servicio de reservas
/// </summary>
public class BookingServiceMock : IBookingService
{
    private readonly IAvailabilityService _availabilityService;

    // Almacenamiento temporal en memoria (simula DB)
    private static readonly Dictionary<string, BookingSummary> _bookings = new();

    public BookingServiceMock(IAvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }

    public async Task<BookingResult> CreatePendingBookingAsync(BookingRequest request)
    {
        // Simular delay de red
        await Task.Delay(500);

        // Verificar disponibilidad
        var availability = await _availabilityService.CheckAvailabilityAsync(
            request.CheckIn,
            request.CheckOut);

        if (!availability.IsAvailable)
        {
            return new BookingResult
            {
                Success = false,
                ErrorMessage = availability.Message
            };
        }

        // Calcular precios
        var priceBreakdown = await CalculatePriceBreakdownAsync(
            request.CheckIn,
            request.CheckOut,
            request.OptionalServiceIds,
            request.PromoCode);

        // Crear reserva pendiente
        var bookingId = Guid.NewGuid().ToString("N")[..8].ToUpper();
        var confirmationCode = $"RDS-{bookingId}";

        var summary = new BookingSummary
        {
            BookingId = bookingId,
            ConfirmationCode = confirmationCode,
            Status = BookingStatus.Pending,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            Nights = (request.CheckOut - request.CheckIn).Days,
            Adults = request.Adults,
            Children = request.Children,
            GuestName = request.GuestName,
            GuestEmail = request.GuestEmail,
            GuestPhone = request.GuestPhone,
            PriceBreakdown = priceBreakdown,
            CreatedAt = DateTime.Now
        };

        _bookings[bookingId] = summary;

        return new BookingResult
        {
            Success = true,
            BookingId = bookingId,
            ConfirmationCode = confirmationCode,
            TotalAmount = priceBreakdown.Total,
            DepositAmount = priceBreakdown.DepositRequired,
            CreatedAt = DateTime.Now
        };
    }

    public async Task<BookingResult> ConfirmBookingAsync(string bookingId, string paymentId)
    {
        await Task.Delay(300);

        if (!_bookings.TryGetValue(bookingId, out var booking))
        {
            return new BookingResult
            {
                Success = false,
                ErrorMessage = "Reserva no encontrada."
            };
        }

        booking.Status = BookingStatus.Confirmed;
        booking.PaidAt = DateTime.Now;

        return new BookingResult
        {
            Success = true,
            BookingId = bookingId,
            ConfirmationCode = booking.ConfirmationCode,
            TotalAmount = booking.PriceBreakdown.Total
        };
    }

    public Task<BookingSummary?> GetBookingSummaryAsync(string bookingId)
    {
        _bookings.TryGetValue(bookingId, out var booking);
        return Task.FromResult(booking);
    }

    public Task<bool> CancelBookingAsync(string bookingId, string reason)
    {
        if (_bookings.TryGetValue(bookingId, out var booking))
        {
            booking.Status = BookingStatus.Cancelled;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public async Task<PriceBreakdown> CalculatePriceBreakdownAsync(
        DateTime checkIn,
        DateTime checkOut,
        List<string> optionalServiceIds,
        string? promoCode = null)
    {
        var nights = (checkOut - checkIn).Days;
        var pricePerNight = 250m;
        var basePrice = pricePerNight * nights;

        var breakdown = new PriceBreakdown
        {
            Nights = nights,
            PricePerNight = pricePerNight,
            BasePrice = basePrice
        };

        // Calcular servicios opcionales
        var optionalServices = await GetOptionalServicesAsync();
        foreach (var serviceId in optionalServiceIds)
        {
            var service = optionalServices.FirstOrDefault(s => s.Id == serviceId);
            if (service != null)
            {
                var servicePrice = new OptionalServicePrice
                {
                    ServiceId = service.Id,
                    ServiceName = service.Name,
                    PricePerDay = service.PricePerDay,
                    Days = nights,
                    Total = service.PricePerDay * nights
                };
                breakdown.OptionalServices.Add(servicePrice);
            }
        }

        breakdown.OptionalServicesTotal = breakdown.OptionalServices.Sum(s => s.Total);
        breakdown.Subtotal = breakdown.BasePrice + breakdown.OptionalServicesTotal;

        // Aplicar descuento si hay código promo
        if (!string.IsNullOrEmpty(promoCode))
        {
            var (isValid, discountPercentage) = await ValidatePromoCodeAsync(promoCode);
            if (isValid)
            {
                breakdown.PromoCode = promoCode;
                breakdown.DiscountPercentage = discountPercentage;
                breakdown.DiscountAmount = breakdown.Subtotal * (discountPercentage / 100m);
            }
        }

        breakdown.Total = breakdown.Subtotal - breakdown.DiscountAmount;
        breakdown.DepositRequired = breakdown.Total * 0.5m; // 50%
        breakdown.BalanceDue = breakdown.Total - breakdown.DepositRequired;

        return breakdown;
    }

    public Task<List<OptionalBookingService>> GetOptionalServicesAsync()
    {
        var services = new List<OptionalBookingService>
        {
            new OptionalBookingService
            {
                Id = "chef",
                Name = "Chef Privado",
                Description = "Chef profesional que prepara tus comidas favoritas. Incluye compra de ingredientes.",
                Icon = Icons.Material.Filled.Restaurant,
                PricePerDay = 150m,
                ImageUrl = "https://picsum.photos/id/292/400/300",
                IsPopular = true
            },
            new OptionalBookingService
            {
                Id = "transport",
                Name = "Transporte Aeropuerto",
                Description = "Traslado desde/hacia el aeropuerto en vehículo con aire acondicionado.",
                Icon = Icons.Material.Filled.DirectionsCar,
                PricePerDay = 75m,
                ImageUrl = "https://picsum.photos/id/111/400/300",
                IsPopular = false
            },
            new OptionalBookingService
            {
                Id = "cleaning",
                Name = "Servicio de Limpieza Diario",
                Description = "Limpieza completa diaria de todas las áreas y cambio de toallas.",
                Icon = Icons.Material.Filled.CleaningServices,
                PricePerDay = 50m,
                ImageUrl = "https://picsum.photos/id/287/400/300",
                IsPopular = true
            },
            new OptionalBookingService
            {
                Id = "tours",
                Name = "Tours Personalizados",
                Description = "Tours guiados a volcanes, cascadas y pueblos coloniales. Incluye transporte.",
                Icon = Icons.Material.Filled.Hiking,
                PricePerDay = 120m,
                ImageUrl = "https://picsum.photos/id/1036/400/300",
                IsPopular = true
            }
        };

        return Task.FromResult(services);
    }

    public Task<(bool IsValid, decimal DiscountPercentage)> ValidatePromoCodeAsync(string promoCode)
    {
        // Códigos promocionales mock
        var validCodes = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
        {
            { "VERANO2025", 15m },
            { "PRIMERA", 20m },
            { "FAMILIA10", 10m }
        };

        if (validCodes.TryGetValue(promoCode, out var discount))
        {
            return Task.FromResult((true, discount));
        }

        return Task.FromResult((false, 0m));
    }
}