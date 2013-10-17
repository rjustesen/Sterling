//------------------------------------------------------------------------
// Copyright (c) 2011 Beneficial Life Insurance Company
// This program is an unpublished work fully protected by the United      
// States Copyright laws and is considered a trade secret belonging to    
// the copyright holder -- Beneficial Life Insurance Company              
//------------------------------------------------------------------------

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Popup message box control
/// </summary>
public partial class PopupMessageBox : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnOk.OnClientClick = string.Format("fnClickOK('{0}','{1}')", btnOk.UniqueID, "");
    }

    /// <summary>
    /// Display a message
    /// </summary>
    /// <param name="Message"></param>
    public void ShowMessage(string Message)
    {
        lblMessage.Text = Message;
        lblCaption.Text = "";
        tdCaption.Visible = false;
        mpext.Show();
    }
    /// <summary>
    /// Display a message with a caption
    /// </summary>
    /// <param name="Message"></param>
    /// <param name="Caption"></param>
    public void ShowMessage(string Message , string Caption)
    {
        lblMessage.Text = Message;
        lblCaption.Text = Caption;
        tdCaption.Visible = true;
        mpext.Show();
    }
    /// <summary>
    /// Hide the popup
    /// </summary>
    public void Hide()
    {
        lblMessage.Text = "";
        lblCaption.Text = "";
        mpext.Hide();
    }
    /// <summary>
    /// Handle the button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
     protected void btnOk_Click(object sender, EventArgs e )
     {
        Hide();
     }
}
