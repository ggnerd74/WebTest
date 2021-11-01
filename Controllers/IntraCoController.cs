using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTest.Models;
using WebTest.Code;
using System.Xml.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;

namespace WebTest.Controllers
{
    public class IntraCoController : Controller
    {

        public ActionResult PrepackForecast()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetPrepackForecastGridSet(int skip, int take, int page, int pageSize, string forecastdate)
        {
            List<PrePack_Forecast_GridSet_Result> results = new List<PrePack_Forecast_GridSet_Result>();

            try
            {
                using (var db = new FoodDistribute_DWEntities())
                {
                    DateTime dt = DateTime.Parse(forecastdate);
                    results = db.PrePack_Forecast_GridSet(dt).ToList();

                    var totalcount = results.Count;
                    results = results.Skip(skip).Take(take).ToList();

                    return Json(new { total = totalcount, items = results }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exc)
            {
                return Json(new { error = exc.Message, stack = exc.StackTrace }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult PrepackForecastUpdate(PrePack_Forecast_GridSet_Result forecast)
        {
            try
            {
                int userid = this.GetMVCUserID(User.Identity.Name);

                using (var db = new FoodDistribute_DWEntities())
                {
                    var results = db.PrePack_ForecastUpsert(forecast.rid, forecast.forecastdate, forecast.productcode, forecast.um, forecast.forecastvalue, forecast.actualvalue, userid, forecast.startingqoh).ToList();

                    return Json(new { update = true, rid = results!=null?results.First().rid:-1 });
                }
            }
            catch (Exception exc)
            {
                return Json(new { update = false, error = exc.Message });
            }
        }
    }
}