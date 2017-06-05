using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(maratonaxamarin5Service.Startup))]

namespace maratonaxamarin5Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}