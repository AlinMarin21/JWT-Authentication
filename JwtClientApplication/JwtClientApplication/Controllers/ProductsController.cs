using JwtClientApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace JwtClientApplication.Controllers
{
    public class ProductsController : Controller
    {
        public static string baseUrl = "https://localhost:7175/api/products";
        public async Task<IActionResult> Index()
        {
            var products = await GetProducts();
            return View(products);
        }

        [HttpGet]
        public async Task<List<Products>> GetProducts()
        {
            //use the access token to call a protected web api
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonString = await client.GetStringAsync(url);

            var res = JsonConvert.DeserializeObject<List<Products>>(jsonString).ToList();

            return res;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId, ProductName, Category, UnitPrice, StockQty")] Products products)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var stringContent = new StringContent(JsonConvert.SerializeObject(products), Encoding.UTF8, "application/json");

            await client.PostAsync(url, stringContent);

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl + "/" + id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonString = await client.GetStringAsync(url);

            var res = JsonConvert.DeserializeObject<Products>(jsonString);

            if (res == null)
            {
                return NotFound();
            }

            return View(res);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl + "/" + id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonString = await client.GetStringAsync(url);

            var res = JsonConvert.DeserializeObject<Products>(jsonString);

            if (res == null)
            {
                return NotFound();
            }

            return View(res);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl + "/" + id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            await client.DeleteAsync(url);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl + "/" + id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonString = await client.GetStringAsync(url);

            var products = JsonConvert.DeserializeObject<Products>(jsonString);

            if (products == null)
            {
                return NotFound();
            }

            return View(products);

        }
    }
}
