using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.sitecore.admin;

namespace SharedSource.LogTail.sitecore.admin
{
    public partial class logstream : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckSecurity();
        }
    }
}