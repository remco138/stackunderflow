using MvcApplicationDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplicationDatabase.Controllers
{
    public class TagController : Controller
    {




        private StackOverflowDatabaseContext db = new StackOverflowDatabaseContext();

        //
        // GET: /Tag/

        public ActionResult Index()
        {
            List<Tag> tagList = db.Tags.ToList();
            tagList.OrderByDescending(x => x.Questions.Count);  //I've got not clue whether this works or not 
            ViewBag.tagList = tagList;
 
            return View();  //(tagNameList);
        }

    }
}
