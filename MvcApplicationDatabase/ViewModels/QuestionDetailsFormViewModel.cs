using MvcApplicationDatabase.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MvcApplicationDatabase.ViewModels
{
    public class QuestionDetailsFormViewModel
    {
        private StackOverflowDatabaseContext db = new StackOverflowDatabaseContext();
        public Question Question { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; }
        public Post OpeningPost { get; set; }
        public Post BestAnswerPost { get; set; }
        public string[] GetUserDetails(int id)
        {
            string[] user = new string[2];
            user[0] = db.Users.Where(u => u.User_id == id).Select(u => u.Username).Single();
            user[1] = db.Users.Where(u => u.User_id == id).Select(u => u.Photo).Single();
            return user;
        }
    }
}