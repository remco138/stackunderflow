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
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    
    public partial class Post
    {
        public Post()
        {
            this.Comments = new HashSet<Comment>();
            this.Questions = new HashSet<Question>();
            this.Posts1 = new HashSet<Post>();
        }
    
        public int Post_id { get; set; }
        public Nullable<int> Question_id { get; set; }
        public Nullable<int> ReplyToPost_id { get; set; }
        [Required]
        [AllowHtml]
        public string Content { get; set; }
        public Nullable<int> Votes { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> User_id { get; set; }
    
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Question Question { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Post> Posts1 { get; set; }
        public virtual Post Post1 { get; set; }
        public virtual User User { get; set; }
    }
}
