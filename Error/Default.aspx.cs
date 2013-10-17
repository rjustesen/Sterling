using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Error_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
         
            string errorPath = Request["aspxerrorpath"];
            Exception ex = Server.GetLastError();
            string msg;
            if (null != ex)
            {
                msg = string.Format("An Error has occured. If you see this page please report the error to  the system administrator.<br />{0:s}<br />{1:s}", ex.Message, ex.StackTrace);
            }
            else
            {
                msg = string.Format("An Error has occured.<br />{0:s}", errorPath);
            }
            lblErrorMessage.Text = msg.Replace("\n", "<br />");
        }
    }
}
