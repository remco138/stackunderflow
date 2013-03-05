using MvcApplicationDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplicationDatabase.Controllers
{
    public class HomeController : Controller
    {
        private StackOverflowDatabaseContext db = new StackOverflowDatabaseContext();

        // GET: /Home/
        //
        public ActionResult Index()
        {
            // Voorbeeld: Om alle tags op te vragen die bij een bepaalde question horen:
            //
            var tagList = db.Questions.ToList()[0].Tags;
            // Werkt ook omgekeerd 
            var randomTag = new Tag();
            var QuestionList = randomTag.Questions.ToList();



            return View(db.Questions.ToList());
            
        }


    }
}
