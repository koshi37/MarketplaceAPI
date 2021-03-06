﻿//using MarketplaceDataAccess;
//using MarketplaceServices.Models;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web;
//using System.Web.Http;
//using System.Web.Http.Cors;

//namespace MarketplaceServices.Controllers
//{
//    [EnableCors(origins: "*", headers: "*", methods: "*")]
//    public class PostsController : ApiController
//    {
//        private marketplaceEntities db = new marketplaceEntities();

//        [HttpGet]
//        public IEnumerable<ad> GetAll()
//        {
//            var posts = db.ad.ToList();
//            if (posts == null || !posts.Any())
//            {
//                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
//            }
//            return posts;
//        }

//        [HttpGet]
//        public ad GetById(int id)
//        {
//            var posts = db.ad.Where(o => o.ad_id == id).FirstOrDefault();
//            if (posts == null)
//            {
//                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
//            }
//            return posts;
//        }

//        [HttpGet]
//        public IEnumerable<ad> GetAllCategory(int id)
//        {
//            return db.ad.Where(p => p.category_id == id);
//        }

//        [HttpGet]
//        public IEnumerable<ad> GetAllUser(int id)
//        {
//            return db.ad.Where(p => p.user_id == id);
//        }

//        [HttpGet]
//        public IEnumerable<ad> GetAllUsername(string username)
//        {
//            user user_ = db.user.Where(u => u.username == username).FirstOrDefault();
//            return db.ad.Where(p => p.user_id == user_.user_id);
//        }

//        [HttpPost]
//        public IHttpActionResult AddPost(ad _ad)
//        {
//            var maxId = db.ad.Max(a => a.ad_id) + 1;
//            //ad newPost = _ad;
//            //newPost.ad_id = maxId;
//            //db.ad.Add(newPost);
//            db.ad.Add(new ad()
//            {
//                ad_id = maxId,
//                title = _ad.title,
//                category_id = _ad.category_id,
//                description = _ad.description,
//                date_added = DateTime.Now,
//                price = _ad.price,
//                user_id = _ad.user_id,
//                image_url = _ad.image_url
//            });
//            db.SaveChanges();
//            return Ok("Added post succesfully");
//        }

//        [HttpPut]
//        public IHttpActionResult EditPost(ad _ad)
//        {
//            var post = db.ad.Where(r => r.ad_id == _ad.ad_id).FirstOrDefault<ad>();

//            if (post != null)
//            {
//                post.title = _ad.title;
//                post.category_id = _ad.category_id;
//                post.description = _ad.description;
//                post.price = _ad.price;
//                post.image_url = _ad.image_url;

//                db.SaveChanges();
//                return Ok();
//            }
//            else
//            {
//                return NotFound();
//            }
//        }

//        [HttpDelete]
//        public IHttpActionResult DeletePost(int id)
//        {
//            if (id <= 0)
//                return BadRequest("Not a valid post id");

//            //var _reported_post = db.reported_post.Where(r => r.ad_id == id);
//            //db.Entry(_reported_post).State = System.Data.Entity.EntityState.Deleted;

//            //var _observed_post = db.observed_ad.Where(o => o.ad_id == id);
//            //db.Entry(_observed_post).State = System.Data.Entity.EntityState.Deleted;

//            var _ad = db.ad
//                .Where(a => a.ad_id == id)
//                .FirstOrDefault();
//            var observed_list = db.observed_ad.Where(o => o.ad_id == id).ToList();
//            var reported_list = db.reported_post.Where(r => r.ad_id == id).ToList();

//            foreach(observed_ad o in observed_list)
//            {
//                db.observed_ad.Remove(o);
//            }

//            foreach(reported_post r in reported_list)
//            {
//                db.reported_post.Remove(r);
//            }

//            db.ad.Remove(_ad);
//            //db.Entry(_ad).State = System.Data.Entity.EntityState.Deleted;
//            db.SaveChanges();

//            return Ok($"Successfully deleted post with id {_ad.ad_id}");
//        }

//        [HttpGet]
//        public IEnumerable<ad> GetObservedPostsUserId(int id)
//        {
//            List<ad> observed_list = new List<ad>();
//            IEnumerable<observed_ad> observed_ads = db.observed_ad.Where(o => o.user_id == id).ToList();
//            foreach (var o in observed_ads)
//            {
//                observed_list.Add(db.ad.Where(a => a.ad_id == o.ad_id).FirstOrDefault());
//            }
//            return observed_list;
//        }

//        [HttpGet]
//        public IEnumerable<ad> Search(string searchString)
//        {
//            var posts = from a in db.ad
//                         select a;
//            if (!String.IsNullOrEmpty(searchString))
//            {
//                posts = posts.Where(a => a.title.Contains(searchString));
//            }

//            return posts;
//        }

//        [HttpGet]
//        public IEnumerable<ad> SearchForCategory(string searchString, int categoryId)
//        {
//            var posts = from a in db.ad
//                        select a;
//            if (!String.IsNullOrEmpty(searchString))
//            {
//                posts = posts.Where(a => a.title.Contains(searchString) && a.category_id == categoryId);
//            }

//            return posts;
//        }

//        [HttpGet]
//        public PagingParameter Paging(int pageNumber, int pageSize, IEnumerable<ad> posts)
//        {
//            //var posts = db.ad.ToList();

//            var count = posts.Count();

//            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
//            var TotalPages = (int)Math.Ceiling(count / (double)pageSize);

//            // Returns List of Customer after applying Paging   
//            var items = posts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

//            // if CurrentPage is greater than 1 means it has previousPage  
//            var previousPage = pageNumber > 1;

//            // if TotalPages is greater than CurrentPage means it has nextPage  
//            var nextPage = pageNumber < TotalPages;

//            // Object which we are going to send in header   
//            var paginationMetadata = new PagingParameter
//            {
//                totalCount = count,
//                pageSize = pageSize,
//                currentPage = pageNumber,
//                totalPages = TotalPages,
//                previousPage = previousPage,
//                nextPage = nextPage,
//                posts = items
//            };

//            // Setting Header  
//            //HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
//            // Returing List of Customers Collections  
//            return paginationMetadata;
//        }
//    }
//}
