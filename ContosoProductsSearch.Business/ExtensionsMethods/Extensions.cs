using ContosoProductsSearch.Business.Interfaces;
using ContosoProductsSearch.Core.Models;
using ContosoProductsSearch.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoProductsSearch.Business.ExtensionsMethods;

public static class Extensions
{
    static public string FromDecimalToDiscount(this decimal? Price)
    {
        if (Price is not null)
        {
            return ((int)(Price * 100)).ToString() + "%";
        }

        return "";
    }

    static public string FromDecimalToCssClass(this decimal? Discount)
    {
        if (Discount is not null)
        {
            if (Discount >= 0.3m)
            {
                return "bg-danger";
            }
            else
            {
                return "bg-warning";
            }
        }

        return "bg-info";
    }

    static public CategorieVM? ToCategorieVM(this IEnumerable<CategoriaPerCliente>? categorieDbo, IPrezzoProdotti prezzoProdotti)
    {
        var vm = new CategorieVM
        {
            Categorie = categorieDbo?.Select(c => new CategoriaVM
            {
                Id = c.IdCategoria,
                Nome = c.NomeCategoria,
                Prodotti = c.Prodotti?.Select(p => new ProdottoVM
                {
                    Prodotto = p,
                    CssDiscountClass = prezzoProdotti.GetSconto(p.Scorte).FromDecimalToCssClass(),
                    Sconto = prezzoProdotti.GetSconto(p.Scorte).FromDecimalToDiscount()
                })
            })
        };

        return vm;
    }
}
