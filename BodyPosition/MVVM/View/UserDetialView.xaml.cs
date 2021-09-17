using BodyPosition.MVVM.Model.UserModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BodyPosition.MVVM.View
{
    /// <summary>
    /// Interaction logic for UserDetialView.xaml
    /// </summary>
    public partial class UserDetialView : Window
    {
        private UserModel _user;
        State state;

        private Dictionary<string, UserModel> _userDic = new Dictionary<string, UserModel>();
        public enum State
        {
            edit = 0,
            save = 1,
            done = 2,
        }

        public UserDetialView(UserModel user)
        {
            InitializeComponent();

            _user = user;
            _userDic = ReadJson();

            state = State.done;

            firstName.Text = _user.FirstName;
            lastName.Text = _user.LastName;
            gender.SelectedItem = _user.Gender;
            phone.Text = _user.Tel;
            weight.Text = _user.Weight.ToString();
            height.Text = _user.Height.ToString();
            date.Text = _user.Date;
            time.Text = _user.Time;
            
        }

        private void EditUserInfo(object sender, RoutedEventArgs e)
        {
            switch (state)
            {
                case State.done:
                    EditData();
                    EditButton.Content = "SAVE";
                    state = State.edit;
                    break;

                case State.edit:
                    SaveData();
                    EditButton.Content = "EDIT";
                    state = State.done;
                    break;
            }
        }

        private void EditData()
        {
            firstName.IsEnabled = true;
            lastName.IsEnabled = true;
            gender.IsEnabled = true;
            phone.IsEnabled = true;
            weight.IsEnabled = true;
            height.IsEnabled = true;
            date.IsEnabled = true;
            time.IsEnabled = true;
        }

        private void CloseEditData()
        {
            firstName.IsEnabled = false;
            lastName.IsEnabled = false;
            gender.IsEnabled = false;
            phone.IsEnabled = false;
            weight.IsEnabled = false;
            height.IsEnabled = false;
            date.IsEnabled = false;
            time.IsEnabled = false;
        }

        private void SaveData()
        {
            string gUser = _user.Gender;
            if (gender.SelectedItem != null)
            {
                gUser = gender.Text;
            }

            UserModel nUser = new UserModel()
            {
                FirstName = firstName.Text,
                LastName = lastName.Text,
                Id = _user.Id,
                Height = double.Parse(height.Text),
                Weight = double.Parse(weight.Text),
                Gender = gUser,
                Tel = phone.Text,
                Date = date.Text,
                Time = time.Text
            };

            _userDic.Remove(_user.Id.ToString());
            _userDic.Add(_user.Id.ToString(), nUser);
            WriteFile(_userDic);
            
            CloseEditData();
            MessageBox.Show("อัพเดตข้อมูลเสร็จสิ้น");

            this.Close();
        }

        private Dictionary<string, UserModel> ReadJson()
        {
            JObject json = JObject.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\UserJson.json")));
            var userModel = UserModel.FromJson(json.ToString());
            return userModel;
        }
        private void WriteFile(Dictionary<string, UserModel> user)
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\UserJson.json"), user.ToJson());
        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
