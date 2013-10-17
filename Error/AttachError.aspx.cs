using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Error_AttachError : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblErrorMessage.Text = "Your attachment exxceed the maximum size alloed by the system. Please reduce the size of the attachment and try again";
        }
    }
}
