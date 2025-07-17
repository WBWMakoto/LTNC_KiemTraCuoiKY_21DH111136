using System.ComponentModel.DataAnnotations;
using SportProductApp.Models;

namespace SportProductApp.ViewModels
{
    public class ProductEditViewModel
    {
        public int ProductID { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string? NamePro { get; set; }

        [Display(Name = "Mô tả")]
        public string? DecriptionPro { get; set; }

        [Display(Name = "Loại sản phẩm")]
        [Required(ErrorMessage = "Vui lòng chọn loại sản phẩm")]
        public string? Category { get; set; }

        [Display(Name = "Giá")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal? Price { get; set; }

        [Display(Name = "Hình ảnh hiện tại")]
        public string? ImagePro { get; set; }

        [Display(Name = "Ngày sản xuất")]
        [Required(ErrorMessage = "Vui lòng chọn ngày sản xuất")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(ProductEditViewModel), nameof(ValidateManufacturingDate))]
        public DateTime ManufacturingDate { get; set; }

        [Display(Name = "Chọn cách nhập hình ảnh")]
        public string ImageOption { get; set; } = "upload"; // "upload" hoặc "url"

        [Display(Name = "Upload hình ảnh")]
        [CustomValidation(typeof(ProductEditViewModel), nameof(ValidateImageFile))]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "URL hình ảnh")]
        [Url(ErrorMessage = "Vui lòng nhập URL hợp lệ")]
        [CustomValidation(typeof(ProductEditViewModel), nameof(ValidateImageUrl))]
        public string? ImageUrl { get; set; }

        public static ValidationResult? ValidateManufacturingDate(DateTime date, ValidationContext context)
        {
            if (date >= DateTime.Now.Date)
            {
                return new ValidationResult("Ngày sản xuất phải nhỏ hơn ngày hiện tại");
            }
            return ValidationResult.Success;
        }

        public static ValidationResult? ValidateImageFile(IFormFile? file, ValidationContext context)
        {
            var instance = context.ObjectInstance as ProductEditViewModel;

            if (instance?.ImageOption == "upload" && file != null)
            {
                // Kiểm tra định dạng file
                var allowedExtensions = new[] { ".jpg", ".webp", ".jpeg", ".png", ".gif", ".bmp" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    return new ValidationResult("Chỉ chấp nhận file hình ảnh (jpg, jpeg, png, gif, bmp)");
                }

                // Kiểm tra kích thước file (5MB)
                if (file.Length > 5 * 1024 * 1024)
                {
                    return new ValidationResult("Kích thước file không được vượt quá 5MB");
                }
            }

            return ValidationResult.Success;
        }

        public static ValidationResult? ValidateImageUrl(string? url, ValidationContext context)
        {
            var instance = context.ObjectInstance as ProductEditViewModel;

            if (instance?.ImageOption == "url" && string.IsNullOrEmpty(url))
            {
                return new ValidationResult("Vui lòng nhập URL hình ảnh");
            }

            return ValidationResult.Success;
        }
    }
}