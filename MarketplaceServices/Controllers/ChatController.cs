using MarketplaceDataAccess;
using MarketplaceServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MarketplaceServices.Controllers
{
    public class ChatController : ApiController
    {
        private marketplaceEntities db = new marketplaceEntities();

        [HttpPost]
        public IHttpActionResult CreateChat(chat _chat)
        {
            var maxId = 1;
            if(db.chat.Any())
                maxId= db.chat.Max(c => c.chat_id) + 1;
            //ad newPost = _ad;
            //newPost.ad_id = maxId;
            //db.ad.Add(newPost);
            if(db.chat.Where(c => c.chat_id == _chat.chat_id && c.ad_id == _chat.ad_id && c.seller_id == _chat.seller_id && c.buyer_id == _chat.buyer_id).Any())
            {
                db.chat.Add(new chat()
                {
                    chat_id = _chat.chat_id,
                    ad_id = _chat.ad_id,
                    seller_id = _chat.seller_id,
                    buyer_id = _chat.buyer_id
                });
                db.SaveChanges();
                return Ok("Created new chat succesfully");
            }
            return Ok("Chat already exists");
        }
        
        [HttpGet]
        public chat GetPostBuyer(int adId, int buyerId)
        {
            if(adId > 0 && buyerId > 0)
            {
                var chat = db.chat.Where(c => c.ad_id == adId && c.buyer_id == buyerId).FirstOrDefault();
                if (chat == null)
                {
                    var maxId = 1;
                    if (db.chat.Any())
                        maxId = db.chat.Max(c => c.chat_id) + 1;
                    var ad = db.ad.Where(a => a.ad_id == adId).FirstOrDefault();
                    var seller_id = ad.user_id;
                    chat = new chat
                    {
                        chat_id = maxId,
                        ad_id = adId,
                        seller_id = seller_id,
                        buyer_id = buyerId
                    };

                    db.chat.Add(chat);
                    db.SaveChanges();
                }

                return chat;
            }
            return null;
        }

        [HttpGet]
        public IEnumerable<chat> GetUserChats(int id)
        {
            var chats = db.chat.Where(c => (c.seller_id == id || c.buyer_id == id) && c.message.Count() > 0).ToList();
            return chats;
        }
    }
}
