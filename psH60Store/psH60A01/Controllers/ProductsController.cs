
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
    // GET: PRODUCTS
    public async Task<IActionResult> Index()    
    {
        return View(ProductCategory.GetAllProductCategories(_context));
    }

    [Route("All")]
    public async Task<IActionResult> AllProducts()
    {
        return View(Product.GetAllProducts(_context));
    }

    [Route("Details/{productid:int}")]
    // GET: PRODUCTS/Details/5
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
    // GET: PRODUCTS/Create
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
        if (ModelState.IsValid)
        {
            product.ProductId = Product.currentId;
            product.Create(_context, product);
            Product.currentId++;
            return RedirectToAction(nameof(AllProducts));
        }
        return View(product);
    }

    // GET: PRODUCTS/Edit/5
    [Route("EditProduct")]
    public async Task<IActionResult> Edit(int? productId)
    {
        var product = Product.GetProductById(_context, (int)productId);
        ViewBag.Categories = new SelectList(_context.ProductCategories, "CategoryId", "ProdCat");
        return View(product);
    }

    // POST: PRODUCTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("EditProduct")]
    public async Task<IActionResult> Edit(int? id, [Bind("ProdCatId", "Description", "Manufacturer")] Product product)
    {
        ModelState.Remove(nameof(product.ProdCat));

        if (ModelState.IsValid)
        {
            product.ProductId = (int)id;
            product.Update(_context, product);
            return RedirectToAction(nameof(AllProducts));
        }
        return View(product);
    }

    
    [Route("DeleteProduct")]
    [HttpDelete("{productId}")]
    public async Task<IActionResult> Delete(int? productId)
    {
        if (productId == null)
        {
            return NotFound();
        }

        var product = Product.GetProductById(_context, (int)productId);

        if (product != null)
        {
            product.Delete(_context, product.ProductId);
        }

        return RedirectToAction(nameof(AllProducts));
    }

    // POST: PRODUCTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? productid)
    {
        var product = await _context.Products.FindAsync(productid);
        if (product != null)
        {
            _context.Products.Remove(product);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
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
    public async Task<IActionResult> EditStock(int? productid, int adjustment)
    {
        var product = Product.GetProductById(_context, (int)productid);

        if (adjustment > product.Stock)
        {
            throw new ArgumentOutOfRangeException(nameof(adjustment));
        }

        product.Stock += adjustment;

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
            throw ArithmeticException(nameof(newPrice));
        }

        if (newPrice < 0)
        {
            throw InvalidOperationException(nameof(newPrice));
        }

        if (newPrice > product.SellPrice)
        {
            throw InvalidOperationException(nameof(newPrice));
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
            throw ArithmeticException(nameof(newPrice));
        }

        if (newPrice < 0)
        {
            throw InvalidOperationException(nameof(newPrice));
        }

        if (newPrice < product.BuyPrice)
        {
            throw InvalidOperationException(nameof(newPrice));
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

        product.BuyPrice = newPrice;

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
    // GET: PRODUCTS/Create
    public IActionResult CreateCategory()
    {
        return View();
    }

    [Route("CreateCategory")]
    // POST: PRODUCTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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


    [Route("DeleteCategory")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int? id)
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

    private Exception InvalidOperationException(string v)
    {
        throw new NotImplementedException();
    }

    private Exception ArithmeticException(string v)
    {
        throw new NotImplementedException();
    }
}
