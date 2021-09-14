using BodyPosition.Core;
using BodyPosition.MVVM.Model;
using BodyPosition.MVVM.Model.AngleModel;
using BodyPosition.MVVM.Model.TestModel;
using BodyPosition.MVVM.Model.UserModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace BodyPosition.MVVM.View
{
    /// <summary>
    /// Interaction logic for TestSelectionView.xaml
    /// </summary>
    public partial class TestSelectionView : Page
    {
        private List<TestModel> test = new List<TestModel>();

        private TestModel testSelected;
        public TestModel TestSelected
        {
            get { return testSelected; }
            set { testSelected = value; }
        }

        private UserModel _userModel;
        public UserModel UserModel
        {
            get { return _userModel; }
            set { _userModel = value; }
        }

        private Dictionary<string, TestModel> _testSelectedByUserID = new Dictionary<string, TestModel>();

        private Dictionary<string, Dictionary<string, TestModel>> _testReadFile = new Dictionary<string, Dictionary<string, TestModel>>();

        private Dictionary<string, Dictionary<string, AngleModel>> _angleReadFile = new Dictionary<string, Dictionary<string, AngleModel>>();

        public TestSelectionView(UserModel user_model)
        {
            InitializeComponent();

            UserModel = user_model;

            _testReadFile = ReadTest();
            _angleReadFile = ReadAngle();

            LoadTest(UserModel);
            refreshTest();

        }
        private void Update(object sender, RoutedEventArgs e)
        {
            TestSelected = dgTest.SelectedItem as TestModel;
            if (TestSelected == null)
            {
                return;
            }

            TestModel nTest = new TestModel()
            {
                Id = TestSelected.Id,
                TestName = TestSelected.TestName,
                UserId = TestSelected.UserId,
                Date = TestSelected.Date,
                Time = TestSelected.Time
            };

            _testReadFile[UserModel.Id.ToString()].Remove(TestSelected.Id.ToString());
            _testReadFile[UserModel.Id.ToString()].Add(TestSelected.Id.ToString(), nTest);
            WriteTest(_testReadFile);

            MessageBox.Show("อัพเดตข้อมูลเสร็จสิ้น");
        }
        private void AddTest(object sender, RoutedEventArgs e)
        {
            DateTime currentTime = DateTime.Now;

            TestModel addTest = new TestModel()
            {
                Id = _testSelectedByUserID.Count + 1,
                TestName = UserModel.FirstName+"_"+UserModel.LastName+"_"+(_testSelectedByUserID.Count + 1),
                UserId = UserModel.Id,
                Date = currentTime.ToString("dd/MM/yyyy"),
                Time = currentTime.ToString("hh:mm tt")
            };

            _testReadFile[UserModel.Id.ToString()].Add(addTest.Id.ToString(),addTest);
            WriteTest(_testReadFile);

            AngleModel newAngle = new AngleModel() {
                Angle = new Dictionary<string, Angle>(),
            };
            _angleReadFile[UserModel.Id.ToString()].Add(addTest.Id.ToString(), newAngle);
            WriteAngle(_angleReadFile);

            test.Clear();

            _testSelectedByUserID.Clear();
            _testReadFile.Clear();
            _angleReadFile.Clear();

            _testReadFile = ReadTest();
            _angleReadFile = ReadAngle();

            LoadTest(UserModel);
            refreshTest();
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            TestSelected = dgTest.SelectedItem as TestModel;

            if(TestSelected == null)
            {
                return;
            }

            _testReadFile[UserModel.Id.ToString()].Remove(TestSelected.Id.ToString());
            WriteTest(_testReadFile);

            _angleReadFile[UserModel.Id.ToString()].Remove(TestSelected.Id.ToString());
            WriteAngle(_angleReadFile);

            test.Clear();

            _testSelectedByUserID.Clear();
            _testReadFile.Clear();
            _angleReadFile.Clear();

            _testReadFile = ReadTest();
            _angleReadFile = ReadAngle();

            LoadTest(UserModel);
            refreshTest();
        }
        private void Selected(object sender, RoutedEventArgs e)
        {
            TestSelected = dgTest.SelectedItem as TestModel;
            if (TestSelected == null)
            {
                return;
            }
            NavigationService.Navigate(new HomeView(UserModel, TestSelected));
        }
        private void LoadTest(UserModel userModel)
        {
            _testSelectedByUserID = _testReadFile[userModel.Id.ToString()];

            foreach (var ts in _testSelectedByUserID)
            {
                test.Add(_testSelectedByUserID[ts.Value.Id.ToString()]);
            }
        }
        private void refreshTest()
        {
            dgTest.ItemsSource = null;
            dgTest.ItemsSource = test;
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
        #region json method
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
