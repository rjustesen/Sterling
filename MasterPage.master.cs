using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SS.Model;

public partial class MasterPage : BaseMaster
{
   
    public override void SetLoginInfo(User user)
    {
        if (null != lblUserName)
        {
            lblUserName.Text = string.Format("{0:s}", user.FullName);
        }
        lnkLogout.Visible = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        /* Add generic javascript files to the header here
         * Note: This does not add the jquery files to the default master page header.
         * This should be done in content pages that actually need the jquery controls or functionality */
        HtmlGenericControl script = new HtmlGenericControl();
        script.TagName = "script";
        script.Attributes.Add("type", "text/javascript");
        script.Attributes.Add("language", "javascript"); //don't need it usually but for cross browser.
        script.Attributes.Add("src", ResolveUrl("~/assets/javascript/jscript.js"));
        this.Page.Header.Controls.Add(script);

    }

    private void Page_Init(object sender, EventArgs e)
    {

    }

    public override void ShowErrorMessage(string message)
    {
        lblErrorMessage.Text = message.Replace("\n", "<br />");
        App_Error.Visible = true;
    }
    public override void ShowWarningMessage(string message)
    {
        lblWarningMessage.Text = message.Replace("\n", "<br />");
        App_Warning.Visible = true;
    }
    public override void ShowInfoMessage(string message)
    {
        lblInformationMessage.Text = message.Replace("\n", "<br />");
        App_Information.Visible = true;
    }
    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx");
    }
}
