using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Polly.Startup))]
namespace Polly
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
