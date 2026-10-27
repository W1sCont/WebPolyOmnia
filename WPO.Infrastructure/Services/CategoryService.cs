using AutoMapper;
using WPO.Application.DTO;
using WPO.Application.Exceptions;
using WPO.Application.Interfaces;
using WPO.Domain;

namespace WPO.Infrastructure.Services
{
    public class CategoryService(IUnitOfWork db, IMapper mapper) : ICategoryService
    {
        public async Task CreateCategory(CategoryDTO categoryDTO)
        {
            var category = mapper.Map<CategoryDTO, Category>(categoryDTO);
            await db.Category.AddAsync(category);
            await db.Save();
        }
        public async Task UpdateCategory(CategoryDTO categoryDTO)
        {
            var category = mapper.Map<CategoryDTO, Category>(categoryDTO);
            await db.Category.UpdateAsync(category);
            await db.Save();
        }
        public async Task DeleteCategory(CategoryDTO categoryDTO)
        {
            var category = await db.Category.GetByIdAsync(categoryDTO.Id);
            if (category == null)
            {
                throw new ValidationException($"Category with id not found", nameof(categoryDTO.Id));
            }
            await db.Category.DeleteAsync(category);
            await db.Save();
        }
        public async Task<CategoryDTO?> GetCategoryById(int categoryId)
        {
            var category = await db.Category.GetByIdAsync(categoryId);
            if (category == null)
                throw new ValidationException("Category with id {categoryId} not found", nameof(categoryId));
            return mapper.Map<Category, CategoryDTO>(category);
        }
        public async Task<CategoryDTO?> GetCategoryByName(string name)
        {
            var category = await db.Category.GetByNameAsync(name);
            if (category == null)
                throw new ValidationException("Category with name not found", nameof(name));
            return mapper.Map<Category, CategoryDTO>(category);
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            var categories = await db.Category.GetAllAsync();
            return mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(categories);
        }
        public async Task<CategoryDTO?> GetCategoryWithVideos(int categoryId)
        {
            var categories = await db.Category.GetCategoryWithVideosAsync(categoryId);
            if (categories == null)
                throw new ValidationException("Category with id not found", nameof(categoryId));

            return mapper.Map<Category, CategoryDTO>(categories);
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllWithVideoCount()
        {
            var categories = await db.Category.GetAllWithVideoCountAsync();
            return mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(categories);
        }
    }
}
