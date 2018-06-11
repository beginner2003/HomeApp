using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<HomeData>> Get()
        {
            using (var db = new LiteDatabase(@"\Data\HomeData.db"))
            {
                var col = db.GetCollection<HomeData>("HomeData");
                return col.FindAll().ToList();
            };
        }

        [HttpGet]
        public ActionResult<IEnumerable<HomeData>> GetLast24()
        {
            using (var db = new LiteDatabase(@"\Data\HomeData.db"))
            {
                var col = db.GetCollection<HomeData>("HomeData");
                return col.Find(x => x.Date >=  DateTime.Now.AddHours(-24).AddMinutes(-1)).ToList();
            };
        }

        [HttpGet]
        public ActionResult<KeyValuePair<DateTime, double>> GetLatestFishTankTemp()
        {
            using (var db = new LiteDatabase(@"\Data\HomeData.db"))
            {
                var col = db.GetCollection<HomeData>("HomeData");
                var result = col.FindOne(Query.All(Query.Descending));
                if (result != null){
                    return new KeyValuePair<DateTime,double>(result.Date, result.FishTankTemp);
                } 
                return null;
            }
        }

        // POST api/values
        [HttpPost]
        public void PostFishTankTemp(double value)
        {
            using (var db = new LiteDatabase(@"\Data\HomeData.db"))
            {
                var col = db.GetCollection<HomeData>("HomeData");
                col.Insert(new HomeData {
                    Date = DateTime.Now,
                    FishTankTemp = value
                });
            }
        }
    }
}
