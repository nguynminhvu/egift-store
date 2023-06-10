using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class CategoryService : ICategoryService
    {
        private IMapper _mapper;
        private IUnitIOfWork _uow;

        public CategoryService(IUnitIOfWork unitIOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _uow = unitIOfWork;
        }
        public async Task<CategoryViewModel> CreateCategory(string name)
        {
            Persistence.Entities.Category category = new Persistence.Entities.Category
            {
                Id = Guid.NewGuid(),
                Name = name,
                Status = "Active"
            };
            await _uow.CategoryRepository.AddAsync(category);
            return await _uow.SaveChangesAsync() > 0 ? _mapper.Map<CategoryViewModel>(category) : null!;
        }

        public async Task<IActionResult> GetCategories(string? name)
        {
            var query = _uow.CategoryRepository.GetAll();
            if (name != null)
            {
                query= query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            var categories = await query.ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return new JsonResult(categories);
        }

        public async Task<CategoryViewModel> GetCategory(Guid id)
        {
            var category = await _uow.CategoryRepository.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (category != null)
            {
                return _mapper.Map<CategoryViewModel>(category);
            }
            return null!;
        }

        public async Task<IActionResult> RemoveCategory(Guid id)
        {
            var category = await _uow.CategoryRepository.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (category != null)
            {
                category.Status = "InActive";
                return await _uow.SaveChangesAsync() > 0 ? new StatusCodeResult(204) : new StatusCodeResult(500);
            }
            return new StatusCodeResult(400);
        }

        public async Task<CategoryViewModel> UpdateCategory(Guid id, string? name)
        {
            var category = await _uow.CategoryRepository.FirstOrDefaultAsync(x => x.Id.Equals(id));
            category.Name = name ?? category.Name;
            await _uow.SaveChangesAsync();
            return await GetCategory(id);
        }


    }
}
