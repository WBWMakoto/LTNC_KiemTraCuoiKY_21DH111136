using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportProductApp.Data;
using SportProductApp.Models;

namespace SportProductApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly SportProductContext _context;

        public ProductController(SportProductContext context)
        {
            _context = context;
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

            return View(product);
        }

        // POST: Product/Edit - Xử lý cập nhật sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction(nameof(Details));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật sản phẩm. Vui lòng thử lại.");
                }
            }

            return View(product);
        }
    }
}