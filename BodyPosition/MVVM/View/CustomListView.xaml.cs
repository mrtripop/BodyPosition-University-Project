using BodyPosition.Core;
using BodyPosition.MVVM.Model;
using BodyPosition.MVVM.ViewModel;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BodyPosition.MVVM.View
{
    /// <summary>
    /// Interaction logic for CustomList.xaml
    /// </summary>
    public partial class CustomListView : UserControl
    {
        public CustomListView()
        {
            InitializeComponent();

            DataContext = SqliteDataAccess.LoadPeopleCustomListView();
        }
    }
}
