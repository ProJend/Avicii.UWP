using System.Collections.Generic;

namespace TrueLove.Lib.Models.Datebase
{
    public class Comment 
    {
        public string name { get; set; }
        public string comment { get; set; }
    }

    public class CommentManager
    {
        public static List<Comment> GetBooks()
        {
            var comments = new List<Comment>();
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });
            comments.Add(new Comment { name = "adad", comment = "adad" });

            return comments;
        }
    }
}
