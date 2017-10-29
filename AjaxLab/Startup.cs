using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AjaxLab.Startup))]
namespace AjaxLab
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
