using System;
using System.Collections.Generic;

namespace GCG.Web.Booking.Models;

public partial class ServiceCategory
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Icon { get; set; }

    public int Order { get; set; }

    public sbyte IsActive { get; set; }

    public sbyte Deleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    //CUSTOM PROPERTIES
    public bool IsActiveBool
    {
        get => IsActive == 1;
        set => IsActive = (sbyte)(value ? 1 : 0);
    }

}