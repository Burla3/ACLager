using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ACLager.Startup))]
namespace ACLager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
