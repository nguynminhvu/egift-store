using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModel.Response
{
	public class OrderViewModel
	{
		public Guid Id { get; set; }

		public Guid CustomerId { get; set; }

		public double Amount { get; set; }

		public DateTime CreateDate { get; set; }

		public string Status { get; set; } = null!;

		public virtual ICollection<OrderDetailViewModel> OrderDetails { get; set; } = new List<OrderDetailViewModel>();
	}
}
