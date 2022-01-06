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
        private int selectedID = 0;
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
        //Add
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
        //get text.
        private async void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            selectedID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            using (HttpClient httpClient=new HttpClient())
            {
                var user = await httpClient.GetFromJsonAsync<UserDto>(url + "users/getbyid/" + selectedID);
                txtAdress.Text = user.Address;
                txtName.Text = user.FirstName;
                txtLastName.Text = user.LastName;
                txtUserName.Text = user.UserName;
                txtEmail.Text = user.Email;
                txtPassword.Text = String.Empty;
                dtp.Value = user.DateOfBirth;
                cbb.SelectedValue = user.Gender == true ? 1 : 2;
                CmbGenderFill();//cinsiyetleri getir
            }
        }
        //Edit . .
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                UserUpdateDto userUpdateDto = new UserUpdateDto()
                {
                    Id = selectedID,
                    FirstName = txtName.Text,
                    Address = txtAdress.Text,
                    Email = txtEmail.Text,
                    DateOfBirth = Convert.ToDateTime(dtp.Text),
                    Gender = cbb.Text == "Erkek" ? true : false,
                    LastName=txtLastName.Text,
                    Password=txtPassword.Text,
                    UserName=txtUserName.Text
                };
                HttpResponseMessage response = await httpClient.PutAsJsonAsync(url + "users/update", userUpdateDto);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Düzenleme başarılı");
                    await DataGridViewFill();
                }
                else
                {
                    MessageBox.Show("Düzenleme başarısız");
                }
                
            }
        }




        void CmbGenderFill()
        {
            List<Gender> genders = new List<Gender>();
            genders.Add(new Gender() { Id = 1, GenderName = "Erkek" });
            genders.Add(new Gender() { Id = 2, GenderName = "Kadın" });
            cbb.DataSource = genders;
            cbb.DisplayMember = "GenderName";
            cbb.ValueMember = "Id";
        }
        private class Gender
        {
            public int Id { get; set; }
            public string GenderName { get; set; }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            using (HttpClient httpClient=new HttpClient())
            {
                HttpResponseMessage response = await httpClient.DeleteAsync(url + "users/Delete/" + selectedID);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Silme işlemi başarılı...");
                    await DataGridViewFill();
                }
                else
                {
                    MessageBox.Show("silme başarısız..");
                }
            }
        }
    }
}
