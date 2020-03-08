using System;
using System.Text.Json.Serialization;
namespace blog.netcore.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
        [JsonIgnore]
        public string Nonce { get;set; }

    }
}
