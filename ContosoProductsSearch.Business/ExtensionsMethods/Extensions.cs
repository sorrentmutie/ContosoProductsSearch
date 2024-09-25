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
                    CssDiscountClass = prezzoProdotti.GetSconto(p.Scorte).FromDecimalToCssClass(),
                    Sconto = prezzoProdotti.GetSconto(p.Scorte).FromDecimalToDiscount()
                })
            })
        };

        return vm;
    }


    static public CategorieVM? ToCategorieVM(this IEnumerable<CategoriaPerCliente>? categorieDbo, IPrezzoProdotti prezzoProdotti, bool isBirthday)
    {
        var vm = new CategorieVM
        {
            Categorie = categorieDbo?.Select(c => new CategoriaVM
            {
                Id = c.IdCategoria,
                Nome = c.NomeCategoria,
                Prodotti = c.Prodotti?.Select(p => new ProdottoVM
                {
                    NomeProdotto = p.Nome,
                    PrezzoOriginario = p.PrezzoUnitario,
                    CssDiscountClass = prezzoProdotti.GetSconto(p.Scorte).FromDecimalToCssClass(),
                    Scorte = p.Scorte,
                    Sconto = isBirthday ? (prezzoProdotti.GetSconto(p.Scorte)+0.1m).FromDecimalToDiscount() : prezzoProdotti.GetSconto(p.Scorte).FromDecimalToDiscount()
                })
            })
        };

        var categorieList = vm.Categorie?.ToList();
        
        if (categorieList is not null)
        {
            foreach (var categoria in categorieList)
            {
                var prodottiList = categoria?.Prodotti?.ToList();
                if (prodottiList is not null)
                {
                    foreach (var prodotto in prodottiList)
                    {
                        var sconto = isBirthday ? (prezzoProdotti.GetSconto(prodotto.Scorte) + 0.1m) : prezzoProdotti.GetSconto(prodotto.Scorte);
                        prodotto.PrezzoScontato = prodotto.PrezzoOriginario * (1 - sconto);
                    }
                }

                categoria!.Prodotti = prodottiList;
            }
        }

        
        vm.Categorie = categorieList;
       


        return vm;
    }
}
