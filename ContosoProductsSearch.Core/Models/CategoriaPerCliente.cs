using ContosoProductsSearch.Core.DTO;

namespace ContosoProductsSearch.Core.Models;

public class CategoriaPerCliente
{
    public int IdCategoria { get; set; }
    public string? NomeCategoria { get; set; }
    public int NumeroProdotti { get; set; }
    public int Scorte { get; set; }
    public IEnumerable<ProdottoDTO>? Prodotti{ get; set; }
}
