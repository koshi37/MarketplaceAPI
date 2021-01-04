using MarketplaceDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketplaceServices.Controllers
{
    public class CategoryController : ApiController
    {
        private marketplaceEntities db = new marketplaceEntities();

        [HttpGet]
        public category GetByName(string name)
        {
            return db.category.Where(c => c.name == name).FirstOrDefault();
        }

        [HttpGet]
        public category GetById(int id)
        {
            return db.category.Where(o => o.category_id == id).FirstOrDefault();
        }

        public IEnumerable<category> GetAll()
        {
            return db.category.ToList();
        }
    }
}
