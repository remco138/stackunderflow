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
    
    public partial class User
    {
        public User()
        {
            this.Comments = new HashSet<Comment>();
            this.Posts = new HashSet<Post>();
        }
        public int User_id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public int PermissionLEvel { get; set; }
        public int Votes { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Zip { get; set; }
        public string Photo { get; set; }
    
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
