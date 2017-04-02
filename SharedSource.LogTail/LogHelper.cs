using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace SharedSource.LogTail
{
    public static class LogHelper
    {
        public static string[] Tail(this TextReader reader, int startLine, out int lastLine)
        {
            var buffer = new List<string>();
            string line;
            
            for (int i = 0; i < startLine; i++)
            {
                line = reader.ReadLine();
                if (line == null)
                {
                    lastLine = i;
                    return buffer.ToArray();
                }
            }

            lastLine = startLine;

            while (null != (line = reader.ReadLine()))
            {
                lastLine++;
                buffer.Add(line);
            }

            return buffer.ToArray();
        }

        public static string GetLatestLogFileName()
        {
            string dataFolder = Sitecore.Configuration.Settings.DataFolder;
            string logFolder = dataFolder + "\\logs";

            var logNameRegex = new Regex(@"^log\.([012]{2}[\d]{2})(\d{2})(\d{2})(\.(\d{2})(\d{2})(\d{2})){0,1}\.txt$", RegexOptions.Compiled);
            var fileList = Directory.GetFiles(logFolder);

            var matchedFiles = new List<Tuple<string, DateTime>>();
            
            foreach (var fileNameCandidate in fileList)
            {
                if (string.IsNullOrEmpty(fileNameCandidate))
                {
                    continue;
                }

                var match = logNameRegex.Match(Path.GetFileName(fileNameCandidate));
                if (match.Success)
                {
                    var date = new DateTime(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));

                    //check if we have time
                    if (match.Groups.Count > 4 && 
                        !string.IsNullOrEmpty(match.Groups[5].Value) && 
                        !string.IsNullOrEmpty(match.Groups[6].Value) && 
                        !string.IsNullOrEmpty(match.Groups[7].Value))
                    {
                        date = date.Add(new TimeSpan(int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value)));
                    }

                    matchedFiles.Add(new Tuple<string, DateTime>(fileNameCandidate, date));
                }
            }

            var latestLog = matchedFiles.OrderByDescending(i => i.Item2).FirstOrDefault();

            return latestLog != null ? latestLog.Item1 : null;
        }
    }
}