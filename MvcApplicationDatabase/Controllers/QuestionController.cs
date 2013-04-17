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
    public class QuestionController : Controller
    {
        /* 
         * This is the question controller, Here models are sent to the views to render
         * 
         */

        private StackOverflowDatabaseContext db = new StackOverflowDatabaseContext();

        // GET: /Question/
        //Index action, it returns the 15 most recent questions by default.
        //The view allows to press buttons to sort on different methods, sorting will be based on arguments
        //
        //
        public ActionResult Index(string sort = "", int page = 1, int pagesize = 15)
        {
            page = page - 1;
            int questionCount = db.Questions.Count();
            IQueryable questionList;

            switch (sort)
            {
                case "votes":
                    questionList = db.Questions.OrderByDescending(q => q.Posts.FirstOrDefault().Votes)
                                           .Skip(page * pagesize)
                                           .Take(pagesize);
                    break;
                case "unanswered":
                    questionList = db.Questions.Where(q => q.Posts.Count == 1).OrderByDescending(q => q.DateCreated)
                                            .Skip(page * pagesize)
                                            .Take(pagesize);
                    break;
                case "reported":
                    if (UserController.isAdmin)
                    {
                        questionList = db.Questions.OrderByDescending(q => q.DateCreated)
                                                   .Where(q => q.Reported != "")
                                                   .Skip(page * pagesize)
                                                   .Take(pagesize);

                        break;
                    }
                    return RedirectToAction("Index");
                default:
                    questionList = db.Questions.OrderByDescending(q => q.DateCreated)
                                           .Skip(page * pagesize)
                                           .Take(pagesize);
                    break;
            }

            //provide the view with basic info
            ViewBag.StdPageSize = pagesize;
            ViewBag.QuestionCount = questionCount;
            ViewBag.RecentTags = db.Tags.OrderByDescending(x => x.Questions.Count).ToArray();
            ViewBag.isAdmin = UserController.isAdmin;
            ViewBag.showReportedTab = false;
    
            // Check for reported questions
            if (ViewBag.isAdmin && db.Questions.Any(q => q.Reported != ""))
            {
                ViewBag.showReportedTab = true;
            }

            return View(questionList);
        }

        //Will list questions which are tagged with the appropriate tag
        public ActionResult Tagged(string tag, int page = 1, int pagesize = 15)
        {
            page = page - 1;
            var questionList = db.Questions.OrderBy(q => q.DateCreated)
                                           .Where(q => q.Tags.Any(w => w.Name == tag))
                                           .Skip(page * pagesize)
                                           .Take(pagesize);

            //Provides view with basic info
            ViewBag.QuestionCount = db.Questions.Count();
            ViewBag.PageSize = pagesize;
            ViewBag.RecentTags = db.Tags.OrderByDescending(x => x.Questions.Count).ToArray();
            ViewBag.isAdmin = UserController.isAdmin;
            ViewBag.showReportedTab = false;

            // Check for reported questions
            if (UserController.isAdmin && db.Questions.Any(q => q.Reported != ""))
            {
                ViewBag.showReportedTab = true;
            }
            return View("Index", questionList);
        }

        //Action for the ask page, will redirect to the inlog page if user is not logged in
        public ActionResult Ask()
        {
            if (!UserController.isLoggedIn)
                return RedirectToAction("login", "user", new { auth_error = 1 });
            return View();
        }

        //The action for inserting a new question in the database
        [HttpPost, ValidateInput(false)]
        public ActionResult Ask(QuestionFormViewModel vm)
        {
            Markdown md = new Markdown();
            if (ModelState.IsValid)
            {
                vm.Question.Active = true;
                vm.Question.DateCreated = DateTime.Now;
                vm.Question.Posts.Add(new Post()
                {
                    Active = true,
                    Content = md.Transform(Request.Form["Post.Content"]),
                    User_id = (int)Session["ID"],
                    DateCreated = DateTime.Now,
                });
                
                // Was unable to add Tags, but fixed this by following the steps under 'Issues With Views': http://oyonti.wordpress.com/2011/05/26/unable-to-update-the-entityset-because-it-has-a-definingquery/
                //
                var tagNames = Request.Form["Tag.Name"].Split(" ,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (var t in tagNames)
                {
                    if (db.Tags.Where(q => q.Name == t).Count() == 0) //More elegant use of extensions goes here
                    {
                        vm.Question.Tags.Add(new Tag()
                        {
                            Active = true,
                            Name = t,
                            Summary = "Requires summary",
                        });
                    }
                    else
                    {
                        vm.Question.Tags.Add(db.Tags.Where(q => q.Name == t).ToList()[0]);
                    }
                }

                //Add and apply changes to the database
                db.Questions.Add(vm.Question);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(vm.Question);
        }


        //  /Question/1234
        //         
        //      Maps to:
        //  
        //  /Question/Details/1234
        //
        //
        public ActionResult Details(int id)
        {
            try
            {
                var question = db.Questions.First(q => q.Question_id == id);
                var posts = question.Posts.OrderBy(q => q.DateCreated).Skip(1);
                ViewBag.Login = Session["login"];
                ViewBag.isAdmin = UserController.isAdmin;

                if (UserController.isLoggedIn)
                    ViewBag.isUserWhoAskedThisQuestion = question.Posts.First().User_id == (int)Session["ID"];
                else
                    ViewBag.isUserWhoAskedThisQuestion = false;

                QuestionDetailsFormViewModel model = new QuestionDetailsFormViewModel()
                    {
                        Question = question,
                        BestAnswerPost = question.BestAnswer,
                        OpeningPost = question.Posts.First(),
                        Posts = posts,
                    };

                var row = db.Questions.Where(q => q.Question_id == id).Single();
                row.Views++;
                db.SaveChanges();
                return View(model);
               
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                // Unable to find the question id
                return RedirectToAction("Index");
            }
        }
        
        //  /Question/1234/answer/submit
        //
        //      Maps to:
        //
        //  [HttpPost]
        //  /Question/Details/1234
        //
        //Inserts a new answer in the database, will redirect to the login page when user is not logged in
        [HttpPost]
        public ActionResult Details(QuestionDetailsFormViewModel model)
        {
            if (!UserController.isLoggedIn)
            {
                return RedirectToRoute("Question", new { id = model.Question.Question_id });
            }

            model.NewAnswer.Active = true;
            model.NewAnswer.Question_id = model.Question.Question_id;
            model.NewAnswer.DateCreated = DateTime.Now;
            model.NewAnswer.User_id = (int)Session["ID"];

            db.Posts.Add(model.NewAnswer);
            db.SaveChanges();

            return RedirectToAction("Details");
        }

        //  /Question/10/bestanswer/3
        //
        //      Maps to:
        //  
        //  /Question/SetBestAnswer?question_id=10&bestanswer_id=12
        //
        //      
        public ActionResult SetBestAnswer(int question_id = 0, int bestanswer_id = 0)       
        {
            if (!UserController.isLoggedIn || question_id == 0 || bestanswer_id == 0)
            {
                return RedirectToAction("Index");
            }

            var targetQuestion = db.Questions.First(q => q.Question_id == question_id);
            var targetQuestionUser_id = targetQuestion.Posts.First().User_id;

            // Only the user who created the question can select a best answer.
            //

            if (targetQuestionUser_id == (int)Session["ID"])
            {
                targetQuestion.BestAnswer_id = bestanswer_id;
                db.SaveChanges();

                return RedirectToRoute("Question", new { id = targetQuestion.Question_id });
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        //Action for editing the question
        public ActionResult Edit(int id)
        {
            try
            {
                var question = db.Questions.First(q => q.Question_id == id);
                var openingPost = question.Posts.First();

                return View(question);
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        //Action for editing the question
        [HttpPost]
        public ActionResult Edit(Question question)
        {
            if (ModelState.IsValid && (question.Active == true || UserController.isAdmin))
            {
                db.Entry(question).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(question);
        }

        public ActionResult Comment(int id) // id = post_id
        {
            UserController.CheckLogin();

            try
            {
                ViewBag.Post_id = id;
                ViewBag.Post = db.Posts.First(p => p.Post_id == id);             
            }
            catch (InvalidOperationException ex)
            {
                return RedirectToAction("Index");
            }
 
            return View();
        }

        //Will insert a comment into the database
        [HttpPost]
        public ActionResult Comment(Comment comment)
        {
            UserController.CheckLogin();
            var post = db.Posts.First(p => p.Post_id == comment.Post_id);

            if (post.Active) // Can only add comments to the post if post is active.
            {
                comment.Active = true;
                comment.User_id = (int)Session["ID"];
                comment.DateCreated = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            return RedirectToRoute("Question", new { id = post.Question_id });
        }

        //Removes the reported status from a question
        //Admin should take action against the user who wrongly reported the questions
        public ActionResult RemoveReport(int id, bool active = true)
        {
           if (UserController.isAdmin && id >= 0)
            {
                try
                {
                    db.Questions.First(q => q.Question_id == id).Reported = null;   
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
                db.SaveChanges();
            }

            if(Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Index");
        }

        //Action for locking/unlocking a question, the active field is set to false
        //If the question is locked 
        public ActionResult Active(int id, bool active = true)
        {
            if (UserController.isAdmin && id >= 0)
            {
                try
                {
                    db.Questions.First(q => q.Question_id == id).Active = active;
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
                db.SaveChanges();
            }

            if(Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Index");
        }

    //Will report a question, mods/admins will see a red '!' next to it
    public ActionResult Report(int id)
        {
            if (id >= 0)
            {
                try
                {
                    db.Questions.First(q => q.Question_id == id).Reported = "May contain bad content"; //Introducing a new textbox will be a lot of effort
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
                db.SaveChanges();
            }
            if(Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Index");
        }

        //Action for handling votes, users who are logged in can upvote or downvote a question
        [WebMethod()]
        public void Vote(int? id, int? user_id, string type = "up")
        {
            if (id != null && user_id != null)
            {
                var post = db.Posts.First(p => p.Post_id == id);
                var user = db.Users.First(u => u.User_id == user_id);
                if (type == "up")
                {
                    ++post.Votes;
                    ++user.Votes;
                }
                else if (type == "down")
                {
                    --post.Votes;
                    --user.Votes;
                }
                db.SaveChanges();
            }
        }
    }
}
