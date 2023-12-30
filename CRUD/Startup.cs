using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CRUD.Startup))]
namespace CRUD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
