using MarketplaceDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketplaceServices.Controllers
{
    public class ProvinceController : ApiController
    {
        private marketplaceEntities db = new marketplaceEntities();

        [HttpGet]
        public province GetByName(string name)
        {
            return db.province.Where(p => p.name == name).FirstOrDefault();
        }

        [HttpGet]
        public province GetById(int id)
        {
            return db.province.Where(p => p.province_id == id).FirstOrDefault();
        }

        public IEnumerable<province> GetAll()
        {
            return db.province.ToList();
        }
    }
}
