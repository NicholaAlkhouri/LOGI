using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LOGI.Startup))]
namespace LOGI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
