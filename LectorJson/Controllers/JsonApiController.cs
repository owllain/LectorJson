using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LectorJson.Models; // Asegúrate de que la ruta del namespace sea correcta

namespace LectorJson.Controllers
{
    public class JsonApiController : Controller
    {
        private readonly HttpClient _httpClient;

        public JsonApiController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            string url = "https://jsonplaceholder.typicode.com/posts";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                List<Post> posts = JsonSerializer.Deserialize<List<Post>>(jsonData);

                // Añadiendo un contador a cada post
                for (int i = 0; i < posts.Count; i++)
                {
                    posts[i].Guide = i + 1; // Empieza en 1
                }

                return View(posts); // Retorna la vista con la lista de posts
            }

            return View("Error"); // Manejo de error
        }
    }
}
