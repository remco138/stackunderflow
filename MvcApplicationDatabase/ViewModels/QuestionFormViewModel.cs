using MvcApplicationDatabase.Models;
using System;
using System.Collections.Generic;

namespace MvcApplicationDatabase.ViewModels
{
    public class QuestionFormViewModel
    {
        public Question Question { get; set; }
        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}
