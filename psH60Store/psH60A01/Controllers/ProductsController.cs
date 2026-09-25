
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using psH60A01.Models;

[Route("Product/")]
public class ProductsController : Controller
{
    private readonly H60AssignmentDbPsContext _context;

    public ProductsController(H60AssignmentDbPsContext context)
    {
        _context = context;
    }

    [Route("Index")]
    public async Task<IActionResult> Index()    
    {
        return View(ProductCategory.GetAllProductCategories(_context));
    }

    [Route("All")]
    public async Task<IActionResult> AllProducts()
    {
        return View(Product.GetAllProducts(_context));
    }

    [Route("ProductsList")]
    public async Task<IActionResult> ProductsList(string search)
    {
        ViewData["CurrentFilter"] = search;

        
        if (string.IsNullOrEmpty(search))
        {
            var products = Product.GetAllProducts(_context);
            return View("ProductsList", products); 
        }
        else
        {
            var products = Product.GetFilteredProducts(_context, search);
            return View("ProductsList", products);
        }


    }

    [Route("Details/{productid:int}")]
    public async Task<IActionResult> Details(int? productid)
    {
        if (productid == null)
        {
            return NotFound();
        }

        var product = Product.GetProductById(_context, (int)productid);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [Route("Create")]
    public IActionResult Create()
    {
        
        ViewBag.Categories = new SelectList(_context.ProductCategories, "CategoryId", "ProdCat");

        return View();
    }

    [Route("Create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProdCatId,Description,Manufacturer,Stock,BuyPrice,SellPrice")] Product product)
    {
        ModelState.Remove(nameof(product.ProdCat));

        if (product.BuyPrice is not decimal)
        {
            throw ArithmeticException("The buy price cannot be non-numeric.");
        }

        if (product.BuyPrice < 0)
        {
            throw InvalidOperationException("The buy price cannot be less than 0.");
        }

        if (product.SellPrice is not decimal)
        {
            throw ArithmeticException("The sell price cannot be non-numeric.");
        }

        if (product.SellPrice < 0)
        {
            throw InvalidOperationException("The sell price cannot be less than 0.");
        }

        if (product.BuyPrice > product.SellPrice)
        {
            throw InvalidOperationException("The buy price cannot be greater than the sell price.");
        }


        int decimalCountBuyPrice = BitConverter.GetBytes(decimal.GetBits((decimal)product.BuyPrice)[3])[2];

        if (decimalCountBuyPrice > 2)
        {
            product.BuyPrice = Math.Round((decimal)product.BuyPrice, 2);
        }

        else if (decimalCountBuyPrice < 2)
        {
            product.BuyPrice = Math.Round((decimal)product.BuyPrice, 2, MidpointRounding.AwayFromZero);
        }

        int decimalCountSellPrice = BitConverter.GetBytes(decimal.GetBits((decimal)product.SellPrice)[3])[2];

        if (decimalCountSellPrice > 2)
        {
            product.SellPrice = Math.Round((decimal)product.SellPrice, 2);
        }

        else if (decimalCountSellPrice < 2)
        {
            product.SellPrice = Math.Round((decimal)product.SellPrice, 2, MidpointRounding.AwayFromZero);
        }

        if (ModelState.IsValid)
        {
            product.ProductId = Product.currentId;
            product.Create(_context, product);
            Product.currentId++;
            return RedirectToAction(nameof(AllProducts));
        }
        return View(product);
    }


    [Route("EditProduct/{id}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        
        var product = Product.GetProductById(_context, (int)id);

        if (product == null)
        {
            return NotFound();
        }

        ViewBag.Categories = new SelectList(_context.ProductCategories, "CategoryId", "ProdCat");
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("EditProduct/{id}")]
    public async Task<IActionResult> Edit(int? id, [Bind("ProductId,ProdCatId,Description,Manufacturer,Stock,SellPrice,BuyPrice")] Product product)
    {
        if (id == null)
        {
            return NotFound();
        }

        ModelState.Remove(nameof(product.ProdCat));

        if (ModelState.IsValid)
        {
            
            product.ProductId = (int)id;
            product.Update(_context, product);
            return RedirectToAction(nameof(ProductsList));
        }

       
        ViewBag.Categories = new SelectList(_context.ProductCategories, "CategoryId", "ProdCat", product.ProdCatId);
        return View(product);
    }


    [Route("DeleteProduct")]
    [HttpGet]
    public async Task<IActionResult> Delete(int? productId)
    {
        if (productId == null)
        {
            return NotFound();
        }

        var product = Product.GetProductById(_context, (int)productId);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }


 
    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var product = Product.GetProductById(_context, (int)id);
        if (product != null)
        {
            product.Delete(_context, product.ProductId);
        }

        
        return RedirectToAction("AllProducts");
    }

    [Route("DeleteCategory/{id?}")]
    [HttpGet]
    public async Task<IActionResult> DeleteCategory(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = ProductCategory.GetProductCategoryById(_context, (int)id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }



    [HttpPost]
    [Route("DeleteCategory/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmedCategory(int? id)
    {
      
            var category = ProductCategory.GetProductCategoryById(_context, (int)id);
            if (category != null)
            {

                if (category.Products != null && category.Products.Count > 0)
                {

                    foreach (var product in category.Products.ToList())
                    {
                        product.Delete(_context, product.ProductId);
                    }
                }

                category.Delete(_context, (int)id);

            }

            return RedirectToAction(nameof(AllCategories));
        
    }

    private bool ProductExists(int? productid)
    {
        return _context.Products.Any(e => e.ProductId == productid);
    }

    [Route("EditStock")]
    [HttpGet]
    public async Task<IActionResult> EditStock(int? productid) {
        var product = Product.GetProductById(_context, (int)productid);
        return View(product);
    }

    [Route("EditStock")]
    [HttpPost]
    public async Task<IActionResult> EditStock(int? productid, int? adjustment)
    {
        var product = Product.GetProductById(_context, (int)productid);

        if (adjustment == null || adjustment is int)
        {
            throw new ArgumentNullException(nameof(adjustment));
        }
        

        if (product.Stock + adjustment < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(adjustment));
        }

        product.Stock += (int)adjustment;

        product.Update(_context, product);


        return RedirectToAction("AllProducts", "Products");
    }

    [Route("UpdateBuyPrice")]
    [HttpGet]
    public async Task<IActionResult> UpdateBuyPrice(int? productid)
    {
        var product = Product.GetProductById(_context, (int)productid);
        return View(product);
    }

    [Route("UpdateBuyPrice")]
    [HttpPost]
    public async Task<IActionResult> UpdateBuyPrice(int? productid, decimal? newPrice)
    {

        var product = Product.GetProductById(_context, (int)productid);


        if (newPrice is not decimal)
        {
            throw ArithmeticException("The new price cannot be non-numeric.");
        }

        if (newPrice < 0)
        {
            throw InvalidOperationException("The new price cannot be less than 0.");
        }

        if (newPrice > product.SellPrice)
        {
            throw InvalidOperationException("The new buy price cannot be more than the current buy price.");
        }

        
        int decimalCount = BitConverter.GetBytes(decimal.GetBits((decimal)newPrice)[3])[2];

        if (decimalCount > 2)
        {
            newPrice = Math.Round((decimal)newPrice, 2);
        }

        else if (decimalCount < 2) {
            newPrice = Math.Round((decimal)newPrice, 2, MidpointRounding.AwayFromZero);
        }

        product.BuyPrice = newPrice;

        product.Update(_context, product);


        return RedirectToAction("AllProducts", "Products");
    }

    [Route("UpdateSellPrice")]
    [HttpGet]
    public async Task<IActionResult> UpdateSellPrice(int? productid)
    {
        var product = Product.GetProductById(_context, (int)productid);
        return View(product);
    }

    [Route("UpdateSellPrice")]
    [HttpPost]
    public async Task<IActionResult> UpdateSellPrice(int? productid, decimal? newPrice)
    {

        var product = Product.GetProductById(_context, (int)productid);


        if (newPrice is not decimal)
        {
            throw ArithmeticException("The new price cannot be non-numeric.");
        }

        if (newPrice < 0)
        {
            throw InvalidOperationException("The new price cannot be less than 0.");
        }

        if (newPrice < product.BuyPrice)
        {
            throw InvalidOperationException("The new sell price cannot be less than the current buy price.");
        }


        int decimalCount = BitConverter.GetBytes(decimal.GetBits((decimal)newPrice)[3])[2];

        if (decimalCount > 2)
        {
            newPrice = Math.Round((decimal)newPrice, 2);
        }

        else if (decimalCount < 2)
        {
            newPrice = Math.Round((decimal)newPrice, 2, MidpointRounding.AwayFromZero);
        }

        product.SellPrice = newPrice;

        product.Update(_context, product);


        return RedirectToAction("AllProducts", "Products");
    }

    [HttpGet]
    [Route("Categories")]

    public IActionResult AllCategories()
    {
        return View(ProductCategory.GetAllProductCategories(_context));
    }

    [Route("ProductsByCategory")]
    public IActionResult ProductsByCategory(int id)
    {
        var products = Product.GetAllProducts(_context).Where(p => p.ProdCatId == id).ToList();
        return View(products);
    }

    [Route("CreateCategory")]
    public IActionResult CreateCategory()
    {
        return View();
    }

    [Route("CreateCategory")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory([Bind("ProdCat")] ProductCategory category)
    {
        ModelState.Remove(nameof(category.Products));
        if (ModelState.IsValid)
        {
            category.CategoryId = ProductCategory.currentId;
            category.Create(_context, category);
            ProductCategory.currentId++;
            return RedirectToAction(nameof(AllCategories));
        }
        return View(category);
    }

    [Route("EditCategory")]
    [HttpGet]
    public async Task<IActionResult> EditCategory(int? id)
    {
        var category = ProductCategory.GetProductCategoryById(_context, (int)id);
        return View(category);
    }

    [Route("EditCategory")]
    [HttpPost]
    public async Task<IActionResult> EditCategory(int id, [Bind("ProdCat")] ProductCategory category)
    {

        if (ModelState.IsValid)
        {
           category.CategoryId = id;
           category.Update(_context, category);
           return RedirectToAction(nameof(AllCategories));
        }

        else { return View(category); }
        
    }



  

    private Exception InvalidOperationException(string v)
    {
        return new InvalidOperationException(v);
    }

    private Exception ArithmeticException(string v)
    {
        return new ArithmeticException(v);
    }
}
