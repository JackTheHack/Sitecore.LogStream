# Sitecore.LogStream
Stream log from Sitecore instance without accessing the environment.

The goal of this module is to allow to view the latest log file from Sitecore using the admin page.
The admin page which could be accessed from /sitecore/admin/logstream.aspx will always stream the latest log file in Sitecore. If Sitecore is inaccessible it will reconnect as soon as Sitecore instance is back online.
