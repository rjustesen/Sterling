//------------------------------------------------------------------------
// Copyright 2011 Beneficial Life Insurance Company
// This program is an unpublished work fully protected by the United      
// States Copyright laws and is considered a trade secret belonging to   
// the copyright holder -- Beneficial Life Insurance Company
//------------------------------------------------------------------------
#region History
/*
 *  RTJ   09/01/2011   Original development
 */
#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using SS.Model;
using SS.Service;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GenericServices;
using log4net;
using Spring.Context;
#endregion

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : System.Web.UI.Page
{
    protected User _user;
    private EMailService _eMailService;
    private UserService _userService;
    private PortfolioService _portfolioService;
    private RegionService _regionService;
    private CategoryService _categoryService;


   
    protected static readonly ILog log = LogManager.GetLogger(typeof(BasePage));

    #region Services
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
    public EMailService EMailService
    {
        get
        {
            if (null == _eMailService)
            {
                IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
                _eMailService = ctx["EMailService"] as EMailService;
            }
            return _eMailService;
        }
        set { _eMailService = value; }
    }

    public PortfolioService PortfolioService
    {
        get
        {
            if (null == _portfolioService)
            {
                IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
                _portfolioService = ctx["PortfolioService"] as PortfolioService;
            }
            return _portfolioService;
        }
        set { _portfolioService = value; }
    }

    public RegionService RegionService
    {
        get
        {
            if (null == _regionService)
            {
                IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
                _regionService = ctx["RegionService"] as RegionService;
            }
            return _regionService;
        }
        set { _regionService = value; }
    }

    public CategoryService CategoryService
    {
        get
        {
            if (null == _categoryService)
            {
                IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
                _categoryService = ctx["CategoryService"] as CategoryService;
            }
            return _categoryService;
        }
        set { _categoryService = value; }
    }
    #endregion

    #region Global Properties
    protected User CurrentUser
    {
        get
        {
            return Session["SessionUser"] as User;
        }
    }
    #endregion

    


    /// <summary>
    /// Get access to the master page
    /// </summary>
    public BaseMaster MasterPage
    {
        get { return this.Master as BaseMaster; }
    }

    public BasePage()
    {
        //  this.Load += new System.EventHandler(this.Page_Load);
        // this.Error += new EventHandler(BasePage_Error);
    }

   
    protected override void OnLoad(EventArgs e)
    {
        if (null == CurrentUser && Request.Url.Segments.FirstOrDefault(x => x.Equals("DocView.aspx")) == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        try
        {
            //switch (CurrentUser.Role)
            //{
            //    case RoleType.Administrator:
            //        //if (Request.Url.Segments.FirstOrDefault(x => x.Contains("Admin")) == null)
            //        //{
            //        //   Response.Redirect("~/Error/NoAccess.aspx");
            //        //}
            //        break;
            //    case RoleType.Nominee:
            //        //if (Request.Url.Segments.FirstOrDefault(x => x.Contains("Nominee")) == null)
            //        //{
            //        //    Response.Redirect("~/Error/NoAccess.aspx");
            //        //}
            //        break;
            //    case RoleType.AreaJudge:
            //    case RoleType.RegionJudge:
            //        //if (Request.Url.Segments.FirstOrDefault(x => x.Contains("Judge")) == null)
            //        //{
            //        //    Response.Redirect("~/Error/NoAccess.aspx");
            //        //}
            //        break;
            //    case RoleType.Coordinator:
            //    case RoleType.Principal:
            //        //if (Request.Url.Segments.FirstOrDefault(x => x.Contains("School")) == null)
            //        //{
            //        //   Response.Redirect("~/Error/NoAccess.aspx");
            //        //}
            //        break;
            //    default:
            //        break;
            //}
            MasterPage.SetLoginInfo(this.CurrentUser);
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        base.OnLoad(e);
    }


    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            //if (null == CurrentUser && Request.Url.Segments.FirstOrDefault(x => x.Equals("DocView.aspx")) == null)
            //{
            //    Response.Redirect("~/Login.aspx");
            //}
            //switch (CurrentUser.Role)
            //{
            //    case RoleType.Administrator:
            //        //if (Request.Url.Segments.FirstOrDefault(x => x.Contains("Admin")) == null)
            //        //{
            //        //   Response.Redirect("~/Error/NoAccess.aspx");
            //        //}
            //        break;
            //    case RoleType.Nominee:
            //        //if (Request.Url.Segments.FirstOrDefault(x => x.Contains("Nominee")) == null)
            //        //{
            //        //    Response.Redirect("~/Error/NoAccess.aspx");
            //        //}
            //        break;
            //    case RoleType.AreaJudge:
            //    case RoleType.RegionJudge:
            //        //if (Request.Url.Segments.FirstOrDefault(x => x.Contains("Judge")) == null)
            //        //{
            //        //    Response.Redirect("~/Error/NoAccess.aspx");
            //        //}
            //        break;
            //    case RoleType.Coordinator:
            //    case RoleType.Principal:
            //        //if (Request.Url.Segments.FirstOrDefault(x => x.Contains("School")) == null)
            //        //{
            //        //   Response.Redirect("~/Error/NoAccess.aspx");
            //        //}
            //        break;
            //    default:
            //        break;
            //}
            //MasterPage.SetLoginInfo(this.CurrentUser);
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }


}
