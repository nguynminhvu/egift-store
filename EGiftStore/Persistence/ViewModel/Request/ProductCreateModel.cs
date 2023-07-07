using Microsoft.AspNetCore.Http;
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

        public List<Guid> CategoryIds { get; set; }

        public List<IFormFile> IFormFiles { get; set; } = new List<IFormFile>();

    }
}
