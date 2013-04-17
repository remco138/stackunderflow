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
        //Index action for tags top-page, will return the tags ordered by times the tag is used
        public ActionResult Index()
        {
            List<Tag> tagList = db.Tags.OrderByDescending(x => x.Questions.Count).ToList();  //I've got not clue whether this works or not 
            ViewBag.isAdmin = UserController.isAdmin;
            return View(tagList);
        }

        //   /Tag/Delete/5
        //Action for admins/mods, will set the tag on "inactive", meaning that it will not be displayed at the top-tags page
        //These tags wont be deleted because there might be questions which use the tag (in a decent manner)
        //And nobody likes questions without tags!
        public ActionResult Delete(int id = -1)
        {
            ViewBag.isAdmin = UserController.isAdmin;

            if (UserController.isAdmin)
            {
                try
                {
                    db.Tags.First(t => t.Tag_id == id).Active = false;
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        //Action for modifying the summary (description) of the tag, this summary will be shown at the top-tags page
        //Only mods/admins will be able to use it, this action is important because new tags will have a default summary
        public ActionResult ModifySummary(int id = -1)
        {
            ViewBag.isAdmin = UserController.isAdmin;

            if (UserController.isAdmin)
            {
                string newSummary = Request.QueryString["newSummary"].Trim();

                if (id >= 0)
                {
                    Tag tag = db.Tags.Find(id);
                    if (tag != null)
                    {
                        tag.Summary = newSummary;
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
