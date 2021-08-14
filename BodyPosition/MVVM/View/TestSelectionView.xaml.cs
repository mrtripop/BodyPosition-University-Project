using BodyPosition.Core;
using BodyPosition.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
        public UserModel userModel
        {
            get { return _userModel; }
            set { _userModel = value; }
        }

        public TestModel addTest;


        public TestSelectionView(UserModel user_model)
        {
            InitializeComponent();

            userModel = user_model;

            LoadTest(userModel);
            refreshTest();
        }

        private void AddTest(object sender, RoutedEventArgs e)
        {
            DateTime currentTime = DateTime.Now;

            addTest = new TestModel();
            addTest.TestName = "test_id" + userModel.ID + "_" + (test.Count+1);
            addTest.Uid = userModel.ID;
            addTest.Date = currentTime.ToString("dd/MM/yyyy");
            addTest.Time = currentTime.ToString("hh:mm tt");

            SqliteDataAccess.AddTest(addTest);
            LoadTest(userModel);
            refreshTest();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            TestSelected = dgTest.SelectedItem as TestModel;

            SqliteDataAccess.DeleteTest(TestSelected);

            LoadTest(userModel);
            refreshTest();
        }

        private void Result(object sender, RoutedEventArgs e)
        {
            TestSelected = dgTest.SelectedItem as TestModel;

            DatabaseView dbv = new DatabaseView(userModel, TestSelected);
            dbv.ShowDialog();
        }

        private void Selected(object sender, RoutedEventArgs e)
        {
            TestSelected = dgTest.SelectedItem as TestModel;

            NavigationService.Navigate(new HomeView(userModel, TestSelected));
        }

        private void LoadTest(UserModel userModel)
        {
            test = SqliteDataAccess.GetTestByUID(userModel);
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
    }
}
