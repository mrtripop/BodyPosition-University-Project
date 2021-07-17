using BodyPosition.Core;

namespace BodyPosition.MVVM.ViewModel
{
    class UserManagementViewModel :ObservableObject
    {
        public CustomListViewModel CustomListVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public UserManagementViewModel()
        {
            CustomListVM = new CustomListViewModel();
            CurrentView = CustomListVM;
        }

    }
}
