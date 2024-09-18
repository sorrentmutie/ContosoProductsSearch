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
        return await context.Orders.Include(o => o.OrderDetails)
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
    }

    public async Task<IEnumerable<CategoriaPerCliente>?> GetCategorieConMaggioriScorte()
    {
        return await context.Orders.Include(o => o.OrderDetails)
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
    }
}
