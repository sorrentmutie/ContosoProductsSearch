using ContosoProductsSearch.Core.DTO;
using ContosoProductsSearch.Core.Interfaces;
using ContosoProductsSearch.Core.Interfaces.Business;

namespace ContosoProductsSearch.Business.Services;

public class CategorieBusinessService : ICategorieBusiness
{
    private readonly ICategorie categorieService;

    public CategorieBusinessService(ICategorie categorieService)
    {
        this.categorieService = categorieService;
    }
    public async Task<IEnumerable<CategoriaDTO>?> GetCategorieConsigliate(string IdCliente)
    {
        var categorie = await categorieService.GetCategorieConMaggioriScorte();
        var categorieDb = await categorieService.GetCategorieAsync(IdCliente);

        if(categorie is not null && categorieDb is not null)
        {
            foreach (var c in categorie)
            {
                if(!categorieDb.Any(x => x.IdCategoria == c.IdCategoria))
                {
                    categorieDb = categorieDb.Append(c);
                }
            }
        }
      

        return categorieDb?
            .OrderByDescending(x => x.Scorte)
            .Select(x => new CategoriaDTO { Id = x.IdCategoria, Nome = x.NomeCategoria });
    }
}
