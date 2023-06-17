using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModel.Response
{
	public class OrderDetailViewModel
	{
		public Guid OrderId { get; set; }

		public Guid ProductId { get; set; }

		public int Quantity { get; set; }

		public double? Discount { get; set; }

		public DateTime CreateDate { get; set; }

	}
}
