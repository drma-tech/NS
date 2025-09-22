using NS.Shared.Models.Auth;

namespace NS.WEB.Modules.Auth.Core
{
    public class SuperData
    {
        public AuthPrincipal? Principal { get; set; }
        public AuthLogin? Login { get; set; }
    }
}