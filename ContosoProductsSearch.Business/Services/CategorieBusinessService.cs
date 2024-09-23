using ContosoProductsSearch.Business.Interfaces;
using ContosoProductsSearch.Core.DTO;
using ContosoProductsSearch.Core.Interfaces;
using ContosoProductsSearch.Core.Interfaces.Business;
using ContosoProductsSearch.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using ContosoProductsSearch.Core.ExtentionsMethods;
using System.Security.Cryptography.X509Certificates;
using ContosoProductsSearch.Core.ViewModel;

namespace ContosoProductsSearch.Business.Services;

public class CategorieBusinessService : ICategorieBusiness
{
    private readonly ICategorie categorieService;
    private readonly IPrezzoProdotti prezzoProdotti;
    private readonly IMemoryCache cache;

    public CategorieBusinessService(ICategorie categorieService, IPrezzoProdotti prezzoProdotti, IMemoryCache cache)
    {
        this.categorieService = categorieService;
        this.prezzoProdotti = prezzoProdotti;
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
                .Select(x => new CategoriaDTO { Id = x.IdCategoria, Nome = x.NomeCategoria, Prodotti = x.Prodotti });

            if(dataCategories is not null)
            {
                foreach (var c in dataCategories)
                {
                    if(c.Prodotti is not null)
                    {
                        foreach (var p in c.Prodotti)
                        {
                            var sconto = prezzoProdotti.GetSconto(p.Scorte);
                            if (sconto is not null)
                            {
                                //p.CssDiscountClass = sconto.FromDecimalToCssClass();
                                //p.Sconto = sconto.FromDecimalToDiscount();
                                p.PrezzoUnitario = (1 - sconto) * p.PrezzoUnitario;
                            }
                        }
                    }
                    
                }
            }
            


            cache.Set(IdCliente, dataCategories, TimeSpan.FromMinutes(5));



            return dataCategories;
        }
    }


    public async Task<CategorieVM?> GetCategorieVM(string IdCliente)
    {

        var categorie = await categorieService.GetCategorieConMaggioriScorte();
        var categorieDbo = await categorieService.GetCategorieAsync(IdCliente);
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

        var dataCategories = categorieDbo?
            .OrderByDescending(x => x.Scorte)
            .Select(x => new CategoriaDTO { Id = x.IdCategoria, Nome = x.NomeCategoria, Prodotti = x.Prodotti });


        var vm = new CategorieVM { Categorie = dataCategories?.Select(c => new CategoriaVM { Id = c.Id, Nome = c.Nome, 
            Prodotti = c.Prodotti?.Select(p => new ProdottoVM { Prodotto = p })}) };

        if(vm.Categorie is not null)
        {
            foreach (var c in vm.Categorie)
            {
                if (c is not null && c.Prodotti is not null)
                {
                    foreach(var p in c.Prodotti)
                    {
                        if(p.Prodotto is not null)
                        {
                            var sconto = prezzoProdotti.GetSconto(p.Prodotto.Scorte);
                            if (sconto is not null)
                            {

                                p.CssDiscountClass = sconto.FromDecimalToCssClass();
                                p.Sconto = sconto.FromDecimalToDiscount();
                                p.Prodotto.PrezzoUnitario = (1 - sconto) * p.Prodotto.PrezzoUnitario;
                            }
                        }
                      
                    }
                }
            }
        }
        

       

        return vm;
    }
}
