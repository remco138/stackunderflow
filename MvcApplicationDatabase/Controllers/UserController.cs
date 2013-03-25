﻿using MvcApplicationDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            string photoURL;
            try
            {
                if ((bool)Session["login"])
                {
                    if (user.Photo == null)
                    {
                        photoURL = "~/Content/themes/layout/photo.jpg";
                    }
                    else
                    {
                        photoURL = "~/Content/themes/layout/profilephoto/" + user.User_id.ToString() + ".jpg";
                    }
                    ViewBag.PhotoURL = photoURL;

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
            if (ModelState.IsValid)
            {
                user.DateCreated = DateTime.Now;
                user.PermissionLEvel = 0;
                user.Votes = 0;
                db.Users.Add(user);
                db.SaveChanges();
            }
            return View();
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id)
        {
            if ((bool)Session["login"])
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
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(user).State = System.Data.EntityState.Modified;
                    db.SaveChanges();

                    return View("index", "home");
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
            int id = db.Users.Where(u => u.Username == user.Username).Select(u => u.User_id).First();
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
                if ( (bool)Session["login"] == true )
                {
                    Session.Add("Username", user.Username);
                    Session.Add("ID", id);
                    return RedirectToAction("index", "home");
                }
                return null;
            }
            catch
            {
                return View();
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
            Session["login"] = false;
            return RedirectToAction("Index", "Home");
        }
    }
}
