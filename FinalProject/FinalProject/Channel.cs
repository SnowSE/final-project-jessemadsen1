using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace FinalProject
{
    public static class MyGlobalVariables
    {
        public static string LastRoute { get; set; }
    }

    public class Channel
    {
        [Required]
        public int ID { get; set; }

        [Required, Display(Name = "Title")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required, Display(Name = "Description")]
        [StringLength(280)]
        public string Body
        {
            get; set;
        }
        public string Slug { get; set; }

        public List<Topic> Topics { get; set; }
    }

    public class Topic
    {
        [Required]
        public int ID { get; set; }

        [Required, Display(Name = "Title")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required, Display(Name = "Description")]
        [StringLength(280)]
        public string Body
        {
            get; set;
        }

        public string Slug { get; set; }
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }

        public List<Post> Posts { get; set; }
    }
    public class Post
    {
        [Required]
        public int ID { get; set; }

        [Required, Display(Name = "Title")]
        [StringLength(100)]
        public string Title { get; set; }
        /*        [Required]*/
        public string Slug { get; set; }

        public string Author { get; set; }
        public int AuthorID { get; set; }

        [Required, Display(Name = "Post")]
        [StringLength(280)]
        public string Body
        {
            get; set;
        }
        [Required, Display(Name = "Last Updated")]
        public DateTime PostedOn { get; set; } = DateTime.Now;

        public DateTime? LastEditedon { get; set; }

        public int Vote { get; set; }

        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public String TopicSlug { get; set; }

        public List<Comment> Comments { get; set; }
    }

    public class Comment
    {
        [Required]
        public int ID { get; set; }

        public string Author { get; set; }
        public int AuthorID { get; set; }
        public string? AvatarFileName { get; set; }

        [Required, Display(Name = "Make a comment")]
        [StringLength(500)]
        public string? Body { get; set; }

        [Required]
        public DateTime PostedOn { get; set;} = DateTime.Now;

        [DisplayName("Post")]
        public int PostId { get; set; }
        public List<Comment> ChildComment { get; set; }
        public int? ParentCommentId { get; set; }

    }

    public class Author
    {
        [Required]
        public int ID { get; set; }


        public string? AvatarFileName { get; set; }

        [StringLength(280)]
        public string Body { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public int? VoteTotal { get; set; }
        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; } 
        public DateTime? LastEditedon { get; set; }

    }

    public class Vote
    {
        [Required]
        public int ID { get; set; }

        public string? Author { get; set; }

        public int? PostId { get; set; }

        public int? CommentId { get; set; }
    }

        //https://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c
        //https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
        public static class Slug
    {
        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveDiacritics().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveDiacritics(this string text)
        {
            var s = new string(text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());

            return s.Normalize(NormalizationForm.FormC);
        }
    }
}
