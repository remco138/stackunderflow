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

            public struct Tag 
            {
                public string Name;
                public string Summary;
                public int Occurrences;

                public Tag(string name, string summary, int occurrences) { Name = name; Summary = summary; Occurrences = occurrences; } 
            }


        private StackOverflowDbContext db = new StackOverflowDbContext();

        //
        // GET: /Tag/

        public ActionResult Index()
        {
            var tagNameList = (from q in db.Tagged_Questions
                              select q.Tag.Name).ToList();

            List<Tag> tagList = new List<Tag>();
            List<string> tagNames = tagNameList.Distinct().ToList();

            foreach (string t in tagNames)
            {
                tagList.Add(new Tag(t, "I require an explanation", tagNames.Count(q => q == t)));
            }
            tagList.OrderByDescending(x => x.Occurrences);  //I've got not clue whether this works or not


            tagList.Add(new Tag("Lisp", "Pure elegancy", 9999));
            tagList.Add(new Tag("C++", "A pretty cool language", 1337));
            tagList.Add(new Tag("C#", "Microsoft(c)", 123));
            tagList.Add(new Tag("VB", "Never heard of it. Disregard this very long text message supposedly explaining VB in detail .,.,gneweasdfghjkkl;asdfghjrwgfji4ker", -1));
            tagList.Add(new Tag("C", "newline\ngoes\nhere", 12));


            ViewBag.tagList = tagList;
            return View();  //(tagNameList);
        }

    }
}
