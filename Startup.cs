using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KeatonBlog.Startup))]
namespace KeatonBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
