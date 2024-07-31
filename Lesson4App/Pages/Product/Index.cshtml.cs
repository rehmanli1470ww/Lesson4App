using Lesson4App.Data;
using Lesson4App.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lesson4App.Pages.Product
{
    public class IndexModel : PageModel
    {
        private readonly ProductDBContext _context;

        public IndexModel(ProductDBContext context)
        {
            _context = context;
        }

        public List<Entities.Product> Products { get; set; }
        public string Info { get; set; }

        [BindProperty]
        public Entities.Product Product { get; set; }

        public void OnGet(string info = "", int? id = null)
        {
            Products = _context.Products.ToList();
            Info = info;

            if (id.HasValue)
            {   
                Product = _context.Products.Find(id);
            }
            else
            {
                Product = new Entities.Product();
            }
        }

        public IActionResult OnPost()
        {
            if (Product.Id == 0)
            {
                if (Product != null)
                {
                    _context.Products.Add(Product);
                    _context.SaveChanges();
                    Info = $"{Product.Name} was successfully added";
                }
                else
                {
                    Info = "Data is empty";
                }
            }
            else
            {
                UpdateProduct(Product);
            }

            return RedirectToPage("Index", new { info = Info });
        }

        public IActionResult OnPostDelete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                Info = $"{product.Name} was successfully deleted";
            }
            else
            {
                Info = "Product not found";
            }
            return RedirectToPage("Index", new { info = Info });
        }

        private void UpdateProduct(Entities.Product updatedProduct)
        {
            var existingProduct = _context.Products.Find(updatedProduct.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = updatedProduct.Name;
                existingProduct.Price = updatedProduct.Price;
                _context.SaveChanges();
                Info = $"{updatedProduct.Name} was successfully updated";
            }
            else
            {
                Info = "Product not found";
            }
        }
    }
}
