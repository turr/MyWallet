using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MyWallet.MVC5.Infrastructure
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// View页面获取当前语言码扩展方法
        /// </summary>
        /// <param name="htmlhelper"></param>
        /// <returns></returns>
        public static string LangCode(this HtmlHelper htmlhelper)
        {
            return CommonFuntion.LangCode();
        }

        /// <summary>
        /// View页面获取多语言扩展方法
        /// </summary>
        /// <param name="htmlhelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Lang(this HtmlHelper htmlhelper, string key)
        {
            return Lang(key);
        }

        /// <summary>
        /// 根据语言码获取多语言值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Lang(string key)
        {
            Type resourceType = null;
            PropertyInfo p = null;
            switch (CommonFuntion.LangCode())
            {
                case "cn":
                    resourceType = typeof(Resources.cn);
                    break;
            }
            p = resourceType.GetProperty(key, BindingFlags.NonPublic | BindingFlags.Static);
            if (p != null)
                return p.GetValue(null, null).ToString();
            else
                return "null";
        }
    }
}