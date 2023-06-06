using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class ProductImage
{
    public Guid Id { get; set; }

    public string Url { get; set; } = null!;

    public string? Type { get; set; }

    public Guid ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
