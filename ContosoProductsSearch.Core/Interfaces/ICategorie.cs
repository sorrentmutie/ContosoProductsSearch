using ContosoProductsSearch.Core.DTO;
using ContosoProductsSearch.Core.Models;

namespace ContosoProductsSearch.Core.Interfaces;

public interface ICategorie
{
    Task<IEnumerable<CategoriaPerCliente>?> GetCategorieAsync(string IdCliente);

    Task<IEnumerable<CategoriaPerCliente>?> GetCategorieConMaggioriScorte();
}
