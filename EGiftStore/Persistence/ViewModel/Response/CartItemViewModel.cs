﻿using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModel.Response
{
    public class CartItemViewModel
    {
        public int Quantity { get; set; }
        public ProductViewModel ProductViewModel { get; set; }
    }
}
