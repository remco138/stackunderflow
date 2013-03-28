﻿using MvcApplicationDatabase.Models;
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
            ViewBag.isAdmin = UserController.isAdmin;

            List<Tag> tagList = db.Tags.OrderByDescending(x => x.Questions.Count).ToList();  //I've got not clue whether this works or not 
            ViewBag.isAdmin = UserController.isAdmin;
            return View(tagList);
        }

        //   /Tag/Delete/5
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
