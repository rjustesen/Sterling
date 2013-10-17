<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;
using SS.Service;
using SS.Model;
using Spring.Context;

public class ImageHandler : IHttpHandler {
    
     
    
    public void ProcessRequest (HttpContext context) {
        
        IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
        PortfolioService portfolioService = ctx["PortfolioService"] as PortfolioService;
        //User user = context.Session["SessionUser"] as User;
        int id = Convert.ToInt32(context.Request.QueryString["id"]);
        Attachment attachment = portfolioService.GetAttachment(id);
        if (null != attachment)
        {
            context.Response.ContentType = "image";
            context.Response.BinaryWrite(attachment.Data);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}