using MarketplaceDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketplaceServices.Controllers
{
    public class MessageController : ApiController
    {
        private marketplaceEntities db = new marketplaceEntities();

        [HttpGet]
        public IEnumerable<message> getForChat(int id)
        {
            var messages = db.message.Where(m => m.chat_id == id).ToList();
            return messages;
        }

        [HttpPost]
        public message sendMessage(message _message)
        {
            var maxId = 1;
            if (db.message.Any())
                maxId = db.message.Max(c => c.message_id) + 1;

            if(_message.message_text != null && _message.chat_id > 0 && _message.author_id > 0)
            {
                var newMessage = new message
                {
                    message_id = maxId,
                    author_id = _message.author_id,
                    chat_id = _message.chat_id,
                    message_text = _message.message_text
                };
                db.message.Add(newMessage);
                db.SaveChanges();
                return newMessage;
            }

            return null;
        }
    }
}
