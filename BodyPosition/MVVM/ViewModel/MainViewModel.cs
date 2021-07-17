using BodyPosition.Core;

namespace BodyPosition.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DatabaseViewCommand { get; set; }
        public RelayCommand UserManagementViewCommand { get; set; }


        public HomeViewModel HomeVM { get; set; }
        public DatabaseViewModel DatabaseVM { get; set; }
        public UserManagementViewModel UserManagementVM { get; set; }

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

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            DatabaseVM = new DatabaseViewModel();
            UserManagementVM = new UserManagementViewModel();

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            DatabaseViewCommand = new RelayCommand(o =>
            {
                CurrentView = DatabaseVM;
            });

            UserManagementViewCommand = new RelayCommand(o =>
            {
                CurrentView = UserManagementVM;
            });
        }
    }
}
