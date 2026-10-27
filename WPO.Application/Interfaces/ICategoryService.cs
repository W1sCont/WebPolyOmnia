using System;
using System.Collections.Generic;
using System.Text;
using WPO.Application.DTO;

namespace WPO.Application.Interfaces
{
    public interface ICategoryService
    {
        Task CreateCategory(CategoryDTO categoryDTO);
        Task UpdateCategory(CategoryDTO categoryDTO);
        Task DeleteCategory(CategoryDTO categoryDTO);
        Task<CategoryDTO?> GetCategoryById(int categoryId);
        Task<CategoryDTO?> GetCategoryByName(string name);
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
        Task<CategoryDTO?> GetCategoryWithVideos(int categoryId);
        Task<IEnumerable<CategoryDTO>> GetAllWithVideoCount();
    }
}
