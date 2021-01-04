using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace MarketplaceServices.Controllers
{
    public class ImageController : ApiController
    {
        public const string CLOUD_NAME = "koshi";
        public const string API_KEY = "523154568781163";
        public const string API_SECRET = "yOJjqidhvBQjjK-Lb_0JN8bLpzs";
        static readonly Account account = new Account(CLOUD_NAME, API_KEY, API_SECRET);
        Cloudinary cloudinary = new Cloudinary(account);

        //[HttpPost]
        //public IHttpActionResult UploadImage(IFormFile file)
        //{
        //    var uploadParams = new ImageUploadParams()
        //    {
        //        File = file
        //    };
        //    var uploadResult = cloudinary.Upload(uploadParams);
        //    return Ok();
        //}

        ////upload image to cloud
        //public ImageUploadResult Upload(ImageUploadParams parameters);
        ////upload from file
        
    }
}
