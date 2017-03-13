#if DEBUG
//#define login_debug
#endif

using MyWallet.BLL.InterfaceService;
using MyWallet.BLL.Service;
using MyWallet.Model;
using MyWallet.Model.Request.User;
using MyWallet.MVC5.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyWallet.MVC5.Controllers
{
    public class UserController : BaseController
    {
        #region 登陆模块

        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOn()
        {
            ViewBag.Title = HtmlExtensions.Lang("User_LogOn_Title");
            return View();
        }

        /// <summary>
        /// 登陆提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogOn(LogOnModel model)
        {
            //判断是否Model是否有错误信息弹出,如果有错误为false 没则为true
            if (ModelState.IsValid)
            {
                t_manager result = null;
                InterfaceManagerService manager_service = new ManagerService();
                try
                {
                    result = manager_service.Login(model).FirstOrDefault();
                }
                catch
                {

                }
#if login_debug
                result = new t_manager();
                result.mana_id = 1;
                result.mana_role = "1";
#endif
                if (result != null)
                {
                    //1.保存登陆名,如果设置了 [Authorize],则那些视图需要登陆成功后才能访问
                    //FormsAuthentication.SetAuthCookie(manager.mana_login_name.ToString(), false);

                    //2.存储登陆名外,再添加角色权限
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1, // 版本号。 
                        result.mana_id.ToString(), // 与身份验证票关联的用户ID。 
                        DateTime.Now, // Cookie 的发出时间。 
                        DateTime.Now.AddMinutes(15),// Cookie 的到期日期。 
                        false, // 如果 Cookie 是持久的，为 true；否则为 false。 
                        result.mana_role.ToString());//将存储在 Cookie 中的用户定义数据。 
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);//加密
                    //存入Cookie 
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);

                    return RedirectToAction("Index", "System");
                }
                else
                {
                    ViewBag.SubmitError = HtmlExtensions.Lang("_Error_User_LogOn_LoginFail");
                }
            }
            return View();
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            foreach (string cookiename in Request.Cookies.AllKeys)
            {
                HttpCookie cookies = Request.Cookies[cookiename];
                if (cookies != null)
                {
                    cookies.Expires = DateTime.Today.AddDays(-1);
                    Response.Cookies.Add(cookies);
                    Request.Cookies.Remove(cookiename);
                }
            }
            return RedirectToAction("LogOn", "User");
        }

        #endregion
    }
}