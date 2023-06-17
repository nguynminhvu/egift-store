using Microsoft.AspNetCore.Mvc;
using Persistence.ViewModel.Request;
using Persistence.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
	public interface IProductService
	{
		public Task<ProductViewModel> GetProduct(Guid id);
		public Task<IActionResult> CreateProduct(ProductCreateModel productCreateModel);
		public Task<IActionResult> GetProducts(ProductFilterModel productFilterModel);
		public Task<IActionResult> GetProductsByCategory(Guid categoryId);
		public Task<IActionResult> UpdateProduct(Guid id, ProductUpdateModel productUpdateModel);
		public Task<IActionResult> RemoveProduct(Guid id);
		public Task<bool> CheckStock(CartViewModel id);
		public Task<bool> UpdateStock(OrderViewModel ovm);
	}
}
