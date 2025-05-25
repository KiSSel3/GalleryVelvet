using GalleryVelvet.BLL.Enums;
using GalleryVelvet.BLL.Services.Interfaces;
using GalleryVelvet.Presentation.Models.Common;
using GalleryVelvet.Presentation.Models.Product;
using Microsoft.AspNetCore.Mvc;

namespace GalleryVelvet.Presentation.Controllers;


[Route("Product")]
public sealed class ProductController(IProductService productService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(
        int pageNumber = 1,
        int pageSize = 6,
        Guid? categoryId = null,
        string? search = null,
        ProductSortOrder sortOrder = ProductSortOrder.None,
        bool onlyDiscounted = false,
        Guid? tagId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var pagedProducts = await productService.GetPagedProductsAsync(
                pageNumber, 
                pageSize, 
                categoryId, 
                search, 
                sortOrder,
                onlyDiscounted,
                tagId,
                cancellationToken);

            var viewModel = new ProductCatalogViewModel()
            {
                Products = pagedProducts,
                SelectedCategoryId = categoryId,
                SearchQuery = search,
                SortOrder = sortOrder,
                OnlyDiscounted = onlyDiscounted,
                SelectedTagId = tagId,
            };
        
            return View(viewModel);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel() { RequestId = ex.Message });
        }
    }
    
    [HttpGet("Details/{id:guid}")]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await productService.GetByIdAsync(id, cancellationToken);

            if (product == null)
            {
                return NotFound();
            }
        
            return View(product);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel() { RequestId = ex.Message });
        }
    }
}