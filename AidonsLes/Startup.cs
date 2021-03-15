using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AidonsLes.Startup))]
namespace AidonsLes
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
