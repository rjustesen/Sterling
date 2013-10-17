using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Service;
using SS.Model;
using Spring.Context;

public partial class DocView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = Request["id"];
        if (null != id)
        {
            IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
            PortfolioService portfolioService = ctx["PortfolioService"] as PortfolioService;
            Attachment attachment = portfolioService.GetAttachment(Convert.ToInt32(id));
            if (null != attachment)
            {
                Response.Clear();
                string ext = Path.GetExtension(attachment.Name).Replace(".", "");
                try
                {
                    string mimeType = ApacheMimeTypes.MimeTypes[ext];
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", attachment.Name));
                    if (null != mimeType)
                    {
                        Response.ContentType = mimeType;
                    }
                }
                catch
                {
                    Response.ContentType = "text/plain";
                }
                Response.BinaryWrite(attachment.Data);
                Response.End();
            }
        }
    }
}
