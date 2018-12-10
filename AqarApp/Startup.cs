using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AqarApp.Startup))]
namespace AqarApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
