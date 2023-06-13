using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModel.Response
{
    public class ProductCartItemViewModel
    {

        public string Name { get; set; } = null!;

        public double Price { get; set; }

        public string Description { get; set; } = null!;

        public int Stock { get; set; }

        public virtual ProductImageViewModel ProductImages { get; set; }

        public virtual ICollection<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

    }
}
