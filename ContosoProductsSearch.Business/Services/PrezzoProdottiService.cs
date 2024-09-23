using ContosoProductsSearch.Business.Interfaces;

namespace ContosoProductsSearch.Business.Services;

public class PrezzoProdottiService : IPrezzoProdotti
{
    public decimal? GetSconto(int NumeroScorte)
    {
        if (NumeroScorte < 10)
        {
            return 0;
        }
        else if(NumeroScorte > 10 && NumeroScorte < 30)
        {
            return 0.1m;
        }
        if (NumeroScorte > 30 && NumeroScorte < 60)
        {
            return 0.15m;
        }
        else if(NumeroScorte > 60 && NumeroScorte < 100)
        {
            return 0.2m;
        }
        else if (NumeroScorte > 100 && NumeroScorte < 200)
        {
            return 0.3m;
        }
        else
        {
            return 0.5m;
        }
    }
}
