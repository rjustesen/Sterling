using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Model;
using SS.Service;
using GenericServices;
using Spring.Context;
using log4net;

public partial class RecoverAccount : System.Web.UI.Page
{
    private UserService _userService;


    public UserService UserService
    {
        get
        {
            if (null == _userService)
            {
                IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
                _userService = ctx["UserService"] as UserService;
            }
            return _userService;
        }
        set { _userService = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
   
    protected void btnReset_Click(object sender, EventArgs e)
    {
        string email = txtEmail.Text;
        
        if (!String.IsNullOrEmpty(email))
        {
            User user = UserService.GetUser(email);
            if (null != user && !user.IsDisabled)
            {
                string newPassword = UserService.NewPassword(user);
                UserService.SendUserEmail(user, "Sterling Scholar password reset request", System.Web.HttpContext.Current.Server.MapPath("~/") + "/assets/PasswordResetTemplate.html");
            }
        }
        Response.Redirect("Default.aspx");
    }
    
}
