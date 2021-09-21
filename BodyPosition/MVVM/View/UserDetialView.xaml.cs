using BodyPosition.Core;
using BodyPosition.MVVM.Model.TestModel;
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
        private State state;

        private Dictionary<string, UserModel> _userDic = new Dictionary<string, UserModel>();
        private Dictionary<string, Dictionary<string, TestModel>> _testDic = new Dictionary<string, Dictionary<string, TestModel>>();
        private Dictionary<string, Dictionary<string, TestModel>> testDic = new Dictionary<string, Dictionary<string, TestModel>>();
        private Dictionary<string, TestModel> nDic = new Dictionary<string, TestModel>();

        public enum State
        {
            edit = 0,
            save = 1,
            done = 2,
        }

        private Database dbUserManager;
        private Database dbUserBackup;

        private Database dbTest;
        private Database dbTestBackup;

        private string PATH_USER = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\UserJson.json");
        private string PATH_TEST = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\TestJson.json");
        private string PATH_USER_BACKUP = Path.Combine(Environment.CurrentDirectory, @"BackupDatabase\UserJson.json");
        private string PATH_TEST_BACKUP = Path.Combine(Environment.CurrentDirectory, @"BackupDatabase\TestJson.json");


        private string PATH_ANGLE = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\Angle\");
        private string PATH_ANGLE_BACKUP = Path.Combine(Environment.CurrentDirectory, @"BackupDatabase\Angle\");

        public UserDetialView(UserModel user)
        {
            InitializeComponent();

            dbUserManager = new Database(PATH_USER);
            dbUserBackup = new Database(PATH_USER_BACKUP);

            dbTest = new Database(PATH_TEST);
            dbTestBackup = new Database(PATH_TEST_BACKUP);

            _user = user;
            _userDic = dbUserManager.ReadUser();
            _testDic = dbTest.ReadTest();

            state = State.done;

            firstName.Text = _user.FirstName;
            lastName.Text = _user.LastName;
            gender.SelectedIndex = ComboIndex(_user.Gender);
            phone.Text = _user.Tel;
            weight.Text = _user.Weight.ToString();
            height.Text = _user.Height.ToString();
            date.Text = _user.Date;
            time.Text = _user.Time;
            age.Text = _user.Age.ToString();
            
        }

        private int ComboIndex(string d)
        {
            if (d == "Male")
            {
                return 0;
            }
            else
            {
                return 1;
            }
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
            age.IsEnabled = true;
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
            age.IsEnabled = false;
        }

        private void SaveData()
        {
            string gUser = _user.Gender;
            if (gender.SelectedItem != null)
            {
                gUser = gender.Text;
            }

            // rename & update UserModel
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
                Time = time.Text,
                Age = int.Parse(age.Text)
            };

            _userDic.Remove(_user.Id.ToString());
            _userDic.Add(_user.Id.ToString(), nUser);

            if(_user.FirstName != nUser.FirstName || _user.LastName != nUser.LastName)
            {
                // read ref database
                testDic = dbTest.ReadTest();

                // remove all test user
                _testDic.Remove(_user.Id.ToString());

                foreach (var test in testDic[_user.Id.ToString()].Values)
                {
                    string testName = nUser.FirstName + "_" + nUser.LastName + "_" + test.Id;

                    TestModel nTest = new TestModel()
                    {
                        Id = test.Id,
                        TestName = testName,
                        UserId = test.UserId,
                        Date = test.Date,
                        Time = test.Time
                    };

                    // add test index
                    nDic.Add(nTest.Id.ToString(), nTest);

                    // rename file JsonDatabase
                    string oldfile = PATH_ANGLE+test.TestName + ".json";
                    string newfile = PATH_ANGLE+nTest.TestName + ".json";

                    File.Move(oldfile, newfile);

                    // rename file BackupDatabase
                    string oldfileB = PATH_ANGLE_BACKUP + test.TestName + ".json";
                    string newfileB = PATH_ANGLE_BACKUP + nTest.TestName + ".json";

                    File.Move(oldfileB, newfileB);
                }

                // add test user
                _testDic.Add(_user.Id.ToString(), nDic);
                testDic.Clear();
            }

            // JsonDatabase
            dbUserManager.WriteJson(_userDic);
            dbTest.WriteJson(_testDic);

            // BackupDatabase
            dbUserBackup.WriteJson(_userDic);
            dbTestBackup.WriteJson(_testDic);

            CloseEditData();
            MessageBox.Show("อัพเดตข้อมูลเสร็จสิ้น");

            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
