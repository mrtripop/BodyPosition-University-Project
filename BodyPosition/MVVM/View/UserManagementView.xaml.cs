using BodyPosition.Core;
using BodyPosition.MVVM.Model;
using BodyPosition.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace BodyPosition.MVVM.View
{
    /// <summary>
    /// Interaction logic for UserManagementView.xaml
    /// </summary>
    public partial class UserManagementView : UserControl
    {
        private PersonModel person = new PersonModel();

        public UserManagementView()
        {
            InitializeComponent();

            //person = ...

        }

        private void AddPerson(object sender, RoutedEventArgs e)
        {
            // step follow
            // 1. show window popup
            // 2. map data from popup to PersonModel
            // 3. query insert into Person table

            SqliteDataAccess.SavePerson(person);
        }

        private void EditPerson(object sender, RoutedEventArgs e)
        {
            //SqliteDataAccess.EditPerson();
        }

        private void DeletePerson(object sender, RoutedEventArgs e)
        {
            SqliteDataAccess.DeletePerson(person.id);
        }
    }
}
