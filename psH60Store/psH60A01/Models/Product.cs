using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace psH60A01.Models;

public partial class Product
{
    public static int currentId { get; set; } = 0;
  
    public int ProductId { get; set; }

    [DisplayName("Category")]
    public int ProdCatId { get; set; }
    [MaxLength(80)]
    [Required]
    public string? Description { get; set; }
    [MaxLength(80)]
    [Required]
    public string? Manufacturer { get; set; }
    [Required]
    public int Stock { get; set; }
    [Precision(8, 2)]
    [Required]
    [DisplayName("Buy price")]
    public decimal? BuyPrice { get; set; }
    [Precision(8, 2)]
    [Required]
    [DisplayName("Sell price")]
    public decimal? SellPrice { get; set; }

    
    public virtual ProductCategory ProdCat { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public static List<Product> GetAllProducts(H60AssignmentDbPsContext context)
    {
        return context.Products
                       .Include(p => p.ProdCat)
                       .OrderBy(x => x.ProdCat.ProdCat)
                       .ThenBy(x => x.Description)
                       .ToList();
    }

    public static Product GetProductById(H60AssignmentDbPsContext context, int id)
    {
       return context.Products.Include(p => p.ProdCat).FirstOrDefault(x => x.ProductId == id);

    }

    public static List<Product> GetFilteredProducts(H60AssignmentDbPsContext context, string search)
    {
        return context.Products.Where(p => p.Description.Contains(search)).ToList();
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
