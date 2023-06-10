using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModel.Request
{
    public class ProductCreateModel
    {
        public string Name { get; set; } = null!;

        public double Price { get; set; }

        public string Description { get; set; } = null!;

        public int Stock { get; set; }
        public List<Guid> CategoryId { get; set; }
        public List<string> ImageUrl { get; set; }

    }
}
