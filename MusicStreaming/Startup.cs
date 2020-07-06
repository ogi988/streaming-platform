using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MusicStreaming.Startup))]
namespace MusicStreaming
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
