using MyWallet.MVC5.Infrastructure;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace MyWallet.MVC5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];
            if (authCookie != null)
            {
                //用户,角色设置
                FormsAuthenticationTicket authTicket = null;
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                string[] roles = authTicket.UserData.Split(new char[] { ',' });//如果存取多个角色,我们把它分解 　 　
                FormsIdentity id = new FormsIdentity(authTicket);
                GenericPrincipal principal = new GenericPrincipal(id, roles);
                Context.User = principal;//存到HttpContext.User中 　 
            }
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            #region 记录错误
            Exception ex = Server.GetLastError().GetBaseException();
            StringBuilder str = new StringBuilder();
            str.Append("\r\n.客户信息：");
            string ip = "";
            if (Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR") != null)
            {
                ip = Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR").ToString().Trim();
            }
            else
            {
                ip = Request.ServerVariables.Get("Remote_Addr").ToString().Trim();
            }
            str.Append("\r\n\tIp:" + ip);
            str.Append("\r\n\t浏览器:" + Request.Browser.Browser.ToString());
            str.Append("\r\n\t浏览器版本:" + Request.Browser.MajorVersion.ToString());
            str.Append("\r\n\t操作系统:" + Request.Browser.Platform.ToString());
            str.Append("\r\n.错误信息：");
            str.Append("\r\n\t页面：" + Request.Url.ToString());
            str.Append("\r\n\t错误信息：" + ex.Message);
            str.Append("\r\n\t错误源：" + ex.Source);
            str.Append("\r\n\t异常方法：" + ex.TargetSite);
            str.Append("\r\n\t堆栈信息：" + ex.StackTrace);
            str.Append("\r\n--------------------------------------------------------------------------------------------------");
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Error(str.ToString());
            #endregion

            #region 跳转到错误友好页面(400,500)
            HttpException httpex = ex as HttpException;
            int httpStatusCode = httpex != null ? httpex.GetHttpCode() : 500;
            if (httpStatusCode == 404)
            {
                Response.Redirect("~/"+ CommonFuntion.LangCode() + "/System/PageNotFound");
            }
            else if (httpStatusCode == 500)
            {
                Response.Redirect("~/"+ CommonFuntion.LangCode() + "/System/PageError");
            }
            #endregion
        }

        
    }
}
