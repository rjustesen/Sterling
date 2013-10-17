using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Service;
using SS.Model;
using log4net;

public partial class Login : System.Web.UI.Page
{
    public virtual UserService UserService { get; set; }


    protected static readonly ILog log = LogManager.GetLogger(typeof(Login));


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["Logout"] != null)
            {
                UserService.LogOff(null);
            }
        }
    }
  
    protected void LoginButton_Click(object sender, EventArgs e)
    {
        User user = UserService.GetUser(txtUserName.Text);
        string redirect = "~/Default.aspx";
        if (!UserService.ValidateUser(txtUserName.Text, txtPassword.Text))
        {
            if (user != null)
            {
                if (user.IsDisabled)
                {
                    txtFailureText.Text = "LoginFailed - Account is Disabled";
                    redirect = "";
                } 
                else if (UserService.IsAccountLocked(user))
                {
                    txtFailureText.Text = "Login Failed - Account is locked";
                    redirect = "";
                }
                else if (!user.IsApproved && !user.IsDisabled)
                {
                    if (UserService.CheckPassword(txtPassword.Text, user.Password))
                    {
                        redirect = "Profile.aspx?reset=true";
                    }
                    else
                    {
                        txtFailureText.Text = "Login Failed - Please try again";
                        redirect = "";
                    }
                }
                else
                {
                    txtFailureText.Text = "Login Failed - Please try again";
                    redirect = "";
                }
            }
            else
            {
                txtFailureText.Text = "Login Failed - Please try again";
                redirect = "";
            }
        }
       
        if (!String.IsNullOrEmpty(redirect) && !user.IsDisabled)
        {
            UserService.LogIn(user);
            BaseMaster bm = this.Master as BaseMaster;
            Session["SessionUser"] = user;
            bm.SetLoginInfo(user);
            Response.Redirect(redirect);
        }
    }
}
