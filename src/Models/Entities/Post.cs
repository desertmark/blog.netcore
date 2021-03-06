using System;
using System.Collections.Generic;

namespace blog.netcore.Models
{
    public class Post
    {
        public int PostId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Body { get; set; }

        public User User { get; set; }

        public ICollection<Comment> Comments  { get; set; }
    }
}
