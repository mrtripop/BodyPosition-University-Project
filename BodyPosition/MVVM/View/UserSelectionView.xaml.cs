
using BodyPosition.MVVM.Model.AngleModel;
using BodyPosition.MVVM.Model.TestModel;
using BodyPosition.MVVM.Model.UserModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BodyPosition.MVVM.View
{
    /// <summary>
    /// Interaction logic for UserSelectionView.xaml
    /// </summary>
    public partial class UserSelectionView : Page
    {
        #region Parameter
        public List<UserModel> users = new List<UserModel>();
        private UserModel selectedUser;
        public UserModel UserSelected
        {
            get { return selectedUser; }
            set { selectedUser = value; }
        }

        private Dictionary<string, UserModel> _userReadFile = new Dictionary<string, UserModel>();

        private Dictionary<string, Dictionary<string, TestModel>> _testReadFile = new Dictionary<string, Dictionary<string, TestModel>>();

        private Dictionary<string, Dictionary<string, AngleModel>> _angleReadFile = new Dictionary<string, Dictionary<string, AngleModel>>();
        #endregion

        #region Initialize
        public UserSelectionView()
        {
            InitializeComponent();

            _userReadFile = ReadJson();
            _testReadFile = ReadTest();
            _angleReadFile = ReadAngle();

            LoadUser();
            refreshList();

            this.DataContext = this;
        }
        #endregion

        #region Method
        private void refreshList()
        {
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource = users;
        }
        private void LoadUser()
        {
            foreach(var i in _userReadFile)
            {
                users.Add(i.Value);
            }
        }
        #endregion

        #region Button Event Method
        private void Selected(object sender, RoutedEventArgs e)
        {
            UserSelected = dgUsers.SelectedItem as UserModel;
            if (UserSelected == null)
            {
                return;
            }
            NavigationService.Navigate(new TestSelectionView(UserSelected));
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            // ตั้งตัวแปรรับค่าที่เลือกมาเป็น UserModel
            UserSelected = dgUsers.SelectedItem as UserModel;
            if (UserSelected == null)
            {
                return;
            }

            users.Remove(UserSelected);
            _userReadFile.Remove(UserSelected.Id.ToString());
            WriteFile(_userReadFile);

            _testReadFile.Remove(UserSelected.Id.ToString());
            WriteTest(_testReadFile);

            _angleReadFile.Remove(UserSelected.Id.ToString());
            WriteAngle(_angleReadFile);
            
            users.Clear();

            _userReadFile.Clear();
            _testReadFile.Clear();
            _angleReadFile.Clear();

            _userReadFile = ReadJson();

            LoadUser();
            refreshList();
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            UserSelected = dgUsers.SelectedItem as UserModel;
            if (UserSelected == null)
            {
                return;
            }

            UserModel nUser = new UserModel()
            {
                FirstName = UserSelected.FirstName,
                LastName = UserSelected.LastName,
                Id = UserSelected.Id,
                Height = UserSelected.Height,
                Weight = UserSelected.Weight,
                Gender = UserSelected.Gender,
                Tel = UserSelected.Tel,
                Date = UserSelected.Date,
                Time = UserSelected.Time
            };

            _userReadFile.Remove(UserSelected.Id.ToString());
            _userReadFile.Add(UserSelected.Id.ToString(), nUser);
            WriteFile(_userReadFile);

            MessageBox.Show("อัพเดตข้อมูลเสร็จสิ้น");
        }
        private void AddUser(object sender, RoutedEventArgs e)
        {
            RegisterPage regis = new RegisterPage();
            regis.ShowDialog();

            users.Clear();

            _userReadFile.Clear();

            _userReadFile = ReadJson();
            
            LoadUser();
            refreshList();
        }
        private void Search(object sender, RoutedEventArgs e)
        {
            if(searchTextBox.Text != "" )
            {
                int b = int.Parse(searchTextBox.Text);
                if (b <= _userReadFile.Count && b >0)
                {
                    List<UserModel> searchUser = new List<UserModel>();
                    string a = searchTextBox.Text.ToString();

                    searchUser.Add(_userReadFile[a]);

                    dgUsers.ItemsSource = null;
                    dgUsers.ItemsSource = searchUser;
                }
                else
                {
                    users.Clear();
                    _userReadFile.Clear();

                    _userReadFile = ReadJson();
                    MessageBox.Show("ไม่พบเลข ID ที่คุณต้องการ");

                    LoadUser();
                    refreshList();
                }
            }
            else
            {
                users.Clear();
                _userReadFile.Clear();

                _userReadFile = ReadJson();

                LoadUser();
                refreshList();
            }
        }
        #endregion

        #region json method
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
        private Dictionary<string, Dictionary<string, TestModel>> ReadTest()
        {
            JObject json = JObject.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\TestJson.json")));
            var testModel = TestModel.FromJson(json.ToString());
            return testModel;
        }
        private void WriteTest(Dictionary<string, Dictionary<string, TestModel>> test)
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\TestJson.json"), test.ToJson());
        }
        private Dictionary<string, Dictionary<string, AngleModel>> ReadAngle()
        {
            JObject json = JObject.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\AngleJson.json")));
            var angleModel = AngleModel.FromJson(json.ToString());
            return angleModel;
        }
        private void WriteAngle(Dictionary<string, Dictionary<string, AngleModel>> angle)
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\AngleJson.json"), angle.ToJson());
        }
        #endregion
    }
}
