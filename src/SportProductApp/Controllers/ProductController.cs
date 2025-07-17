using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportProductApp.Data;
using SportProductApp.ViewModels;

namespace SportProductApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly SportProductContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductController(SportProductContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Product/Details - Hiển thị chi tiết sản phẩm đầu tiên
        public async Task<IActionResult> Details()
        {
            var product = await _context.Products.FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound("Không tìm thấy sản phẩm nào trong hệ thống");
            }

            return View(product);
        }

        // GET: Product/Edit - Hiển thị form chỉnh sửa sản phẩm đầu tiên
        public async Task<IActionResult> Edit()
        {
            var product = await _context.Products.FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound("Không tìm thấy sản phẩm nào trong hệ thống");
            }

            var viewModel = new ProductEditViewModel
            {
                ProductID = product.ProductID,
                NamePro = product.NamePro,
                DecriptionPro = product.DecriptionPro,
                Category = product.Category,
                Price = product.Price,
                ImagePro = product.ImagePro,
                ManufacturingDate = product.ManufacturingDate,
                ImageOption = string.IsNullOrEmpty(product.ImagePro) ? "upload" :
                             (product.ImagePro.StartsWith("http") ? "url" : "upload")
            };

            return View(viewModel);
        }

        // POST: Product/Edit - Xử lý cập nhật sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _context.Products.FindAsync(viewModel.ProductID);
                    if (product == null)
                    {
                        return NotFound();
                    }

                    // Xử lý hình ảnh
                    string imagePath = product.ImagePro; // Giữ nguyên ảnh cũ nếu không có thay đổi

                    if (viewModel.ImageOption == "upload" && viewModel.ImageFile != null)
                    {
                        // Xử lý upload file
                        imagePath = await SaveUploadedImage(viewModel.ImageFile);
                    }
                    else if (viewModel.ImageOption == "url" && !string.IsNullOrEmpty(viewModel.ImageUrl))
                    {
                        // Sử dụng URL
                        imagePath = viewModel.ImageUrl;
                    }

                    // Cập nhật thông tin sản phẩm
                    product.NamePro = viewModel.NamePro;
                    product.DecriptionPro = viewModel.DecriptionPro;
                    product.Category = viewModel.Category;
                    product.Price = viewModel.Price;
                    product.ImagePro = imagePath;
                    product.ManufacturingDate = viewModel.ManufacturingDate;

                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction(nameof(Details));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật sản phẩm. Vui lòng thử lại.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                }
            }

            return View(viewModel);
        }

        private async Task<string> SaveUploadedImage(IFormFile imageFile)
        {
            try
            {
                // Tạo thư mục uploads nếu chưa có
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // Tạo tên file unique
                var fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                var filePath = Path.Combine(uploadsPath, fileName);

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Trả về đường dẫn relative
                return $"/uploads/{fileName}";
            }
            catch (Exception)
            {
                throw new Exception("Không thể lưu file hình ảnh. Vui lòng thử lại.");
            }
        }
    }
}