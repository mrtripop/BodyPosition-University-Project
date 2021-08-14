using BodyPosition.Core;
using BodyPosition.MVVM.Model;
using System.Collections.Generic;
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
        public List<UserModel> users = new List<UserModel>();
        private UserModel selectedUser;

        public UserModel UserSelected
        {
            get { return selectedUser; }
            set { selectedUser = value; }
        }

        public UserSelectionView()
        {
            InitializeComponent();

            LoadUser();
            refreshList();

            this.DataContext = this;
        }

        private void Selected(object sender, RoutedEventArgs e)
        {
            // ตั้งตัวแปรรับค่าที่เลือกมาเป็น UserModel
            UserSelected = dgUsers.SelectedItem as UserModel;

            NavigationService.Navigate(new TestSelectionView(UserSelected));
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            // ตั้งตัวแปรรับค่าที่เลือกมาเป็น UserModel
            UserSelected = dgUsers.SelectedItem as UserModel;

            SqliteDataAccess.DeletePerson(UserSelected);

            LoadUser();
            refreshList();
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            RegisterPage regis = new RegisterPage();
            regis.ShowDialog();

            LoadUser();
            refreshList();
        }

        private void refreshList()
        {
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource = users;
        }

        private void LoadUser()
        {
            users = SqliteDataAccess.LoadPeople();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            if(searchTextBox.Text != "")
            {
                int search = int.Parse(searchTextBox.Text);
                users = SqliteDataAccess.LoadPeopleByID(search);
            }
            else
            {
                LoadUser();
            }

            refreshList();

        }
    }
}
