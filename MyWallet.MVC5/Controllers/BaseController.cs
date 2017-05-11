using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyWallet.MVC5.Infrastructure;

namespace MyWallet.MVC5.Controllers
{
    public class BaseController : Controller 
    {
        /// <summary>
        /// 重写控制器方法Action执行前判断
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            #region 设置语言码和保存到cookies
            //获取URL中的语言字符串
            string lang = filterContext.RouteData.Values["lang"].ToString().ToLower();
            string encodingLang = "";
            HttpCookie langCookie = Request.Cookies["PresentLang"];
            //查看url里面语言码是否错误
            if (!WebCont.STA_ALL_LANG.Contains(lang))
            {
                if (langCookie != null)
                {
                    lang = CommonFuntion.StringDecoding(langCookie.Value);
                }
                else
                {
                    lang = WebCont.DEFAULT_LANG;
                }
            }
            filterContext.RouteData.Values["lang"] = lang;
            encodingLang = CommonFuntion.StringEncoding(lang);
            //保存到cookies
            if (langCookie == null)
            {
                langCookie = new HttpCookie("PresentLang");
                langCookie.Value = encodingLang;
                langCookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(langCookie);
            }
            else if (langCookie.Value != encodingLang)
            {
                Request.Cookies["PresentLang"].Value = encodingLang;
                Response.Cookies["PresentLang"].Value = encodingLang;
                Response.Cookies["PresentLang"].Expires = DateTime.Now.AddDays(1);
            }
            #endregion
        }


        protected string ReturnMessageAndRedirect(string message, string controller, string action)
        {
            string result = "<script>alert('" + message + "') ;window.open('" + Url.Content("~/" + CommonFuntion.LangCode() + "/" + controller + "/" + action) + "', '_self')</script>";
            return result;
        }
    }
}