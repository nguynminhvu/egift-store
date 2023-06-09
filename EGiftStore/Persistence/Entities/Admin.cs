using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class Admin
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? ExpireToken { get; set; }
}
