using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class Customer
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? Address { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime LastUpdate { get; set; }

    public DateTime? ExpireToken { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
