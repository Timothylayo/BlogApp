using Microsoft.AspNetCore.Mvc;
using BlogAppSharedProject.Models;
using BlogApp.Repositories;

namespace BloggingApp.Controller
{
    [Route("api/[controller]")]
	[ApiController]
	public class CategoryController(ICategoryRepository categoryRepository) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<Category[]>> GetAllCategories()
		{
			var categories = await categoryRepository.GetCategoryAsync();
			return Ok(categories);
		}

		

		[HttpGet("bySlug")]
		public async Task<ActionResult<Category?>> GetCategoryBySlug(string Slug)
		{
			var categoriesBySlug = await categoryRepository.GetCategoryBySlugAsync(Slug);
			return Ok(categoriesBySlug);
		} 
	}
}
