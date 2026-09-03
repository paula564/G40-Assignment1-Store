
using Microsoft.AspNetCore.Mvc;
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
        return View();
    }

    [Route("Create")]
    // POST: PRODUCTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProductId,ProdCatId,Description,Manufacturer,Stock,BuyPrice,SellPrice,ProdCat")] Product product)
    {
        if (ModelState.IsValid)
        {
            product.Create(_context, product);
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // GET: PRODUCTS/Edit/5
    public async Task<IActionResult> Edit(int? productid)
    {
        if (productid == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(productid);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // POST: PRODUCTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? productid, [Bind("ProductId,ProdCatId,Description,Manufacturer,Stock,BuyPrice,SellPrice,ProdCat")] Product product)
    {
        if (productid != product.ProductId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // GET: PRODUCTS/Delete/5
    public async Task<IActionResult> Delete(int? productid)
    {
        if (productid == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.ProductId == productid);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
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
}
