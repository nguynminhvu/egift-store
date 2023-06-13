using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModel.Request
{
    public class CartUpdateModel
    {
        public Guid CartId { get; set; }
        public List<CartItemUpdateModel> CartItemUpdate { get; set; }
    }
}
