using BodyPosition.MVVM.View;
using System.Windows;

namespace BodyPosition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new UserSelectionView());
        }
    }
}
