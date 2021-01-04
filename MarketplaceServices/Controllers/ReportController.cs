using MarketplaceDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketplaceServices.Controllers
{
    public class ReportController : ApiController
    {
        marketplaceEntities db = new marketplaceEntities();
        
        [HttpGet]
        public IEnumerable<reported_post> GetAllConsidered()
        {
            var list = db.reported_post.Where(r => r.status >= 2);
            if (list == null || !list.Any())
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return list;
        }

        [HttpGet]
        public IEnumerable<reported_post> GetAllNotConsidered()
        {
            var list = db.reported_post.Where(r => r.status < 2);
            if (list == null || !list.Any())
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return list;
        }

        [HttpGet]
        public IEnumerable<reported_post> GetAll()
        {
            var list = db.reported_post.ToList();
            if (list == null || !list.Any())
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return list;
        }

        [HttpPost]
        public IHttpActionResult ReportPost(reported_post reported)
        {
            var maxId = (db.reported_post.Any() ? db.reported_post.Max(r => r.report_id) : 0) + 1;
            //ad newPost = _ad;
            //newPost.ad_id = maxId;
            //db.ad.Add(newPost);
            db.reported_post.Add(new reported_post()
            {
                report_id = maxId,
                user_id = reported.user_id,
                status = 0,
                reason = reported.reason,
                ad_id = reported.ad_id,
                date_added = DateTime.Now
            });
            db.SaveChanges();
            return Ok("Added report succesfully");
        }

        [HttpPut]
        public IHttpActionResult UpdateStatus(reported_post reported)
        {
            var report = db.reported_post.Where(r => r.report_id == reported.report_id).FirstOrDefault<reported_post>();

            if (report != null)
            {
                report.status = reported.status;

                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
