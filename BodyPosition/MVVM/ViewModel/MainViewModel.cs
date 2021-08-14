using BodyPosition.Core;
using BodyPosition.MVVM.Model;
using System;

namespace BodyPosition.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand UserSelectionViewCommand { get; set; }
        public UserSelectionViewModel UserSelectionVM { get; set; }

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
            UserSelectionVM = new UserSelectionViewModel();

            CurrentView = UserSelectionVM;

            UserSelectionViewCommand = new RelayCommand(o =>
            {
                CurrentView = UserSelectionVM;
            });
        }
    }
}
