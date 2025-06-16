using GalleryVelvet.BLL.DTOs.Admin;
using GalleryVelvet.BLL.Services.Interfaces;
using GalleryVelvet.Presentation.Models.Admin;
using GalleryVelvet.Presentation.Models.Common;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GalleryVelvet.Presentation.Controllers;

[Authorize(Roles = "Admin")]
[Route("Admin")]
public sealed class AdminController(IAdminService adminService) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    #region Управление заказами

    [HttpGet("Orders")]
    public async Task<IActionResult> Orders(
        int pageNumber = 1,
        int pageSize = 10,
        string? searchTerm = null,
        Guid? statusId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var orders = await adminService.GetPagedOrdersAsync(
                pageNumber, pageSize, searchTerm, statusId, fromDate, toDate, cancellationToken);

            var statuses = await adminService.GetAllOrderStatusesAsync(cancellationToken);

            var viewModel = new OrdersAdminViewModel
            {
                Orders = orders,
                OrderStatuses = statuses,
                SearchTerm = searchTerm,
                SelectedStatusId = statusId,
                FromDate = fromDate,
                ToDate = toDate
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [HttpGet("Orders/{id:guid}")]
    public async Task<IActionResult> OrderDetails(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await adminService.GetOrderDetailsAsync(id, cancellationToken);
            if (order == null) return NotFound();

            var statuses = await adminService.GetAllOrderStatusesAsync(cancellationToken);

            var viewModel = new OrderDetailsAdminViewModel
            {
                Order = order,
                OrderStatuses = statuses
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [HttpPost("Orders/{id:guid}/UpdateStatus")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, UpdateOrderStatusDto dto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var success = await adminService.UpdateOrderStatusAsync(id, dto.StatusId, cancellationToken);

            if (success)
            {
                TempData["Success"] = "Статус заказа успешно обновлен";
            }
            else
            {
                TempData["Error"] = "Не удалось обновить статус заказа";
            }

            return RedirectToAction("OrderDetails", new { id });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("OrderDetails", new { id });
        }
    }

    #endregion

    #region Управление пользователями

    [HttpGet("Users")]
    public async Task<IActionResult> Users(
        int pageNumber = 1,
        int pageSize = 10,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await adminService.GetPagedUsersAsync(pageNumber, pageSize, searchTerm, cancellationToken);

            var viewModel = new UsersAdminViewModel
            {
                Users = users,
                SearchTerm = searchTerm
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [HttpGet("Users/{id:guid}")]
    public async Task<IActionResult> UserDetails(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await adminService.GetUserDetailsAsync(id, cancellationToken);
            if (user == null) return NotFound();

            var roles = await adminService.GetAllRolesAsync(cancellationToken);

            var viewModel = new UserDetailsAdminViewModel
            {
                User = user,
                AllRoles = roles,
                SelectedRoleIds = user.UserRoles.Select(ur => ur.RoleId).ToList()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [HttpPost("Users/{id:guid}/UpdateRoles")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateUserRoles(Guid id, UpdateUserRolesDto dto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var success = await adminService.UpdateUserRolesAsync(id, dto.RoleIds, cancellationToken);

            if (success)
            {
                TempData["Success"] = "Роли пользователя успешно обновлены";
            }
            else
            {
                TempData["Error"] = "Не удалось обновить роли пользователя";
            }

            return RedirectToAction("UserDetails", new { id });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("UserDetails", new { id });
        }
    }

    #endregion

    #region Управление категориями

    [HttpGet("Categories")]
    public async Task<IActionResult> Categories(
        int pageNumber = 1,
        int pageSize = 10,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var categories =
                await adminService.GetPagedCategoriesAsync(pageNumber, pageSize, searchTerm, cancellationToken);

            var viewModel = new CategoriesAdminViewModel
            {
                Categories = categories,
                SearchTerm = searchTerm
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [HttpGet("Categories/Create")]
    public IActionResult CreateCategory()
    {
        return View();
    }

    [HttpPost("Categories/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto dto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        try
        {
            await adminService.CreateCategoryAsync(dto, cancellationToken);
            TempData["Success"] = "Категория успешно создана";
            return RedirectToAction("Categories");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
    }

    [HttpGet("Categories/{id:guid}/Edit")]
    public async Task<IActionResult> EditCategory(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var categories = await adminService.GetPagedCategoriesAsync(1, 100, cancellationToken: cancellationToken);
            var category = categories.FirstOrDefault(c => c.Id == id);

            if (category == null) return NotFound();

            var dto = category.Adapt<UpdateCategoryDto>();
            ViewData["CategoryId"] = id;
            return View(dto);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [HttpPost("Categories/{id:guid}/Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCategory(Guid id, UpdateCategoryDto dto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            ViewData["CategoryId"] = id;
            return View(dto);
        }

        try
        {
            var category = await adminService.UpdateCategoryAsync(id, dto, cancellationToken);

            if (category != null)
            {
                TempData["Success"] = "Категория успешно обновлена";
                return RedirectToAction("Categories");
            }
            else
            {
                ModelState.AddModelError("", "Категория не найдена");
                ViewData["CategoryId"] = id;
                return View(dto);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewData["CategoryId"] = id;
            return View(dto);
        }
    }

    [HttpPost("Categories/{id:guid}/Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await adminService.DeleteCategoryAsync(id, cancellationToken);
            TempData["Success"] = "Категория успешно удалена";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Categories");
    }

    #endregion

    #region Управление тегами

    [HttpGet("Tags")]
    public async Task<IActionResult> Tags(
        int pageNumber = 1,
        int pageSize = 10,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var tags = await adminService.GetPagedTagsAsync(pageNumber, pageSize, searchTerm, cancellationToken);

            var viewModel = new TagsAdminViewModel
            {
                Tags = tags,
                SearchTerm = searchTerm
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [HttpGet("Tags/Create")]
    public IActionResult CreateTag()
    {
        return View();
    }

    [HttpPost("Tags/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTag(CreateTagDto dto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        try
        {
            await adminService.CreateTagAsync(dto, cancellationToken);
            TempData["Success"] = "Тег успешно создан";
            return RedirectToAction("Tags");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
    }

    [HttpGet("Tags/{id:guid}/Edit")]
    public async Task<IActionResult> EditTag(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var tags = await adminService.GetPagedTagsAsync(1, 100, cancellationToken: cancellationToken);
            var tag = tags.FirstOrDefault(t => t.Id == id);

            if (tag == null) return NotFound();

            var dto = tag.Adapt<UpdateTagDto>();
            ViewData["TagId"] = id;
            return View(dto);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [HttpPost("Tags/{id:guid}/Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTag(Guid id, UpdateTagDto dto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            ViewData["TagId"] = id;
            return View(dto);
        }

        try
        {
            var tag = await adminService.UpdateTagAsync(id, dto, cancellationToken);

            if (tag != null)
            {
                TempData["Success"] = "Тег успешно обновлен";
                return RedirectToAction("Tags");
            }
            else
            {
                ModelState.AddModelError("", "Тег не найден");
                ViewData["TagId"] = id;
                return View(dto);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewData["TagId"] = id;
            return View(dto);
        }
    }

    [HttpPost("Tags/{id:guid}/Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTag(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await adminService.DeleteTagAsync(id, cancellationToken);
            TempData["Success"] = "Тег успешно удален";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Tags");
    }

    #endregion

    #region Управление продуктами

    [HttpGet("Products")]
    public async Task<IActionResult> Products(
        int pageNumber = 1,
        int pageSize = 10,
        string? searchTerm = null,
        Guid? categoryId = null,
        bool includeDeleted = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var products = await adminService.GetPagedProductsForAdminAsync(
                pageNumber, pageSize, searchTerm, categoryId, includeDeleted, cancellationToken);

            var categories = await adminService.GetAllCategoriesAsync(cancellationToken);

            var viewModel = new ProductsAdminViewModel
            {
                Products = products,
                Categories = categories,
                SearchTerm = searchTerm,
                SelectedCategoryId = categoryId,
                IncludeDeleted = includeDeleted
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [HttpGet("Products/Create")]
    public async Task<IActionResult> CreateProduct(CancellationToken cancellationToken = default)
    {
        await PopulateCreateProductViewData(cancellationToken);
        
        return View();
    }

    [HttpPost("Products/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(CreateProductRequestViewModel model,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCreateProductViewData(cancellationToken);
            return View(model);
        }

        try
        {
            var imagesDtos = new List<ProductImageDto>();

            if (model.Images != null && model.Images.Any())
            {
                foreach (var imageFile in model.Images)
                {
                    if (imageFile.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                        var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("Images", $"Неподдерживаемый формат файла: {extension}");
                            await PopulateCreateProductViewData(cancellationToken);
                            return View(model);
                        }
                        
                        using var memoryStream = new MemoryStream();
                        await imageFile.CopyToAsync(memoryStream, cancellationToken);
                        var imageBytes = memoryStream.ToArray();
                        var base64String = Convert.ToBase64String(imageBytes);
                        
                        var mimeType = imageFile.ContentType;
                        if (string.IsNullOrEmpty(mimeType))
                        {
                            mimeType = extension switch
                            {
                                ".jpg" or ".jpeg" => "image/jpeg",
                                ".png" => "image/png",
                                ".gif" => "image/gif",
                                ".webp" => "image/webp",
                                _ => "image/jpeg"
                            };
                        }

                        imagesDtos.Add(new ProductImageDto
                        {
                            Image = base64String,
                            Format = mimeType
                        });
                    }
                }
            }
            
            var createProductDto = new CreateProductDto
            {
                Name = model.Name,
                Description = model.Description,
                CompositionAndCare = model.CompositionAndCare,
                Price = model.Price,
                DiscountPrice = model.DiscountPrice,
                CategoryId = model.CategoryId,
                TagIds = model.TagIds ?? new List<Guid>(),
                SizeIds = model.SizeIds ?? new List<Guid>(),
                Images = imagesDtos
            };

            await adminService.CreateProductAsync(createProductDto, cancellationToken);
            TempData["Success"] = "Продукт успешно создан";
            return RedirectToAction("Products");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            await PopulateCreateProductViewData(cancellationToken);
            return View(model);
        }
    }


    [HttpPost("Products/{id:guid}/Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(Guid id, bool hardDelete = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await adminService.DeleteProductAsync(id, hardDelete, cancellationToken);
            TempData["Success"] = hardDelete ? "Продукт полностью удален" : "Продукт помечен как удаленный";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Products");
    }

    [HttpPost("Products/{id:guid}/Restore")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RestoreProduct(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await adminService.RestoreProductAsync(id, cancellationToken);
            TempData["Success"] = "Продукт восстановлен";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Products");
    }

    [HttpGet("Products/{id:guid}/Edit")]
    public async Task<IActionResult> EditProduct(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var products = await adminService.GetPagedProductsForAdminAsync(1, 1000, includeDeleted: true,
                cancellationToken: cancellationToken);
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            var categories = await adminService.GetAllCategoriesAsync(cancellationToken);
            var tags = await adminService.GetAllTagsAsync(cancellationToken);
            var sizes = await adminService.GetAllSizesAsync(cancellationToken);

            var model = new EditProductRequestViewModel
            {
                Name = product.Name,
                Description = product.Description,
                CompositionAndCare = product.CompositionAndCare,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                CategoryId = product.CategoryId,
                TagIds = product.ProductTags?.Select(pt => pt.TagId).ToList() ?? new List<Guid>(),
                SizeIds = product.ProductSizes?.Select(ps => ps.SizeId).ToList() ?? new List<Guid>(),
                ExistingImages = product.Images?.Select(img => new ExistingImageViewModel
                {
                    Id = img.Id,
                    Image = img.Image,
                    Format = img.Format
                }).ToList() ?? []
            };

            ViewBag.Categories = categories;
            ViewBag.Tags = tags;
            ViewBag.Sizes = sizes;
            ViewData["ProductId"] = id;

            return View(model);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [HttpPost("Products/{id:guid}/Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(
        Guid id,
        EditProductRequestViewModel model,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            await PopulateEditProductViewData(id, cancellationToken);
            return View(model);
        }

        try
        {
            var imagesDtos = new List<ProductImageDto>();
            
            if (model.ExistingImages != null)
            {
                var imagesToKeep = model.ExistingImages
                    .Where(img => model.ImagesToDelete == null || !model.ImagesToDelete.Contains(img.Id))
                    .ToList();

                foreach (var existingImage in imagesToKeep)
                {
                    imagesDtos.Add(new ProductImageDto
                    {
                        Image = existingImage.Image,
                        Format = existingImage.Format
                    });
                }
            }
            
            if (model.Images != null && model.Images.Any())
            {
                foreach (var imageFile in model.Images)
                {
                    if (imageFile.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                        var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("Images", $"Неподдерживаемый формат файла: {extension}");
                            await PopulateEditProductViewData(id, cancellationToken);
                            return View(model);
                        }

                        using var memoryStream = new MemoryStream();
                        await imageFile.CopyToAsync(memoryStream, cancellationToken);
                        var imageBytes = memoryStream.ToArray();
                        var base64String = Convert.ToBase64String(imageBytes);

                        var mimeType = imageFile.ContentType;
                        if (string.IsNullOrEmpty(mimeType))
                        {
                            mimeType = extension switch
                            {
                                ".jpg" or ".jpeg" => "image/jpeg",
                                ".png" => "image/png",
                                ".gif" => "image/gif",
                                ".webp" => "image/webp",
                                _ => "image/jpeg"
                            };
                        }

                        imagesDtos.Add(new ProductImageDto
                        {
                            Image = base64String,
                            Format = mimeType
                        });
                    }
                }
            }

            var updateProductDto = new UpdateProductDto
            {
                Name = model.Name,
                Description = model.Description,
                CompositionAndCare = model.CompositionAndCare,
                Price = model.Price,
                DiscountPrice = model.DiscountPrice,
                CategoryId = model.CategoryId,
                TagIds = model.TagIds ?? new List<Guid>(),
                SizeIds = model.SizeIds ?? new List<Guid>(),
                Images = imagesDtos
            };

            var updatedProduct = await adminService.UpdateProductAsync(id, updateProductDto, cancellationToken);

            if (updatedProduct != null)
            {
                TempData["Success"] = "Продукт успешно обновлен";
                return RedirectToAction("Products");
            }
            else
            {
                ModelState.AddModelError("", "Продукт не найден");
                await PopulateEditProductViewData(id, cancellationToken);
                return View(model);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            await PopulateEditProductViewData(id, cancellationToken);
            return View(model);
        }
    }

    #endregion

    private async Task PopulateCreateProductViewData(CancellationToken cancellationToken)
    {
        var categories = await adminService.GetAllCategoriesAsync(cancellationToken);
        var tags = await adminService.GetAllTagsAsync(cancellationToken);
        var sizes = await adminService.GetAllSizesAsync(cancellationToken);

        ViewBag.Categories = categories;
        ViewBag.Tags = tags;
        ViewBag.Sizes = sizes;
    }

    private async Task PopulateEditProductViewData(Guid productId, CancellationToken cancellationToken)
    {
        var categories = await adminService.GetAllCategoriesAsync(cancellationToken);
        var tags = await adminService.GetAllTagsAsync(cancellationToken);
        var sizes = await adminService.GetAllSizesAsync(cancellationToken);

        ViewBag.Categories = categories;
        ViewBag.Tags = tags;
        ViewBag.Sizes = sizes;
        ViewData["ProductId"] = productId;
    }
}