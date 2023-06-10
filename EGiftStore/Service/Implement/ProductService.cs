using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entities;
using Persistence.ViewModel.Request;
using Persistence.ViewModel.Response;
using Repository;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implement
{
    public class ProductService : IProductService
    {

        private IMapper _mapper;
        private IUnitIOfWork _uow;

        public ProductService(IUnitIOfWork unitIOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _uow = unitIOfWork;
        }

        public async Task<IActionResult> CreateProduct(ProductCreateModel productCreateModel)
        {
            Product product = new Product
            {
                Id = Guid.NewGuid(),
                Name = productCreateModel.Name,
                Price = productCreateModel.Price,
                Description = productCreateModel.Description,
                Stock = productCreateModel.Stock,
                Status = "Active",
                CreateDate = DateTime.Now,
                LastUpdate = DateTime.Now
            };
            await _uow.ProductRepository.AddAsync(product);
            return await _uow.SaveChangesAsync() > 0 ? new JsonResult(await GetProduct(product.Id)) : new StatusCodeResult(500);
        }

        public Task<ProductViewModel> GetProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetProducts(ProductFilterModel productFilterModel)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetProductsByCategory(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductViewModel> RemoveProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> UpdateProduct(ProductUpdateModel productUpdateModel)
        {
            throw new NotImplementedException();
        }
    }
}
