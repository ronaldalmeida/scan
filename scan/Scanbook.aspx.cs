using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace scan
{
    public partial class Scanbook : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StatusText.Text = string.Format("Hello {0}!", User.Identity.Name);
        }
    }
}