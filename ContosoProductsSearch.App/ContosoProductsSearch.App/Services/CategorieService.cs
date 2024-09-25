using ContosoProductsSearch.Core.DTO;
using ContosoProductsSearch.Core.Interfaces;
using ContosoProductsSearch.Core.Interfaces.Business;
using ContosoProductsSearch.Core.Models;
using ContosoProductsSearch.Data.Models;
using System.Linq;

namespace ContosoProductsSearch.App.Services;

public class CategorieService : ICategorie
{
    private readonly NorthwindContext context;
    private readonly IConfiguration configuration;
    private int limiteSuProdottiConPiuScorte;

    public CategorieService(NorthwindContext context, IConfiguration configuration)
    {
        this.context = context;
        this.configuration = configuration;
        limiteSuProdottiConPiuScorte = configuration.GetValue<int>("LimiteSuProdottiConPiuScorte");
    }

    public async Task<IEnumerable<CategoriaPerCliente>?> GetCategorieAsync(string IdCliente)
    {
        var categories = await context.Orders.Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .ThenInclude(p => p.Category)
            .Where(o => o.CustomerId == IdCliente)
            .SelectMany(o => o.OrderDetails) // Estrai OrderDetails da ogni ordine
            .GroupBy(od => new { od.Product.CategoryId, od.Product.Category.CategoryName }) // Raggruppa per IDCategoria e NomeCategoria
            .Select(g => new CategoriaPerCliente()
            {
                IdCategoria = g.Key.CategoryId ?? 0,  // ID della categoria
                NomeCategoria = g.Key.CategoryName,      // Nome della categoria
                NumeroProdotti = g.Count(),               // Conteggio degli OrderDetails nel gruppo
                Scorte = g.Sum(od => od.Product.UnitsInStock) ?? 0
            })
            .Take(limiteSuProdottiConPiuScorte)
            .ToListAsync();

        foreach (var category in categories)
        {
            var products = await context.Products
                .Include(p => p.Supplier)
                .Where(p => p.CategoryId == category.IdCategoria)
                .OrderByDescending(p => p.UnitsInStock)
                .Take(limiteSuProdottiConPiuScorte)
                .Select(x => new ProdottoDTO { Id = x.ProductId, 
                    Nome = x.ProductName, 
                    NomeFornitore = x.Supplier.CompanyName, 
                    PrezzoUnitario = x.UnitPrice,
                    Scorte = x.UnitsInStock.HasValue ? x.UnitsInStock.Value : 0
                })
                .ToListAsync();

            category.Prodotti = products;
        }
        
        return categories;
    }

    public async Task<IEnumerable<CategoriaPerCliente>?> GetCategorieConMaggioriScorte()
    {
        var categories = await context.Orders.Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .ThenInclude(p => p.Category)
            .SelectMany(o => o.OrderDetails) // Estrai OrderDetails da ogni ordine
            .GroupBy(od => new { od.Product.CategoryId, od.Product.Category.CategoryName }) // Raggruppa per IDCategoria e NomeCategoria
            .Select(g => new CategoriaPerCliente()
            {
                IdCategoria = g.Key.CategoryId ?? 0,  // ID della categoria
                NomeCategoria = g.Key.CategoryName,      // Nome della categoria
                NumeroProdotti = g.Count(),               // Conteggio degli OrderDetails nel gruppo
                Scorte = g.Sum(od => od.Product.UnitsInStock) ?? 0
            })
            .Take(limiteSuProdottiConPiuScorte)
            .ToListAsync();

        foreach (var category in categories)
        {
            var products = await context.Products
                .Include(p => p.Supplier)
                .Where(p => p.CategoryId == category.IdCategoria)
                .OrderByDescending(p => p.UnitsInStock)
                .Take(limiteSuProdottiConPiuScorte)
                .Select(x => new ProdottoDTO { Id = x.ProductId, 
                    Nome = x.ProductName, 
                    NomeFornitore = x.Supplier.CompanyName, 
                    PrezzoUnitario = x.UnitPrice,
                    Scorte = x.UnitsInStock.HasValue ? x.UnitsInStock.Value : 0
                })
                .ToListAsync();

            category.Prodotti = products;
        }

        return categories;
    }

    public async Task<IEnumerable<CategoriaPerCliente>?> GetCategorieMergiate(string IdCliente)
    {
        var categorie = await GetCategorieConMaggioriScorte();
        var categorieDbo = await GetCategorieAsync(IdCliente);
        if (categorie is not null && categorieDbo is not null)
        {
            foreach (var c in categorie)
            {
                if (!categorieDbo.Any(x => x.IdCategoria == c.IdCategoria))
                {
                    categorieDbo = categorieDbo.Append(c);
                }
            }
        }

        return categorieDbo;
    }
}
