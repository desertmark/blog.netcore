using System;
using System.Collections.Generic;

namespace blog.netcore.Models
{
    public class PaginatedModel
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
