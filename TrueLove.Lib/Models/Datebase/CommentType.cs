using System;
using System.Collections.Generic;

namespace TrueLove.Lib.Models.Datebase
{
    public class CommentType 
    {
        public string name { get; set; }
        public string comment { get; set; }
        public DateTime dateTime { get; set; }
        public int width { get; set; }
    }

    public class CommentManager
    {
        public static List<CommentType> GetComment()
        {
            var comments = new List<CommentType>();
            comments.Add(new CommentType { comment = "aaaaaaaaaaaxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", name = "ccc", width=256 });
            comments.Add(new CommentType { comment = "aaaaaaaaa", name = "ccc", width=50});
            comments.Add(new CommentType { comment = "aaaaaaaaaaaaaaaaaaa", name = "ccc", width=10 });
            comments.Add(new CommentType { comment = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", name = "ccc", width = 256 });
            comments.Add(new CommentType { comment = "aaaaaaaaa", name = "ccc", width = 50 });
            comments.Add(new CommentType { comment = "aaaaaaaaaaaaaaaaaaa", name = "ccc", width = 10 });
            comments.Add(new CommentType { comment = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", name = "ccc", width = 256 });
            comments.Add(new CommentType { comment = "aaaaaaaaa", name = "ccc", width = 50 });
            comments.Add(new CommentType { comment = "aaaaaaaaaaaaaaaaaaa", name = "ccc", width = 10 });

            return comments;
        }
    }
}
