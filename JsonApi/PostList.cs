namespace JsonApi
{
    using System.Collections.Generic;

    public class PostList
    {
        public List<Post> Posts { get; set; }

        public PostList()
        {
            Posts = new List<Post>();
        }
    }

}
