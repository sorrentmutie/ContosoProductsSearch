using ContosoProductsSearch.Core.DTO;

namespace ContosoProductsSearch.Core.ViewModel;

public class CategoriaVM
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public IEnumerable<ProdottoVM>? Prodotti { get; set; }

}
