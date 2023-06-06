using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModel.Response
{
    public class CustomerViewModel
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

        public bool IsActive { get; set; }
    }
}
