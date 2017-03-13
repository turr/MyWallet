using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MyWallet.MVC5.Infrastructure
{
    public class CommonFuntion
    {
        /// <summary>
        /// 获取Cookie的语言码
        /// </summary>
        /// <returns></returns>
        public static string LangCode()
        {
            string result = WebCont.DEFAULT_LANG;
            HttpCookie langCookie = HttpContext.Current.Request.Cookies["PresentLang"];
            if (langCookie != null)
            {
                result = CommonFuntion.StringDecoding(langCookie.Value);
            }
            return result;
        }

        /// <summary>
        /// 获取登陆用户信息
        /// </summary>
        /// <returns></returns>
        public static FormsAuthenticationTicket GetAuthenticationTicket()
        {
            //用户,角色设置
            FormsAuthenticationTicket authTicket = null;
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (authCookie != null)
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            return authTicket;
        }

        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringEncoding(string str)
        {
            char[] arrChar = str.ToCharArray();
            string strChar = "";
            for (int i = 0; i < arrChar.Length; i++)
            {
                arrChar[i] = Convert.ToChar(arrChar[i] + 1);
                strChar = strChar + arrChar[i].ToString();
            }
            return strChar;
        }

        /// <summary>
        /// 字符串解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringDecoding(string str)
        {
            char[] arrChar = str.ToCharArray();
            string strChar = "";
            for (int i = 0; i < arrChar.Length; i++)
            {
                arrChar[i] = Convert.ToChar(arrChar[i] - 1);
                strChar = strChar + arrChar[i].ToString();
            }
            return strChar;
        }

    }
}