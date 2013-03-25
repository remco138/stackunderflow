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


        public ActionResult ModifySummary()
        {
            int tagId = Int32.Parse(Request.QueryString["tagId"].Replace("summary", "").Trim()); //Wish i knew earlier about Trim(), removes all trailing whitespaces
            string newSummary = Request.QueryString["newSummary"].Trim();

            if (tagId >= 0)
            {
                Tag tag = db.Tags.Find(tagId);
                if (tag != null)
                {
                    tag.Summary = newSummary;
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
