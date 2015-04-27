using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace DeKrekelGroup5.Controllers
{
    public class UploadController : Controller
    {
        // Enable both Get and Post so that our jquery call can send data, and get a status
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage UploadImage()
        {
            // Get a reference to the file that our jQuery sent.  Even with multiple files, they will all be their own request and be the 0 index
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];

            // do something with the file in this space 
            string pic = System.IO.Path.GetFileName(file.FileName);
            string path = System.IO.Path.Combine(Server.MapPath("~FTP/Images/profile"), pic);
            // file is uploaded
            file.SaveAs(path);

            // end of file doing

            // Now we need to wire up a response so that the calling script understands what happened
            System.Web.HttpContext.Current.Response.ContentType = "text/plain";
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new { name = file.FileName };

            System.Web.HttpContext.Current.Response.Write(serializer.Serialize(result));
            System.Web.HttpContext.Current.Response.StatusCode = 200;

            // For compatibility with IE's "done" event we need to return a result as well as setting the context.response
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}