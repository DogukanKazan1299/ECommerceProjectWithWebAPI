using Entities.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebAPIWithCoreMvc.Controllers
{
    public class UsersController : Controller
    {
        HttpClient _httpClient;
        private string url = "http://localhost:50331/api/";

        public UsersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _httpClient.GetFromJsonAsync<List<UserDetailDto>>(url + "users/getlist");
            return View(users);
        }
    }
}
