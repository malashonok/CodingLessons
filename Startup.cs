using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CodingLessons.Startup))]
namespace CodingLessons
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
