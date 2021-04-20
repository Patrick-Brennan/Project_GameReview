using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GameReview2.Startup))]
namespace GameReview2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
