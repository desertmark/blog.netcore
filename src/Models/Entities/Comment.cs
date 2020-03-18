using System;
using System.Text.Json.Serialization;

namespace blog.netcore.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Body { get; set; }

        public User User { get; set; }

        [JsonIgnore]
        public Post Post { get; set; }

    }
}
