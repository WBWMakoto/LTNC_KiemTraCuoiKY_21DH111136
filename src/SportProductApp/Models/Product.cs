using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportProductApp.Models
{
    public class Product
    {
        [Key]
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

        [Display(Name = "Hình ảnh")]
        public string? ImagePro { get; set; }

        [Display(Name = "Ngày sản xuất")]
        [Required(ErrorMessage = "Vui lòng chọn ngày sản xuất")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Product), nameof(ValidateManufacturingDate))]
        public DateTime ManufacturingDate { get; set; }

        public static ValidationResult? ValidateManufacturingDate(DateTime date, ValidationContext context)
        {
            if (date >= DateTime.Now.Date)
            {
                return new ValidationResult("Ngày sản xuất phải nhỏ hơn ngày hiện tại");
            }
            return ValidationResult.Success;
        }
    }
}