using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(finnfox.Startup))]
namespace finnfox
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
