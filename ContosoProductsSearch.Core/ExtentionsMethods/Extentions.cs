namespace ContosoProductsSearch.Core.ExtentionsMethods;

public static class Extentions
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
                return "bg-info";
            }
        }

        return "bg-info";
    }
}
