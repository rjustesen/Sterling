//------------------------------------------------------------------------
// Copyright (c) 2011 Beneficial Life Insurance Company
// This program is an unpublished work fully protected by the United      
// States Copyright laws and is considered a trade secret belonging to    
// the copyright holder -- Beneficial Life Insurance Company              
//------------------------------------------------------------------------
#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

/// <summary>
/// This user control is used to add a modal dialog box to a page.
/// </summary>
[ParseChildren(true)]
[PersistChildren(false)]
[ToolboxData("<{0}:ModalDialog runat=server></{0}:ModalDialog>")]
public partial class ModalDialog : System.Web.UI.UserControl
{

    //A panel that will represent the content.
    public class MyContainer : Panel, INamingContainer
    {
    }

    //Private class variables.
    private ITemplate _content;

    private MyContainer _container = new MyContainer();

    public ModalDialog()
    {
        Load += Page_Load;
        //Register some events.
        this.Init += new EventHandler(ModalDialog_Init);
        this.PreRender += new EventHandler(ModalDialog_PreRender);
    }


    [NotifyParentProperty(true)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ITemplate Content
    {
        get { return _content; }
        set { _content = value; }
    }

    public int Width
    {
        get { return Convert.ToInt32(pnlModelMain.Width.Value); }
        set { pnlModelMain.Width = new Unit(value); }
    }


    public int Height
    {
        get { return Convert.ToInt32(pnlModelMain.Height.Value); }
        set { pnlModelMain.Height = new Unit(value); }
    }

    public bool ShowTheModal
    {
        get
        {
            if (ViewState["ModalDialogShowModal"] == null)
            {
                ViewState["ModalDialogShowModal"] = false;
            }

            return Convert.ToBoolean(ViewState["ModalDialogShowModal"]);
        }
        private set { ViewState["ModalDialogShowModal"] = value; }
    }


    public bool OverflowAuto
    {
        get
        {
            if (ViewState["ModalDialogOverflowAuto"] == null)
            {
                ViewState["ModalDialogOverflowAuto"] = false;
            }

            return Convert.ToBoolean(ViewState["ModalDialogOverflowAuto"]);
        }
        set { ViewState["ModalDialogOverflowAuto"] = value; }
    }


    private void ModalDialog_Init(object sender, EventArgs e)
    {
        pnlModelMain.Controls.Clear();
        Content.InstantiateIn(_container);
        pnlModelMain.Controls.Add(_container);
    }


    private void ModalDialog_PreRender(object sender, EventArgs e)
    {
        if (OverflowAuto)
        {
            pnlModelMain.Style.Add("overflow", "auto");
        }
        else
        {
            pnlModelMain.Style.Remove("overflow");
        }

        //Show or hide the panels.
        if (ShowTheModal)
        {
            pnlModalMask.Visible = true;
            pnlModelMain.Visible = true;
        }
        else
        {
            pnlModalMask.Visible = false;
            pnlModelMain.Visible = false;
        }
    }

    public void Page_Load(object Src, EventArgs E)
    {
        // Sets style to position:absolute for IE 6
        if (Request.Browser.Browser == "IE" && Request.Browser.Version[0] == '6')
        {
            pnlModalMask.Style["position"] = "absolute";
            pnlModelMain.Style["position"] = "absolute";
        }
        // create the javascript using RegisterClientScriptBlock so that it only appears once on the page.
        string javascriptBlock = null;
        javascriptBlock = "<script type=\"text/javascript\">";
        javascriptBlock += "window.onresize = screenSize;";
        javascriptBlock += "window.onscroll = screenSize;";
        javascriptBlock += "function screenSize() {";
        javascriptBlock += "document.cookie = 'Width=' + document.documentElement.clientWidth; ";
        javascriptBlock += "document.cookie = 'Height=' + document.documentElement.clientHeight; ";
        javascriptBlock += "document.cookie = 'ScrollHeight=' + document.documentElement.scrollHeight; ";
        javascriptBlock += "document.cookie = 'ScrollWidth=' + document.documentElement.scrollWidth; ";
        javascriptBlock += "document.cookie = 'ScrollX=' + document.documentElement.scrollLeft; ";
        javascriptBlock += "document.cookie = 'ScrollY=' + document.documentElement.scrollTop; ";
        //  End If

        javascriptBlock += "}";
        javascriptBlock += "</script>";

        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "modalJS", javascriptBlock);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ModalStartScript", "screenSize();", true);
    }

    public override Control FindControl(string id)
    {
        Control control = base.FindControl(id);
        if (control == null)
        {
            return _container.FindControl(id);
        }
        else
        {
            return control;
        }
    }

    public void ShowModal()
    {
        //Get the screen width and height from the hid values.
        int width = Int32.Parse(Request.Cookies["Width"].Value);
        int height = Int32.Parse(Request.Cookies["Height"].Value);
        int scrollX = Int32.Parse(Request.Cookies["ScrollX"].Value);
        int scrollY = Int32.Parse(Request.Cookies["ScrollY"].Value);

        //Build the top and left offset for the main panel. This makes the panel appear
        //in the center of the screen.
        String top = ((height / 2) - (pnlModelMain.Height.Value / 2)).ToString() + "px";
        String left = ((width / 2) - (pnlModelMain.Width.Value / 2)).ToString() + "px";

        // changes position for IE 6
        if (Request.Browser.Browser == "IE" && Request.Browser.Version == "6.0")
        {
            top = ((height / 2) - (pnlModelMain.Height.Value / 2) + scrollY).ToString() + "px";
        }

        //Set the panel styles.
        pnlModelMain.Style.Add("top", top);
        pnlModelMain.Style.Add("left", left);

        pnlModalMask.Style.Add("top", "0px");
        pnlModalMask.Style.Add("left", "0px");
        pnlModalMask.Style.Add("left", scrollX.ToString());


        if (Request.Browser.Browser == "IE" && Request.Browser.Version == "6.0")
        {
            // changes height for IE 6
            pnlModalMask.Style.Add("width", Request.Cookies["ScrollWidth"].Value + "px");
            pnlModalMask.Style.Add("height", Request.Cookies["ScrollHeight"].Value + "px");
        }
        else
        {
            // other browsers
            pnlModalMask.Style.Add("width", Request.Cookies["Width"].Value + "px");
            pnlModalMask.Style.Add("height", Request.Cookies["Height"].Value + "px");
        }

        ShowTheModal = true;
    }

    public void HideModal()
    {
        ShowTheModal = false;
    }
}