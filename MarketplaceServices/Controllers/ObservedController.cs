using MarketplaceDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketplaceServices.Controllers
{
    public class ObservedController : ApiController
    {
        private marketplaceEntities db = new marketplaceEntities();

        [HttpGet]
        public observed_ad GetList(int id)
        {
            return db.observed_ad.Where(o => o.observed_id == id).FirstOrDefault();
        }

        [HttpGet]
        public observed_ad GetListUsername(string username)
        {
            user user_ = db.user.Where(u => u.username == username).FirstOrDefault();
            return db.observed_ad.Where(o => o.user_id == user_.user_id).FirstOrDefault();
        }

        [HttpGet]
        public observed_ad GetListUserId(int id)
        {
            return db.observed_ad.Where(o => o.user_id == id).FirstOrDefault();
        }

        [HttpGet]
        public IEnumerable<observed_ad> GetAll()
        {
            return db.observed_ad.ToList();
        }

        [HttpGet]
        public bool IfObserved(int postId, int userId)
        {
            var observed = db.observed_ad.Any(p => p.ad_id == postId && p.user_id == userId);
            return observed;
        }

        [HttpPost]
        public IHttpActionResult ObservePost(observed_ad obs)
        {
            var exists = db.observed_ad.Any(o => o.ad_id == obs.ad_id && o.user_id == obs.user_id);
            if(exists)
            {
                return BadRequest("Already observed.");
            }
            else
            {
                var maxId = 1;
                if (db.observed_ad.Any())
                 maxId = db.observed_ad.Max(o => o.observed_id) + 1;
                db.observed_ad.Add(new observed_ad()
                {
                    observed_id = maxId,
                    user_id = obs.user_id,
                    ad_id = obs.ad_id
                });
                db.SaveChanges();
                return Ok("Successfully observed.");
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(int adId, int userId)
        {
            //var ifDelete = db.observed_ad.Any(o => o.ad_id == obs.ad_id && o.user_id == obs.user_id);
            observed_ad toDelete = db.observed_ad.FirstOrDefault(o => o.ad_id == adId && o.user_id == userId);
            if(toDelete != null)
            {
                db.Entry(toDelete).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest("No data to delete.");
            }
        }
    }
}
