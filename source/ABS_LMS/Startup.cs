using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ABS_LMS.Startup))]
namespace ABS_LMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
