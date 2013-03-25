//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcApplicationDatabase.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Post
    {
        public Post()
        {
            this.Comments = new HashSet<Comment>();
            this.Questions = new HashSet<Question>();
        }
    
        public int Post_id { get; set; }
        public int Question_id { get; set; }
        public int User_id { get; set; }
        public string Content { get; set; }
        public int Votes { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string Reported { get; set; }
        public Nullable<bool> Active { get; set; }
    
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Question Question { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual User User { get; set; }
    }
}
