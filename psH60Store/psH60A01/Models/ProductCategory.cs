using System;
using System.Collections.Generic;

namespace psH60A01.Models;

public partial class ProductCategory
{
    public int CategoryId { get; set; }

    public string ProdCat { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public static List<ProductCategory> GetAllProductCategories(H60AssignmentDbPsContext context)
    {
        return context.ProductCategories.OrderBy(p => p.ProdCat).ToList();
    }

    
    public static ProductCategory GetProductCategoryById(H60AssignmentDbPsContext context, int id)
    {
        return context.ProductCategories.FirstOrDefault(x => x.CategoryId == id);
    }
    

    public void Create(H60AssignmentDbPsContext context, ProductCategory productCategory)
    {
        context.ProductCategories.Add(productCategory);
        context.SaveChanges();
    }

    public void Update(H60AssignmentDbPsContext context, ProductCategory productCategory)
    {
        context.ProductCategories.Update(productCategory);
        context.SaveChanges();
    }

    public void Delete(H60AssignmentDbPsContext context, int id)
    {
        var productCategory = context.ProductCategories.FirstOrDefault(x => x.CategoryId == id);
        context.ProductCategories.Remove(productCategory);
        context.SaveChanges();
    }
}
