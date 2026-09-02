using System;
using System.Collections.Generic;

namespace psH60A01.Models;

public partial class Product
{

  
    public int ProductId { get; set; }

    public int ProdCatId { get; set; }

    public string? Description { get; set; }

    public string? Manufacturer { get; set; }

    public int Stock { get; set; }

    public decimal? BuyPrice { get; set; }

    public decimal? SellPrice { get; set; }

    public virtual ProductCategory ProdCat { get; set; } = null!;

    public static List<Product> GetAllProducts(H60AssignmentDbPsContext context)
    {
        return context.Products.ToList();
    }

    public static Product GetProductById(H60AssignmentDbPsContext context, int id)
    {
       return context.Products.FirstOrDefault(x => x.ProductId == id);

    }

    public void Create(H60AssignmentDbPsContext context, Product product)
    {
        context.Products.Add(product);
        context.SaveChanges();
    }

    public void Update(H60AssignmentDbPsContext context, Product product)
    {
        context.Products.Update(product);
        context.SaveChanges();
    }

    public void Delete(H60AssignmentDbPsContext context, int id)
    {
        var product = context.Products.FirstOrDefault(x => x.ProductId == id);
        context.Products.Remove(product);
        context.SaveChanges();
    }
}
