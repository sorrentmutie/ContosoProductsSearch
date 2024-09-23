using ContosoProductsSearch.Core.DTO;

namespace ContosoProductsSearch.Core.Interfaces.Business;

public interface ICategorieBusiness
{
    Task<IEnumerable<CategoriaDTO>?> GetCategorieConsigliate(string IdCliente);
}
