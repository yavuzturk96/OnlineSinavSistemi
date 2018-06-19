using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Online_Sınav_Sistemi.Startup))]
namespace Online_Sınav_Sistemi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
