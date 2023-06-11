﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModel.Response
{
    public class ProductImageViewModel
    {
        public Guid Id { get; set; }

        public string Url { get; set; } = null!;

        public string? Type { get; set; }

    }
}
