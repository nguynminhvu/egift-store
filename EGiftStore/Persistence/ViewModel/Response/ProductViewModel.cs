using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModel.Response
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public double Price { get; set; }

        public string Description { get; set; } = null!;

        public int Stock { get; set; }

        public virtual ICollection<ProductImageViewModel> ProductImages { get; set; } = new List<ProductImageViewModel>();

        public virtual ICollection<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

    }
}
