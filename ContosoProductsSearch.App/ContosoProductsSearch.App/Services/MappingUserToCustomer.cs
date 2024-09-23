namespace ContosoProductsSearch.App.Services;

public class MappingUserToCustomer
{
    private Dictionary<string, string> customers = new();

    public MappingUserToCustomer()
    {
        customers.Add("m.tancorre@cotrap.it", "VINET");    
    }

    public string? GetCustomerName(string email)
    {
        return customers[email];
    }
}
