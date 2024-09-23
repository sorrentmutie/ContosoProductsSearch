using ContosoProductsSearch.Core.DTO;
using ContosoProductsSearch.Core.Interfaces;
using ContosoProductsSearch.Core.Interfaces.Business;
using ContosoProductsSearch.Core.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ContosoProductsSearch.Business.Services;

public class CategorieBusinessService : ICategorieBusiness
{
    private readonly ICategorie categorieService;
    private readonly IMemoryCache cache;

    public CategorieBusinessService(ICategorie categorieService, IMemoryCache cache)
    {
        this.categorieService = categorieService;
        this.cache = cache;
    }
    public async Task<IEnumerable<CategoriaDTO>?> GetCategorieConsigliate(string IdCliente)
    {
        var data = cache.Get<IEnumerable<CategoriaDTO>?>(IdCliente);

        if(data != null)
        {
            return data;
        } else
        {
            var categorie = await categorieService.GetCategorieConMaggioriScorte();
            var categorieDbo = await categorieService.GetCategorieAsync(IdCliente);
            if(categorie is not null && categorieDbo is not null)
            {
                foreach (var c in categorie)
                {
                    if (!categorieDbo.Any(x => x.IdCategoria == c.IdCategoria))
                    {
                        categorieDbo = categorieDbo.Append(c);
                    }
                }
            }

            var dataCategories = categorieDbo?
                .OrderByDescending(x => x.Scorte)
                .Select(x => new CategoriaDTO { Id = x.IdCategoria, Nome = x.NomeCategoria });

            cache.Set(IdCliente, dataCategories, TimeSpan.FromMinutes(5));

            return dataCategories;
        }
    }
}
