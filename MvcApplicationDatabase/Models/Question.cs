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
    
    public partial class Question
    {
        public Question()
        {
            this.Posts = new HashSet<Post>();
            this.Tags = new HashSet<Tag>();
        }
    
        public int Question_id { get; set; }
        public Nullable<int> BestAnswer_id { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string Title { get; set; }
        public Nullable<int> Views { get; set; }
    
        public virtual ICollection<Post> Posts { get; set; }
        public virtual Post Post { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
