using System;

namespace blog.netcore.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string Sub { get; set; }

        public string UniqueName { get; set; }

    }
}
