using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SharedSource.LogTail.Service
{
    public class SignalRLogHub : Hub
    {
        private const int MaxClients = 3;
        private int _clientCount = 0;
        private const int ReadInterval = 1000;
        private string currentFileName = string.Empty;
        int lastReaderLogLine = 0;

        public void LogStream()
        {
            if (!Sitecore.Context.IsLoggedIn || !Sitecore.Context.IsAdministrator)
            {
                Clients.Caller.onFailure("Not authorised.");
                return;
            }

            

            if (_clientCount == 0)
            {
                _clientCount++;

                //string currentFileName = string.Empty;
                

                while (_clientCount > 0)
                {

                    string latestLogFile = LogHelper.GetLatestLogFileName();

                    if (latestLogFile != currentFileName)
                    {
                        Clients.All.onFileNameChanged(Path.GetFileName(latestLogFile));
                        currentFileName = latestLogFile;
                        lastReaderLogLine = 0;
                    }

                    using (var file = new FileStream(currentFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var textReader = new StreamReader(file))
                    {
                        Clients.All.onLogUpdate(textReader.Tail(lastReaderLogLine, out lastReaderLogLine));
                    }

                    Thread.Sleep(ReadInterval);
                }
            }
            else
            {
             //   Clients.Caller.onFailure("Too many connections.");
            }

            _clientCount++;
        }

        public override Task OnConnected()
        {
            lastReaderLogLine = 0;
            Clients.All.onFileNameChanged(Path.GetFileName(currentFileName));

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            lastReaderLogLine = 0;
            Clients.All.onFileNameChanged(Path.GetFileName(currentFileName));

            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _clientCount--;            
            return base.OnDisconnected(stopCalled);
        }
    }
}