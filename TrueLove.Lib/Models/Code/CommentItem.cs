using System;
using System.Collections.Generic;

namespace TrueLove.Lib.Models.Code
{
    public class CommentItem
    {
        public string name { get; set; }
        public string comment { get; set; }
        public DateTime dateTime { get; set; }
        public int width { get; set; }
    }

    public class CommentManager
    {
        public static List<CommentItem> GetComment()
        {
            var comments = new List<CommentItem>();
            comments.Add(new CommentItem { comment = "aaaaaaaaaaaxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", name = "ccc", width=256 });
            comments.Add(new CommentItem { comment = "aaaaaaaaa", name = "ccc", width=50});
            comments.Add(new CommentItem { comment = "aaaaaaaaaaaaaaaaaaa", name = "ccc", width=10 });
            comments.Add(new CommentItem { comment = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", name = "ccc", width = 256 });
            comments.Add(new CommentItem { comment = "aaaaaaaaa", name = "ccc", width = 50 });
            comments.Add(new CommentItem { comment = "aaaaaaaaaaaaaaaaaaa", name = "ccc", width = 10 });
            comments.Add(new CommentItem { comment = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", name = "ccc", width = 256 });
            comments.Add(new CommentItem { comment = "aaaaaaaaa", name = "ccc", width = 50 });
            comments.Add(new CommentItem { comment = "aaaaaaaaaaaaaaaaaaa", name = "ccc", width = 10 });

            return comments;
        }
    }
}
