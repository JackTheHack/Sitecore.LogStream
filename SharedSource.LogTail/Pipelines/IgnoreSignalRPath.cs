using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;

namespace SharedSource.LogTail.Pipelines
{
    public class IgnoreSignalRPath : HttpRequestProcessor
    {
        #region Public methods

        /// <summary>
        /// Runs the processor.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process([NotNull] HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));
            if (args.Url.FilePath.StartsWith(LogSettings.SignalRPath, StringComparison.OrdinalIgnoreCase))
            {
                args.AbortPipeline();
            }
        }

        #endregion
    }
}