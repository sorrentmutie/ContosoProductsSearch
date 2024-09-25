using ContosoProductsSearch.Core.DTO;

namespace ContosoProductsSearch.Core.ViewModel;

public class ProdottoVM
{
    public string? Sconto { get; set; }
    public string? CssDiscountClass { get; set; }

    public string? NomeProdotto { get; set; }

    public decimal? PrezzoOriginario { get; set; }

    public decimal? PrezzoScontato { get; set; }

    public int Scorte { get; set; }
}
