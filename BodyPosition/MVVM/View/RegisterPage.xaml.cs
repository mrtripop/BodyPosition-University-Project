using BodyPosition.MVVM.Model.AngleModel;
using BodyPosition.MVVM.Model.TestModel;
using BodyPosition.MVVM.Model.UserModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

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

        private Dictionary<string,UserModel> _userReadList = new Dictionary<string, UserModel>();

        private Dictionary<string, Dictionary<string, TestModel>> _testReadList = new Dictionary<string, Dictionary<string, TestModel>>();

        private Dictionary<string, Dictionary<string, AngleModel>> _angleReadList = new Dictionary<string, Dictionary<string, AngleModel>>();

        public RegisterPage()
        {
            InitializeComponent();

            _userReadList = ReadFile();
            _testReadList = ReadTest();
            _angleReadList = ReadAngle();
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
            Date = date.Text;
            Time = time.Text;

            // check info
            if (FirstName != "" && LastName != "" && Gender != "" && Phone != "" && weight.Text != "" &&
                height.Text != "" && Date != "" && Time != "")
            {

                UserModel newUser = new UserModel()
                {
                    Id = _userReadList.Count + 1,
                    FirstName = FirstName,
                    LastName = LastName,
                    Gender = Gender,
                    Tel = Phone,
                    Weight = WeightUser,
                    Height = HeightUser,
                    Date = Date,
                    Time = Time
                };

                _userReadList.Add(newUser.Id.ToString(), newUser);
                WriteFile(_userReadList);

                Dictionary<string, TestModel> newTest = new Dictionary<string, TestModel>();
                _testReadList.Add(newUser.Id.ToString(), newTest);
                WriteTest(_testReadList);

                Dictionary<string, AngleModel> newAngle = new Dictionary<string, AngleModel>();
                _angleReadList.Add(newUser.Id.ToString(), newAngle);
                WriteAngle(_angleReadList);


                MessageBox.Show("บันทึกสำเร็จ");
                this.Close();
            }
            else
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบทุกช่องก่อนทำการบันทึก");
            }
        }

        #region json method
        private Dictionary<string,UserModel> ReadFile()
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

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
