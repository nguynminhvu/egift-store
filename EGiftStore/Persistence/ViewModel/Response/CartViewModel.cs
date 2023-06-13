using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModel.Response
{
    public class CartViewModel
    {
        public Guid Id { get; set; }
        public List<CartItemViewModel> CartItems { get; set; }
    }
}
