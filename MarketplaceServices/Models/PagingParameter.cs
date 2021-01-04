using MarketplaceDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketplaceServices.Models
{
    public class PagingParameter : ApiController
    {
        const int maxPageSize = 20;
        public int _pageSize { get; set; } = 10;
        public int totalCount { get; set; } = 0;
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public bool previousPage { get; set; } = false;
        public bool nextPage { get; set; } = false;
        public IEnumerable<ad> posts;

        public int pageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
