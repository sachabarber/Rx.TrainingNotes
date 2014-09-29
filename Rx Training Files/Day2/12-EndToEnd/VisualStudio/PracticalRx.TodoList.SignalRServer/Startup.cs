using Owin;

namespace PracticalRx.TodoList.SignalRServer
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.UseCors(CorsOptions.AllowAll);    //If I need this then get nuget package Install-Package Microsoft.Owin.Cors
            app.MapSignalR();
        }
    }
}