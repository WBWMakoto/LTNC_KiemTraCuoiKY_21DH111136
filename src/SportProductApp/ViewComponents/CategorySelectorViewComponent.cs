using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SportProductApp.ViewComponents
{
    public class CategorySelectorViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string selectedCategory = "")
        {
            var categories = new List<SelectListItem>
            {
                new SelectListItem { Value = "Vợt", Text = "Vợt" },
                new SelectListItem { Value = "Bóng", Text = "Bóng" },
                new SelectListItem { Value = "Cầu", Text = "Cầu" },
                new SelectListItem { Value = "Đệm", Text = "Đệm" },
                new SelectListItem { Value = "Quần áo", Text = "Quần áo" }
            };

            // Set selected value
            if (!string.IsNullOrEmpty(selectedCategory))
            {
                var selectedItem = categories.FirstOrDefault(c => c.Value == selectedCategory);
                if (selectedItem != null)
                {
                    selectedItem.Selected = true;
                }
            }

            return View(categories);
        }
    }
}