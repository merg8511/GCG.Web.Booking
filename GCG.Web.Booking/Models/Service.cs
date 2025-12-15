namespace GCG.Web.Booking.Models;

public partial class Service
{
    public string Id { get; set; }

    public string CategoryId { get; set; }

    public string Title { get; set; }

    public string Subtitle { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public sbyte IsActive { get; set; }

    public sbyte IsOptional { get; set; }

    public sbyte IsFeatured { get; set; }

    public decimal? PricePerDay { get; set; }

    public DateTime? CreatedAt { get; set; }

    public sbyte Deleted { get; set; }
    public virtual ServiceCategory Category { get; set; }

    //CUSTOM PROPERTIES
    public bool IsActiveBool
    {
        get => IsActive == 1;
        set => IsActive = (sbyte)(value ? 1 : 0);
    }

    public bool IsOptionalBool
    {
        get => IsOptional == 1;
        set => IsOptional = (sbyte)(value ? 1 : 0);
    }

    public bool IsFeaturedBool
    {
        get => IsFeatured == 1;
        set => IsFeatured = (sbyte)(value ? 1 : 0);
    }
}