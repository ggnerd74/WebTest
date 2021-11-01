using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebTest.Models;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;
using System.Configuration;

namespace WebTest.Code
{
    public class Email
    {
        public string cc { get; set; }
        public string Bcc { get; set; }
        public string To { get; set; }
        public string SpecialRequest { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string OrderItems { get; set; }
    }

    public static class LocalControllerHelper
    {

        [NonAction]
        public static string GetMVCUserName(this Controller cntr, int userid)
        {
            using (var db = new FoodDistribute_DWEntities())
            {
                var result = db.MVC_User.Where(i => i.UserID == userid).Select(o => o.UserName).FirstOrDefault();
                if (result != null)
                {
                    return result;
                }
                else
                    return "";
            }
        }

        [NonAction]
        public static int GetMVCUserID(this Controller cntr, string username)
        {
            if (cntr.Session[username] != null)
                return (int)cntr.Session[username];
            else
            {
                using (var db = new FoodDistribute_DWEntities())
                {
                    var result = db.MVC_GetUserID(username).FirstOrDefault();
                    if (result != null)
                    {
                        cntr.Session[username] = result.Value;
                        return result.Value;
                    }
                    else
                        return -1;
                }
            }
        }

        [NonAction]
        public static int? GetUserRepID(this Controller cntr, string username)
        {
            if (cntr.Session[username + "_repid"] != null)
                return (int)cntr.Session[username + "_repid"];
            else
            {
                using (var db = new FoodDistribute_DWEntities())
                {
                    var result = db.MVC_GetUser_RepID(username).FirstOrDefault();
                    if (result != null)
                        cntr.Session[username + "_repid"] = result.Value;
                    return result;
                }
            }
        }

        [NonAction]
        public static int? GetUserBuyerID(this Controller cntr, string username)
        {
            if (cntr.Session[username + "_buyerid"] != null)
                return (int)cntr.Session[username + "_buyerid"];
            else
            {
                using (var db = new FoodDistribute_DWEntities())
                {
                    var result = db.MVC_GetUser_BuyerID(username).FirstOrDefault();
                    if (result != null)
                        cntr.Session[username + "_buyerid"] = result.Value;
                    return result;
                }
            }
        }

        [NonAction]
        public static T RetrieveScheduledCache<T>(this Controller cntr, string timekey, string cachekey, Func<object[], T> returnfunc, double refreshperiod, params object[] funcparams)
        {
            var cache = cntr.HttpContext.Cache;
            if (cache[timekey] != null)
            {
                DateTime lastupdate = (DateTime)cache[timekey];
                DateTime now = DateTime.Now;
                long elapsedticks = now.Ticks - lastupdate.Ticks;
                TimeSpan elapsed = TimeSpan.FromTicks(elapsedticks);
                if (elapsed.TotalSeconds < refreshperiod && cache[cachekey] != null)
                {
                    return (T)cache[cachekey];
                }
            }

            if (returnfunc != null && funcparams != null)
            {
                T result = returnfunc(funcparams);
                cache[cachekey] = result;
                cache[timekey] = DateTime.Now;
                return result;
            }
            else
                return default(T);
        }

        [NonAction]
        public static T RetrieveScheduledSessionCache<T>(this Controller cntr, string timekey, string cachekey, Func<object[], T> returnfunc, double refreshperiod, params object[] funcparams)
        {
            var cache = cntr.HttpContext.Session;
            if (cache[timekey] != null)
            {
                DateTime lastupdate = (DateTime)cache[timekey];
                DateTime now = DateTime.Now;
                long elapsedticks = now.Ticks - lastupdate.Ticks;
                TimeSpan elapsed = TimeSpan.FromTicks(elapsedticks);
                if (elapsed.TotalSeconds < refreshperiod && cache[cachekey] != null)
                {
                    return (T)cache[cachekey];
                }
            }

            if (returnfunc != null && funcparams != null)
            {
                T result = returnfunc(funcparams);
                cache[cachekey] = result;
                cache[timekey] = DateTime.Now;
                return result;
            }
            else
                return default(T);
        }

        [NonAction]
        public static string GetStringFromView(this Controller cntr, PartialViewResult partial)
        {
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult ver = partial.ViewEngineCollection.FindPartialView(cntr.ControllerContext, partial.ViewName);
                partial.View = ver.View;

                ViewContext vc = new ViewContext(cntr.ControllerContext, partial.View, partial.ViewData, cntr.TempData, sw);
                partial.View.Render(vc, sw);
                return sw.ToString();
            }
        }
    }
}