using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(pPrimer.Web.Startup))]
namespace pPrimer.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
