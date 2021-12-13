using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalProject.Pages.Shared
{
    public class AddCommentPartialModel : PageModel
    {
    
        public Comment NewComment { get; set; } = new();
        public Post Post { get; set; } = new();

        public int ParentCommentId { get; set; }

        public int CommentId { get; set; }
        public AddCommentPartialModel()
        {

        }
        public AddCommentPartialModel(int parentCommentId, int commentId)
        {
            ParentCommentId = parentCommentId;
            CommentId = commentId;

        }
    }
}
