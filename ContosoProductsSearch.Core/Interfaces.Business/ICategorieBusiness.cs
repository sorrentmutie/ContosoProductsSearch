using ContosoProductsSearch.Core.ViewModel;

namespace ContosoProductsSearch.Core.Interfaces.Business;

public interface ICategorieBusiness
{
    Task<CategorieVM?> GetCategorieVM(string IdCliente);
}
