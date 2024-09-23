using ContosoProductsSearch.Core.DTO;

namespace ContosoProductsSearch.Core.ViewModel;

public class ProdottoVM
{
    public ProdottoDTO? Prodotto { get; set; }
    public string? Sconto { get; set; }
    public string? CssDiscountClass { get; set; }
}
