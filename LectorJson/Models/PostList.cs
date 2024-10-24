using System.Collections.Generic;

namespace LectorJson.Models
{
    public class PostList
    {
        public List<Post> Posts { get; set; }

        public PostList()
        {
            Posts = new List<Post>();
        }
    }
}
