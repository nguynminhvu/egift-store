using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private string ACTIVE = "Active";
        private string INACTIVE = "InActive";
        private IMapper _mapper;
        private IUnitIOfWork _uow;

        public ProductService(IUnitIOfWork unitIOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _uow = unitIOfWork;
        }

        public async Task<IActionResult> CreateProduct(ProductCreateModel productCreateModel)
        {
            if (productCreateModel.CategoryId == null || productCreateModel.ImageUrl == null)
            {
                return new StatusCodeResult(400);
            }
            var category = await _uow.CategoryRepository.GetEntitiesPredicate(x => productCreateModel.CategoryId.Contains(x.Id)).Distinct().ToListAsync();
            Product product = new Product
            {
                Id = Guid.NewGuid(),
                Name = productCreateModel.Name,
                Price = productCreateModel.Price,
                Description = productCreateModel.Description,
                Stock = productCreateModel.Stock,
                Status = ACTIVE,
                CreateDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Categories = category
            };
            await _uow.ProductRepository.AddAsync(product);
            foreach (var item in productCreateModel.ImageUrl)
            {
                await _uow.ProductImageRepository.AddAsync(new ProductImage
                {
                    Id = Guid.NewGuid(),
                    Url = item,
                    ProductId = product.Id,
                    Product = product
                });
            }
            return await _uow.SaveChangesAsync() > 0 ? new JsonResult(await GetProduct(product.Id)) : new StatusCodeResult(500);
        }

        public async Task<ProductViewModel> GetProduct(Guid id)
        {
            var product = await _uow.ProductRepository.FirstOrDefaultAsync(x => x.Id.Equals(id) && x.Status.Equals(ACTIVE), x => x.Categories, x => x.ProductImages);
            return _mapper.Map<ProductViewModel>(product);
        }

        public async Task<IActionResult> GetProducts(ProductFilterModel productFilterModel)
        {
            var productQuery = _uow.ProductRepository.GetAll();

            if (productFilterModel.CategoryName != null)
            {
                productQuery = _uow.ProductRepository.GetEntitiesPredicate(x => x.Categories.Any(x => x.Name.ToLower().Contains(productFilterModel.CategoryName.ToLower())));
            }
            if (productFilterModel.Name != null)
            {
                productQuery = productQuery.Where(x => x.Name.ToLower().Contains(productFilterModel.Name.ToLower()));
            }
            if (productFilterModel.PriceFrom.HasValue)
            {
                productQuery = productQuery.Where(x => x.Price >= productFilterModel.PriceFrom);
            }
            if (productFilterModel.PriceTo.HasValue)
            {
                productQuery = productQuery.Where(x => x.Price <= productFilterModel.PriceTo);
            }
            var product = await productQuery.ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return new JsonResult(product);
        }

        public async Task<IActionResult> GetProductsByCategory(Guid categoryId)
        {
            var rs = await _uow.ProductRepository.GetEntitiesPredicate(x => x.Categories.Any(x => x.Id.Equals(categoryId))).ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return new JsonResult(rs);
        }

        public async Task<IActionResult> RemoveProduct(Guid id)
        {
            var product = await _uow.ProductRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (product != null)
            {
                product.Status = INACTIVE;
                return await _uow.SaveChangesAsync() > 0 ? new StatusCodeResult(204) : new StatusCodeResult(500);
            }
            return new StatusCodeResult(400);
        }

        public async Task<IActionResult> UpdateProduct(Guid id, ProductUpdateModel productUpdateModel)
        {
            var product = await _uow.ProductRepository.FirstOrDefaultAsync(x => x.Id.Equals(id), x => x.Categories, x => x.ProductImages);
            if (product != null)
            {
                product.Name = productUpdateModel.Name ?? product.Name;
                product.Price = productUpdateModel.Price ?? product.Price;
                product.Description = productUpdateModel.Description ?? product.Description;
                product.Stock = productUpdateModel.Stock ?? product.Stock;
                if (productUpdateModel.CategoryId != null && productUpdateModel.CategoryId.Count > 0)
                {
                    var categories = await _uow.CategoryRepository.GetEntitiesPredicate(x => productUpdateModel.CategoryId.Contains(x.Id)).ToListAsync();
                    product.Categories.Clear();
                    product.Categories = categories;
                }
                if (productUpdateModel.ImageUrl != null && productUpdateModel.ImageUrl.Count > 0)
                {
                    var productImages = _uow.ProductImageRepository.GetEntitiesPredicate(x => x.ProductId.Equals(id));
                    _uow.ProductImageRepository.RemoveRange(productImages);
                    foreach (var item in productUpdateModel.ImageUrl)
                    {
                        product.ProductImages.Add(new ProductImage
                        {
                            Id = Guid.NewGuid(),
                            Url = item,
                            ProductId = id
                        });
                    }
                }
                return await _uow.SaveChangesAsync() > 0 ? new JsonResult(await GetProduct(id)) : new StatusCodeResult(500);

            }
            return new StatusCodeResult(400);
        }

    }
}
