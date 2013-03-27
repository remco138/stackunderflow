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
        public Post NewAnswer { get; set; }
    }
}