using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


/// <summary>
/// Summary description for ClientHelper
/// </summary>
public class ClientHelper
{
     public static void ClientMessage(Page p, string caption, string message)
     {
      if (null == p.Master)
      {
            DisplayMessage(p, message);
      }
      else 
      {
            Label _lblClientMessage_  = p.Master.FindControl("_lblClientMessage_") as Label;
            Label _lblCaption_ = p.Master.FindControl("_lblCaption_") as Label;
            if (String.IsNullOrEmpty(caption))
            {
                _lblCaption_.Text = "Error";
            } 
            else 
            {
                _lblCaption_.Text = caption;
            }
            _lblClientMessage_.Text = message;
            UpdatePanel upd = p.Master.FindControl("_updClientMessage_") as UpdatePanel;
            upd.Update();
            AjaxControlToolkit.ModalPopupExtender extender = p.Master.FindControl("mdlPopup") as AjaxControlToolkit.ModalPopupExtender;
            extender.Show();
      }

     }

    public static void DisplayMessage(Page p, string Message)
    {
        p.ClientScript.RegisterClientScriptBlock(p.GetType(), System.Guid.NewGuid().ToString(), String.Format("<script language=JavaScript> alert(‘{0}’);</script>", Message));
    }


    public ClientHelper()
    {
        
    }
}
