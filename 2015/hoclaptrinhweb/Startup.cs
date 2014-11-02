using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(hoclaptrinhweb.Startup))]
namespace hoclaptrinhweb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
