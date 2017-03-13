#if DEBUG
//#define login_debug
#endif

using MyWallet.BLL.InterfaceService;
using MyWallet.BLL.Service;
using MyWallet.Model;
using MyWallet.MVC5.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyWallet.MVC5.Controllers
{
    public class SystemController : BaseController
    {
        /// <summary>
        /// 登陆后主页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = HtmlExtensions.Lang("System_Index_Title"); 
            return View();
        }

        #region 待开发模块

        /// <summary>
        /// 待开发
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult DevelopMessageBoard()
        {
            ViewBag.Title = HtmlExtensions.Lang("_Layout_menu_DevMessageBoard");

            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            int mana_id = authentication == null ? 0 : Convert.ToInt32(authentication.Name);

            string return_data = "";
            t_setting setting = null;
            InterfaceSettingService setting_service = new SettingService();
            try
            {
                setting = setting_service.SearchByManagerID(mana_id).FirstOrDefault();
                if (setting != null)
                {
                    return_data = setting.deve_message_board;
                }
            }
            catch
            {

            }
#if login_debug
            return_data = "Debug 内容测试";
#endif
            ViewBag.DATA = JsonConvert.SerializeObject(return_data) ;
            return View();
        }

        /// <summary>
        /// 待开发说明提交
        /// </summary>
        /// <param name="dev_message"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult SaveDevelopMessageBoard(string dev_message)
        {
            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            int mana_id = authentication == null ? 0 : Convert.ToInt32(authentication.Name);
            InterfaceSettingService setting_service = new SettingService();
            try
            {
                t_setting setting = setting_service.SearchByManagerID(mana_id).FirstOrDefault();
                if (setting == null)
                {
                    setting = new t_setting();
                    setting.deve_message_board = dev_message;
                    setting.mana_id = mana_id;
                    setting_service.Insert(setting);
                }
                else
                {
                    setting.deve_message_board = dev_message;
                    setting_service.Update(setting);
                }
            }
            catch
            {
                
            }
            return RedirectToAction("DevelopMessageBoard", "System");
        }

        #endregion

        #region 错误页面模块

        public ActionResult RoleError()
        {
            FormsAuthenticationTicket authentication = CommonFuntion.GetAuthenticationTicket();
            if(authentication == null)
            {
                return RedirectToAction("LogOn", "User");
            }

            ViewBag.Title = HtmlExtensions.Lang("System_RoleError_Title");
            return View();
        }

        /// <summary>
        /// 400
        /// </summary>
        /// <returns></returns>
        public ActionResult PageNotFound()
        {
            ViewBag.Title = "400";
            return View();
        }

        /// <summary>
        /// 500
        /// </summary>
        /// <returns></returns>
        public ActionResult PageError()
        {
            ViewBag.Title = "500";
            return View();
        }

        #endregion
    }
}