using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplicationDatabase.Models;

namespace MvcApplicationDatabase.ViewModels
{
    public class CommentReply
    {
        Comment comment;
        Post post
        {
            get
            {
                return comment.Post;
            }
        }

    }
}