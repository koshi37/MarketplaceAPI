using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MarketplaceDataAccess;
using MarketplaceServices.Models;

namespace MarketplaceServices.Controllers
{
    public class CommentController : ApiController
    {
        marketplaceEntities db = new marketplaceEntities();

        [HttpGet]
        public IEnumerable<comment> GetUserComments(int id)
        {
            var comments = db.comment.Where(c => c.user_id == id).ToList();
            return comments;
        }

        [HttpPost]
        public IHttpActionResult AddComment(comment com)
        {
            try
            {
                var maxId = db.comment.Max(c => c.comment_id);
                db.comment.Add(new comment()
                {
                    comment_id = maxId + 1,
                    user_id = com.user_id,
                    author_id = com.author_id,
                    title = com.title,
                    text = com.text,
                    rating = com.rating
                });

                db.SaveChanges();
            }
            catch
            {
                return BadRequest("Adding comment error.");
            }

            return Ok("Comment is added");
        }

        [HttpGet]
        public UserRating GetUserRating(int id)
        {
            var comments = db.comment.Where(c => c.user_id == id).ToList();

            var count = comments.Count();
            if (count == 0) return null;
            var sum = comments.Select(c => c.rating).Sum();
            double average = sum / count;
            average = System.Math.Round(average, 2);

            var userRating = new UserRating
            {
                userId = id,
                countComment = count,
                totalRatePoint = sum,
                average = average
            };

            return userRating;
        }
    }
}
