using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebBilling.Startup))]
namespace WebBilling
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
