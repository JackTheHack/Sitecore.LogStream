﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logstream.aspx.cs" Inherits="SharedSource.LogTail.sitecore.admin.logstream" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin - Log Streaming</title>
     <!--Script references. -->
    <!--Reference the jQuery library. -->
    <script src="<%= ResolveUrl("~/Scripts/jquery-1.6.4.min.js") %>"></script>
    <!--Reference the SignalR library. -->
    <script src="<%= ResolveUrl("~/Scripts/jquery.signalR-2.2.1.min.js") %>"></script>
    <!--Reference the autogenerated SignalR hub script. -->
    <script src="/sitecore/admin/signalr/hubs"></script>
    <!--Add script to update the page and send messages.--> 
    <script type="text/javascript">
        

        $(function () {

            function startHub() {
                //$.connection.hub.logging = true;
                $.connection.hub.start({ transport: 'longPolling' }).done(function () {
                    chat.invoke("LogStream");
                    $("#status").text("Polling updates...");
                    console.log("Connection successful...");
                }).fail(function () {
                    console.log("Connection failed... Reconnecting in 2 seconds");
                    $("#status").text("Connection failed... Reconnecting in 2 seconds");
                    setTimeout(function () {startHub(); }, 2000); // Restart connection after 5 seconds.
                });
            }

            // Declare a proxy to reference the hub. 
            var chat = $.connection.signalRLogHub;
            // Create a function that the hub can call to broadcast messages.
            chat.client.onLogUpdate = function (contents) {
                // Html encode display name and message. 

                $.each(contents, function(i, line) {
                    $("#contents").append($("<div/>").html(line));
                });

                if (contents.length > 0) {
                    window.scrollTo(0, document.body.scrollHeight);
                }
            };

            chat.client.onFileNameChanged = function (newFileName) {
                window.title = newFileName;
                $("#filename").text(newFileName);
                console.log("New filename - " + newFileName);
                $("#contents").html('');
            };

            $.connection.hub.disconnected(function () {
                $("#status").text("Lost connection... Reconnecting in 2 seconds...");
                console.log("Lost connection... Reconnecting in 2 seconds...");
                setTimeout(function () { startHub(); }, 2000); // Restart connection after 5 seconds.
            });

            $.connection.hub.connectionSlow(function () {
                console.log('We are currently experiencing difficulties with the connection.')
            });

            $.connection.hub.error(function (error) {
                console.log('SignalR error: ' + error);
            //    $("#status").text("Connection failed... Reconnecting in 2 seconds");
              //  setTimeout(function () { startHub(); }, 2000); // Restart connection after 5 seconds.
            });

            $.connection.hub.reconnecting(function (cb) {
                console.log("Connection failed... Reconnecting in 2 seconds");
                $("#status").text("Connection failed... Reconnecting in 2 seconds");
            });
            $.connection.hub.reconnected(function (cb) {
                console.log('Reconnected');
                $("#status").text("Reconnected");
                setTimeout(function () { startHub(); }, 2000); // Restart connection after 5 seconds.
            });
            // Start the connection.
            startHub();
        });
    </script>
    <style type="text/css">
        #status {
            color: lightgray;
        }

        #filename {
            font-weight: bold;
            color: greenyellow;
            background-color: black;
            height: 30px;
            position: fixed;
            top: 0px;
            width: 100%;            
        }

        #contents {
            margin-top: 30px;
        }

        body {
            margin: 0px;
        }
    </style>
</head>
<body>
    <div id="filename">
        
    </div>
    <div id="contents">
    </div>
    <div id="status"></div>
</body>
</html>
