using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
