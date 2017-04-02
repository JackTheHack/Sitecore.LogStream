using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Configuration;
using Microsoft.Owin;
using Owin;
using Sitecore.Diagnostics;

[assembly: OwinStartup(typeof(SharedSource.LogTail.Service.Startup))]
namespace SharedSource.LogTail.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(3);
            //GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(6);
            // Any connection or hub wire up and configuration should go here
            Log.Info("Initializing log SignalR service...", this);
            app.MapSignalR(LogSettings.SignalRPath, new HubConfiguration() { });
        }
    }
}