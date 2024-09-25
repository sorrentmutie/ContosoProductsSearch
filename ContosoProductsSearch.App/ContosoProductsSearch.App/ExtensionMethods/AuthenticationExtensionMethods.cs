namespace ContosoProductsSearch.App.ExtensionMethods
{
    public static class AuthenticationExtensionMethods
    {
        public static bool IsBirthday(this AuthenticationState authenticationState, string ClaimCompleanno)
        {
            var customerClaimDataNascita = authenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimCompleanno);
            if (customerClaimDataNascita is not null)
            {
                var dataNascita = Convert.ToDateTime(customerClaimDataNascita.Value);
                if (DateTime.Now.Month == dataNascita.Month && DateTime.Now.Day == dataNascita.Day)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
