using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModel.Request
{
    public class CustomerRegisterViewModel
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Fullname { get; set; }

        public string? Address { get; set; }

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }
    }
}
