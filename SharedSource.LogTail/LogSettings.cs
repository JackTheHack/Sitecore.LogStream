using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Configuration;

namespace SharedSource.LogTail
{
    public static class LogSettings
    {
        public static string SignalRPath => Settings.GetSetting("SignalR.Path", "/signalr");
    }
}