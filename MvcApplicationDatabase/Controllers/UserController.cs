using MvcApplicationDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;

namespace MvcApplicationDatabase.Controllers
{
    public class UserController : Controller
    {
        private StackOverflowDatabaseContext db = new StackOverflowDatabaseContext();
        //
        // GET: /User/

        public ActionResult Index()
        {
            var all = db.Users.Take(50);
            return View(all);
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id)
        {
            var user = db.Users.First(u => u.User_id == id);
            try
            {
                if ((bool)Session["login"])
                {
                    ViewBag.PhotoURL = "~/Content/themes/layout/" + user.Photo;

                    if ((int)Session["ID"] == id)
                    {
                        return View("ProfileAdmin", user);
                    }
                    else
                    {
                        ViewBag.wakka = (int)Session["ID"];
                        return View("Profile", user);
                    }
                }
            }
            catch (Exception ex) { return View("login"); }
            return View("login");
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            // WAT GAAT HIER FOUT?
            try
            {
                if (ModelState.IsValid)
                {
                    user.Photo = "photo.jpg";
                    user.Bio = "";
                    user.DateCreated = DateTime.Now;
                    user.PermissionLEvel = 0;
                    user.Votes = 0;
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("login", "user");
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id)
        {
            if (isLoggedIn)
            {
                var user = db.Users.First(u => u.User_id == id);
                return View("Edit", user);
            }
            else { return View("login"); }
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, User user)
        {
            // add lost stuff
            //var userDetails = db.Users.First(u => u.User_id == user.User_id);
            // post edits to db
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(user).State = System.Data.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("details/" + id.ToString());
                }
            }
            catch
            {
                
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Login(User user)
        {
            bool loginCheck = false;
            User userDetails;
            
            try { userDetails = db.Users.First(u => u.Username == user.Username); }
            catch (Exception ex) {
                return RedirectToAction("login", "user");
            }
            
            foreach (var userlist in db.Users)
            {
                if (userlist.Username.Equals(user.Username) && userlist.Password.Equals(user.Password))
                {
                    loginCheck = true;
                    break;
                }
            }
            Session.Add("login", loginCheck);
            try
            {
                Session.Add("ID", userDetails.User_id);
                if ( (bool)Session["login"] == true )
                {
                    Session.Add("Username", user.Username);
                    if (userDetails.PermissionLEvel == 1)
                        Session.Add("isAdmin", true);
                    return RedirectToAction("index", "home");
                }
                return RedirectToAction("index", "home");
            }
            catch
            {
                return RedirectToAction("index", "home");
            }
        }
        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /User/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public static bool isAdmin
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["isAdmin"] != null)
                    return true;
                return false;
            }

        }

        public static bool isLoggedIn
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["Login"] == null)
                    return false;
                else
                    return true;
            }
        }

        public static void CheckLogin()
        {
            if (!isLoggedIn)
            {
                System.Web.HttpContext.Current.Response.RedirectToRoute("Default", new { action = "Login", controller = "User" });
            }
        }
    }
}
