using MvcApplicationDatabase.Models;
using MvcApplicationDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Diagnostics;
using MarkdownSharp;
using System.Web.Services;

namespace MvcApplicationDatabase.Controllers
{
    public class SearchController : Controller
    {
        private StackOverflowDatabaseContext db = new StackOverflowDatabaseContext();

        // GET: /Search/
        //
        [HttpGet]
        public ActionResult Index(String q)
        {
            if (q == "") 
                return View();

            // Search questions where the title, question-content, or tag name contains the search string
            //
            ViewBag.QuestionList = db.Questions.Where(question => question.Title.Contains(q) ||
                                                    question.Posts.FirstOrDefault().Content.Contains(q) ||
                                                    question.Tags.Select(t => t.Name).Contains(q));

            return View();
        }
    }
}
