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

        // GET: /Tag/
        //
        public ActionResult Index()
        {
            List<Tag> tagList = db.Tags.OrderByDescending(x => x.Questions.Count).ToList();  //I've got not clue whether this works or not 
                
            return View(tagList);
        }

    }
}
