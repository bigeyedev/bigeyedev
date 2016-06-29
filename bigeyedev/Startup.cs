using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(bigeyedev.Startup))]
namespace bigeyedev
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
