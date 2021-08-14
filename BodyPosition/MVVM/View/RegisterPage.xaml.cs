using BodyPosition.Core;
using BodyPosition.MVVM.Model;
using System.Windows;

namespace BodyPosition.MVVM.View
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {

        private string FirstName;
        private string LastName;
        private string Phone;
        private string Gender;
        private double WeightUser;
        private double HeightUSer;
        private string Date;
        private string Time;

        UserModel user;

        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            FirstName= firstName.Text;
            LastName = lastName.Text;
            Phone = phone.Text;
            Gender = gender.Text;
            if(weight.Text != "")
            {
                WeightUser = double.Parse(weight.Text);
            }
            if (height.Text != "")
            {
                HeightUSer = double.Parse(height.Text);
            }
            Date = date.Text;
            Time = time.Text;

            if( FirstName != "" && 
                LastName != "" && 
                Gender != "" && 
                Phone != "" && 
                weight.Text != "" && 
                height.Text != "" && 
                Date != "" && 
                Time != "")
            {
                user = new UserModel();

                user.FirstName = FirstName;
                user.LastName = LastName;
                user.Gender = Gender;
                user.Tel = Phone;
                user.Weight = WeightUser;
                user.Height = HeightUSer;
                user.Date = Date;
                user.Time = Time;

                SqliteDataAccess.SavePerson(user);
                MessageBox.Show("บันทึกสำเร็จ");
                this.Close();
            }
            else
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบทุกช่องก่อนทำการบันทึก");
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
