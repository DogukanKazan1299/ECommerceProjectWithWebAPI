using Entities.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebAPIWithCoreMvc.ViewModels;

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

        
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.GenderList = GenderFill();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(UserAddViewModel userAddViewModel)
        {
            UserAddDto userAddDto = new UserAddDto()
            {
                FirstName = userAddViewModel.FirstName,
                LastName = userAddViewModel.LastName,
                Address = userAddViewModel.Address,
                DateOfBirth = userAddViewModel.DateOfBirth,
                Email = userAddViewModel.Email,
                Password = userAddViewModel.Password,
                UserName = userAddViewModel.UserName,
                Gender = userAddViewModel.GenderID == 1 ? true : false
            };
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url + "users/add", userAddDto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }













        private class Gender
        {
            public int Id { get; set; }
            public string GenderName { get; set; }
        }
        private List<Gender> GenderFill()
        {
            List<Gender> genders = new List<Gender>();
            genders.Add(new Gender() { Id = 1, GenderName = "Erkek" });
            genders.Add(new Gender() { Id = 2, GenderName = "Kadın" });
            return genders;
        }

    }
}
