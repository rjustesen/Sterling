using System;
using System.Web;
using System.Web.Configuration;
using System.Configuration;
using log4net;
using Spring.Core.IO;

/// <summary>
/// Summary description for Global
/// </summary>
public class Global : System.Web.HttpApplication 
{
    private ILog log = LogManager.GetLogger(typeof(Global));

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        //if (Request.Url.AbsolutePath.EndsWith("/"))
        //{
        //  //  Server.Transfer(Request.Url.AbsolutePath + "Default.aspx");
        //}
        HttpRuntimeSection runTime = (HttpRuntimeSection)WebConfigurationManager.GetSection("system.web/httpRuntime");
        //Approx 100 Kb(for page content) size has been deducted because the maxRequestLength proprty is the page size, not only the file upload size
        int maxRequestLength = (runTime.MaxRequestLength - 100) * 1024;

        //This code is used to check the request length of the page and if the request length is greater than 
        //MaxRequestLength then retrun to the same page with extra query string value action=exception
        
        HttpContext context = ((HttpApplication)sender).Context;
        if (context.Request.ContentLength > maxRequestLength)
        {
            IServiceProvider provider = (IServiceProvider)context;
            HttpWorkerRequest workerRequest = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));
            // Check if body contains data
            if (workerRequest.HasEntityBody())
            {
                // get the total body length
                int requestLength = workerRequest.GetTotalEntityBodyLength();
                // Get the initial bytes loaded
                int initialBytes = 0;
                if (workerRequest.GetPreloadedEntityBody() != null)
                    initialBytes = workerRequest.GetPreloadedEntityBody().Length;
                if (!workerRequest.IsEntireEntityBodyIsPreloaded())
                {
                    byte[] buffer = new byte[512000];
                    // Set the received bytes to initial bytes before start reading
                    int receivedBytes = initialBytes;
                    while (requestLength - receivedBytes >= initialBytes)
                    {
                        // Read another set of bytes
                        initialBytes = workerRequest.ReadEntityBody(buffer, buffer.Length);
                        // Update the received bytes
                        receivedBytes += initialBytes;
                    }
                    initialBytes = workerRequest.ReadEntityBody(buffer, requestLength - receivedBytes);
                }
            }
            // Redirect the user to the same page with querystring action=exception. 
            context.Response.Redirect("~/Error/AttachError.aspx", true);
        }

    }

    void Application_Start(object sender, EventArgs e)
    {
        log4net.Config.XmlConfigurator.Configure();
        ResourceHandlerRegistry.RegisterResourceHandler("web", typeof(WebResource));
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        try
        {
            Exception ex = Server.GetLastError();
            if (null != ex)
            {
                Exception objErr = ex.GetBaseException();
                string err = " Error Caught in Application_Error event\n" +
                            " Error in: " + Request.Url.ToString() +
                            "\nError Message:" + objErr.Message.ToString() +
                            "\nStack Trace:" + objErr.StackTrace.ToString();
                log.Error(err);
                Server.ClearError();
                Response.Redirect("~/Error/Default.aspx");
            }
        }
        catch (Exception exx)
        {
            log.Error(exx.Message);
            Response.StatusCode = 404;
            this.CompleteRequest();
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        Session["SessionUser"] = null;
    }

}
 
