using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeKrekelGroup5.Startup))]
namespace DeKrekelGroup5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
