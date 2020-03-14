using System;
using System.Collections.Generic;

namespace blog.netcore.Models
{
    public class PaginatedResponse<T> : PaginatedModel where T: class
    {
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
