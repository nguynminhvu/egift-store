using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class OrderDetail
{
    public Guid OrderId { get; set; }

    public Guid Product { get; set; }

    public int Quantity { get; set; }

    public byte[]? Discount { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product ProductNavigation { get; set; } = null!;
}
