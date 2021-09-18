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
        #region Constant

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

        private string PATH_TEST = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\TestJson.json");
        private string PATH_ANGLE;

        private Database dbTestManager;
        private Database dbAngleManager;

        #endregion

        public TestSelectionView(UserModel user_model)
        {
            InitializeComponent();

            UserModel = user_model;
            dbTestManager = new Database(PATH_TEST);

            _testReadFile = dbTestManager.ReadTest();

            LoadTest(UserModel);
            refreshTest();

        }
        

        #region Button Event Method

        private void Update(object sender, RoutedEventArgs e)
        {
            TestSelected = dgTest.SelectedItem as TestModel;
            if (TestSelected == null)
            {
                return;
            }

            // create new instance
            TestModel nTest = new TestModel()
            {
                Id = TestSelected.Id,
                TestName = TestSelected.TestName,
                UserId = TestSelected.UserId,
                Date = TestSelected.Date,
                Time = TestSelected.Time
            };

            // create path from TestModel info
            PATH_ANGLE = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\Angle\" + TestSelected.TestName + ".json");
            dbAngleManager = new Database(PATH_ANGLE);

            // read old json data & update to new json
            AngleModel angleJson = dbAngleManager.ReadAngle();
            angleJson.TestId = nTest.Id;
            angleJson.UserId = nTest.UserId;
            angleJson.TestName = nTest.TestName;

            // update TestJson file
            _testReadFile[UserModel.Id.ToString()].Remove(TestSelected.Id.ToString());
            _testReadFile[UserModel.Id.ToString()].Add(TestSelected.Id.ToString(), nTest);
            dbTestManager.WriteJson(_testReadFile);

            // update angle file
            dbAngleManager.WriteJson(angleJson);
            MessageBox.Show("อัพเดตข้อมูลเสร็จสิ้น");
        }

        private void Insert(object sender, RoutedEventArgs e)
        {
            // calculate value
            DateTime currentTime = DateTime.Now;
            List<int> fMax = new List<int>();
            foreach (var index in _testSelectedByUserID)
            {
                fMax.Add(index.Value.Id);
            }

            int max = 0;
            if (fMax.Count > 0)
            {
                max = FindMax(fMax);
            }

            // map to TestModel
            TestModel addTest = new TestModel()
            {
                Id = max + 1,
                TestName = UserModel.FirstName+"_"+UserModel.LastName+"_"+(max + 1),
                UserId = UserModel.Id,
                Date = currentTime.ToString("dd/MM/yyyy"),
                Time = currentTime.ToString("HH:mm")
            };

            // create path from TestModel info
            string PATH_ANGLE_INSERT = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\Angle\" + addTest.TestName+".json");
            dbAngleManager = new Database(PATH_ANGLE_INSERT);

            // write test file
            _testReadFile[UserModel.Id.ToString()].Add(addTest.Id.ToString(),addTest);
            dbTestManager.WriteJson(_testReadFile);

            // create angle file by TestModel info
            AngleModel newAngle = new AngleModel()
            {
                UserId = UserModel.Id,
                TestId = addTest.Id,
                TestName = addTest.TestName,
                Angle = new Dictionary<string, Angle>(),
            };

            // write angle file
            dbAngleManager.WriteJson(newAngle);

            // clear value
            test.Clear();

            _testSelectedByUserID.Clear();
            _testReadFile.Clear();

            // reload value
            _testReadFile = dbTestManager.ReadTest();

            // update ui
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

            // delete test
            _testReadFile[UserModel.Id.ToString()].Remove(TestSelected.Id.ToString());
            dbTestManager.WriteJson(_testReadFile);

            string PATH_ANGLE_DELETE = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\Angle\" + TestSelected.TestName + ".json");

            // delete angle
            if (File.Exists(PATH_ANGLE_DELETE))
            {   
                File.Delete(PATH_ANGLE_DELETE);
            }

            // clear value
            test.Clear();

            _testSelectedByUserID.Clear();
            _testReadFile.Clear();

            // reload value
            _testReadFile = dbTestManager.ReadTest();

            // update ui
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

            string PATH_ANGLE_SELECT = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\Angle\"+TestSelected.TestName+".json");
            NavigationService.Navigate(new HomeView(UserModel, TestSelected , PATH_ANGLE_SELECT));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        #endregion

        #region Method
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

        private int FindMax(List<int> list)
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

        #endregion

    }
}
