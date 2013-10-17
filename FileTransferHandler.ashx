<%@ WebHandler Language="C#" Class="FileTransferHandler" %>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using SS.Service;
using SS.Model;
using Spring.Context;

public class FileTransferHandler : IHttpHandler {
    private readonly JavaScriptSerializer js = new JavaScriptSerializer();
    
    public void ProcessRequest (HttpContext context) {
        context.Response.AddHeader("Pragma", "no-cache");
        context.Response.AddHeader("Cache-Control", "private, no-cache");

        IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
        PortfolioService portfolioService = ctx["PortfolioService"] as PortfolioService;
        //User user = context.Session["SessionUser"] as User;
        int id = Convert.ToInt32(context.Request.QueryString["id"]);
        string category = context.Request["category"];
        string description = context.Request["description"];
        
        Attachment attachment = portfolioService.GetAttachment(id);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    // Handle request based on method
    private void HandleMethod(HttpContext context)
    {
        switch (context.Request.HttpMethod)
        {
            case "POST":
            case "PUT":
                UploadFile(context);
                break;
            default:
                context.Response.ClearHeaders();
                context.Response.StatusCode = 405;
                break;
        }
    }

    // Upload file to the server
    private void UploadFile(HttpContext context)
    {
        var headers = context.Request.Headers;

        if (string.IsNullOrEmpty(headers["X-File-Name"]))
        {
            UploadWholeFile(context);
        }
        else
        {
            UploadPartialFile(headers["X-File-Name"], context);
        }

        //WriteJsonIframeSafe(context, statuses);
    }

    // Upload partial file
    private void UploadPartialFile(string fileName, HttpContext context)
    {
        if (context.Request.Files.Count != 1) 
                throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
        var inputStream = context.Request.Files[0].InputStream;
        using (BinaryReader reader = new BinaryReader(inputStream))
        {
            byte[] buf = reader.ReadBytes(context.Request.Files[0].ContentLength);
            inputStream.Close();
            //image.Data = buf;// ImageUtils.NormalizeImage(buf);
            reader.Close();
        }
    }

    // Upload entire file
    private void UploadWholeFile(HttpContext context)
    {
        for (int i = 0; i < context.Request.Files.Count; i++)
        {
            var file = context.Request.Files[i];
            var inputStream = file.InputStream;
            using (BinaryReader reader = new BinaryReader(inputStream))
            {
                byte[] buf = reader.ReadBytes(context.Request.Files[0].ContentLength);
                inputStream.Close();
                //image.Data = buf;// ImageUtils.NormalizeImage(buf);
                reader.Close();
            }
            string fullName = Path.GetFileName(file.FileName);
          
        }
    }

    private void WriteJsonIframeSafe(HttpContext context)
    {
        //context.Response.AddHeader("Vary", "Accept");
        //try
        //{
        //    if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
        //        context.Response.ContentType = "application/json";
        //    else
        //        context.Response.ContentType = "text/plain";
        //}
        //catch
        //{
        //    context.Response.ContentType = "text/plain";
        //}

        //var jsonObj = js.Serialize(statuses.ToArray());
        //context.Response.Write(jsonObj);
    }

}