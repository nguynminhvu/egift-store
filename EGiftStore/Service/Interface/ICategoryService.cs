using Microsoft.AspNetCore.Mvc;
using Persistence.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ICategoryService
    {
        public Task<CategoryViewModel> CreateCategory(string name);
        public Task<CategoryViewModel> UpdateCategory(Guid id, string? name);
        public Task<IActionResult> RemoveCategory(Guid id);
        public Task<IActionResult> GetCategories(string? name);
        public Task<CategoryViewModel> GetCategory(Guid id);

    }
}
