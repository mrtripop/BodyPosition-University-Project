using BodyPosition.Core;
using BodyPosition.MVVM.Model.AngleModel;
using BodyPosition.MVVM.Model.TestModel;
using BodyPosition.MVVM.Model.UserModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BodyPosition.MVVM.View
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {

        private string FirstName;
        private string LastName;
        private string Phone;
        private string Gender;
        private double WeightUser;
        private double HeightUser;
        private string Date;
        private string Time;
        private int Age;

        private DateTime currentTime = DateTime.Now;
        private DateTime selectDateTime;

        private Dictionary<string,UserModel> _userReadList = new Dictionary<string, UserModel>();

        private Dictionary<string, Dictionary<string, TestModel>> _testReadList = new Dictionary<string, Dictionary<string, TestModel>>();

        private string PATH_USER = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\UserJson.json");
        private string PATH_TEST = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\TestJson.json");

        private Database dbUserManager;
        private Database dbTestManager;

        public RegisterPage()
        {
            InitializeComponent();

            dbUserManager = new Database(PATH_USER);
            dbTestManager = new Database(PATH_TEST);

            _userReadList = dbUserManager.ReadUser();
            _testReadList = dbTestManager.ReadTest();
        }

        public int FindMax(List<int> list)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Empty list");
            }
            int max = int.MinValue;
            foreach (var type in list)
            {
                if (type > max)
                {
                    max = type;
                }
            }
            return max;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            // initialize info
            FirstName = firstName.Text;
            LastName = lastName.Text;
            Phone = phone.Text;
            Gender = gender.Text;
            if (weight.Text != "")
            {
                WeightUser = double.Parse(weight.Text);
            }
            if (height.Text != "")
            {
                HeightUser = double.Parse(height.Text);
            }
            Date = selectDateTime.ToString("dd/MM/yyyy");
            Time = currentTime.ToString("HH:mm");
            if (age.Text != "")
            {
                Age = int.Parse(age.Text);
            }
            

            // check info
            if (FirstName != "" && LastName != "" && Gender != "" && Phone != "" && weight.Text != "" &&
                height.Text != "" && Date != "" && Time != "" && age.Text != "")
            {
                List<int> fMax = new List<int>();
                foreach (var index in _userReadList)
                {
                    fMax.Add(index.Value.Id);
                }

                int max = FindMax(fMax);

                UserModel newUser = new UserModel()
                {
                    Id = max + 1,
                    FirstName = FirstName,
                    LastName = LastName,
                    Gender = Gender,
                    Tel = Phone,
                    Weight = WeightUser,
                    Height = HeightUser,
                    Date = Date,
                    Time = Time,
                    Age = Age,
                };

                _userReadList.Add(newUser.Id.ToString(), newUser);
                dbUserManager.WriteJson(_userReadList);

                Dictionary<string, TestModel> newTest = new Dictionary<string, TestModel>();
                _testReadList.Add(newUser.Id.ToString(), newTest);
                dbTestManager.WriteJson(_testReadList);

                MessageBox.Show("บันทึกสำเร็จ");
                this.Close();
            }
            else
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบทุกช่องก่อนทำการบันทึก");
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime newDate = (DateTime)(((DatePicker)sender).SelectedDate);
            selectDateTime = newDate;
        }
    }
}
