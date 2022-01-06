using Entities.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebAPIWithWindowsForm
{
    public partial class Form1 : Form
    {
        private string url = "http://localhost:50331/api/";
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await DataGridViewFill();
        }
        //GetAll;
        private async Task DataGridViewFill()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var users = await httpClient.GetFromJsonAsync<List<UserDetailDto>>(new Uri(url + "users/getlist"));
                dataGridView1.DataSource = users;
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            using (HttpClient httpClient=new HttpClient())
            {
                UserAddDto userAddDto = new UserAddDto()
                {
                    FirstName = txtName.Text,
                    Address = txtAdress.Text,
                    DateOfBirth = Convert.ToDateTime(dtp.Text),
                    Email = txtEmail.Text,
                    LastName = txtLastName.Text,
                    Gender = cbb.Text == "Erkek" ? true : false,
                    Password = txtPassword.Text,
                    UserName = txtUserName.Text
                };
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(url + "users/add", userAddDto);
                if (response.IsSuccessStatusCode)
                {
                    await DataGridViewFill();
                    MessageBox.Show("Ekleme işlemi başarılı...");
                }
                else
                {
                    MessageBox.Show("Ekleme işlemi başarısız");
                }
            }
        }
    }
}
