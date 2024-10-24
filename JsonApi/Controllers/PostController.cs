using JsonApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public PostController(IMemoryCache cache)
    {
        _httpClient = new HttpClient();
        _cache = cache;
    }

    // GET: api/Post
    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        if (!_cache.TryGetValue("posts", out List<Post> posts))
        {
            string url = "https://jsonplaceholder.typicode.com/posts";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                posts = JsonSerializer.Deserialize<List<Post>>(jsonData);

                // Asignar el valor de Guide usando un contador
                for (int i = 0; i < posts.Count; i++)
                {
                    posts[i].Guide = i + 1;
                }

                // Almacenar los datos en caché
                _cache.Set("posts", posts);
            }
            else
            {
                return StatusCode(500, "Error al obtener los datos");
            }
        }

        return Ok(posts);
    }

    // GET: api/Post/{guide}
    [HttpGet("{guide}")]
    public async Task<IActionResult> GetPostByGuide(int guide)
    {
        if (_cache.TryGetValue("posts", out List<Post> posts))
        {
            var post = posts.FirstOrDefault(p => p.Guide == guide);
            if (post != null)
            {
                return Ok(post);
            }
            return NotFound($"Post with Guide {guide} not found.");
        }

        return NotFound("No posts available in cache.");
    }

    // DELETE: api/Post/{guide}
    [HttpDelete("{guide}")]
    public async Task<IActionResult> DeletePost(int guide)
    {
        if (_cache.TryGetValue("posts", out List<Post> posts))
        {
            var postToRemove = posts.FirstOrDefault(p => p.Guide == guide);
            if (postToRemove != null)
            {
                posts.Remove(postToRemove);
                _cache.Set("posts", posts); // Actualizar caché
                return Ok($"Post with Guide {guide} deleted.");
            }
            return NotFound($"Post with Guide {guide} not found.");
        }

        return NotFound("No posts available in cache.");
    }

    // PUT: api/Post/{guide}
    [HttpPut("{guide}")]
    public async Task<IActionResult> UpdatePost(int guide, [FromBody] Post updatedPost)
    {
        if (_cache.TryGetValue("posts", out List<Post> posts))
        {
            var existingPost = posts.FirstOrDefault(p => p.Guide == guide);
            if (existingPost != null)
            {
                // Actualiza las propiedades del post existente
                existingPost.Title = updatedPost.Title;
                existingPost.Body = updatedPost.Body;

                _cache.Set("posts", posts); // Actualizar caché
                return Ok($"Post with Guide {guide} updated.");
            }
            return NotFound($"Post with Guide {guide} not found.");
        }

        return NotFound("No posts available in cache.");
    }
}
