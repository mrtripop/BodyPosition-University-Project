
using BodyPosition.Core;
using BodyPosition.MVVM.Model.TestModel;
using BodyPosition.MVVM.Model.UserModel;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Excel = Microsoft.Office.Interop.Excel;

namespace BodyPosition.MVVM.View
{
    /// <summary>
    /// Interaction logic for UserSelectionView.xaml
    /// </summary>
    public partial class UserSelectionView : Page
    {
        #region Parameter
        public List<UserModel> users = new List<UserModel>();
        private UserModel selectedUser;
        public UserModel UserSelected
        {
            get { return selectedUser; }
            set { selectedUser = value; }
        }

        private Dictionary<string, UserModel> _userReadFile = new Dictionary<string, UserModel>();
        private Dictionary<string, Dictionary<string, TestModel>> _testReadFile = new Dictionary<string, Dictionary<string, TestModel>>();

        private Database dbUserManager;
        private Database dbTestManager;

        private Database dbUserBackup;
        private Database dbTestBackup;

        public readonly string PATH_USER = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\UserJson.json");
        public readonly string PATH_TEST = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\TestJson.json");

        public readonly string PATH_USER_BACKUP = Path.Combine(Environment.CurrentDirectory, @"BackupDatabase\UserJson.json");
        public readonly string PATH_TEST_BACKUP = Path.Combine(Environment.CurrentDirectory, @"BackupDatabase\TestJson.json");

        #endregion

        #region Initialize
        public UserSelectionView()
        {
            InitializeComponent();

            dbUserManager = new Database(PATH_USER);
            dbTestManager = new Database(PATH_TEST);

            dbUserBackup = new Database(PATH_USER_BACKUP);
            dbTestBackup = new Database(PATH_TEST_BACKUP);

            _userReadFile = dbUserManager.ReadUser();
            _testReadFile = dbTestManager.ReadTest();

            LoadUser();
            refreshList();

            this.DataContext = this;
        }
        #endregion

        #region Method
        private void refreshList()
        {
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource = users;
        }
        private void LoadUser()
        {
            foreach (var i in _userReadFile)
            {
                users.Add(i.Value);
            }
        }
        #endregion

        #region Button Event Method
        private void Selected(object sender, RoutedEventArgs e)
        {
            UserSelected = dgUsers.SelectedItem as UserModel;
            if (UserSelected == null)
            {
                return;
            }
            NavigationService.Navigate(new TestSelectionView(UserSelected));
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            // ตั้งตัวแปรรับค่าที่เลือกมาเป็น UserModel
            UserSelected = dgUsers.SelectedItem as UserModel;
            if (UserSelected == null)
            {
                return;
            }

            // reload test
            _testReadFile.Clear();
            _testReadFile = dbTestManager.ReadTest();

            // remove user
            users.Remove(UserSelected);

            // delete user
            _userReadFile.Remove(UserSelected.Id.ToString());
            dbUserManager.WriteJson(_userReadFile);

            dbUserBackup.WriteJson(_userReadFile);

            // delete angle file (delete file from folder)
            foreach (var item in _testReadFile[UserSelected.Id.ToString()].Values)
            {
                string PATH_ANGLE_DELETE = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\Angle\" + item.TestName + ".json");
                string PATH_ANGLE_BACKUP_DELETE = Path.Combine(Environment.CurrentDirectory, @"BackupDatabase\Angle\" + item.TestName + ".json");

                // delete angle
                if (File.Exists(PATH_ANGLE_DELETE))
                {
                    File.Delete(PATH_ANGLE_DELETE);
                }

                if (File.Exists(PATH_ANGLE_BACKUP_DELETE))
                {
                    File.Delete(PATH_ANGLE_BACKUP_DELETE);
                }
            }

            // delete test
            _testReadFile.Remove(UserSelected.Id.ToString());
            dbTestManager.WriteJson(_testReadFile);

            dbTestBackup.WriteJson(_testReadFile);

            // clear value
            users.Clear();
            _userReadFile.Clear();
            _testReadFile.Clear();

            // reload value
            _userReadFile = dbUserManager.ReadUser();

            LoadUser();
            refreshList();
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            UserSelected = dgUsers.SelectedItem as UserModel;
            if (UserSelected == null)
            {
                return;
            }

            UserDetialView udt = new UserDetialView(UserSelected);
            udt.ShowDialog();

            users.Clear();
            _userReadFile.Clear();

            _userReadFile = dbUserManager.ReadUser();
            LoadUser();
            refreshList();

        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            RegisterPage regis = new RegisterPage();
            regis.ShowDialog();

            users.Clear();

            _userReadFile.Clear();

            _userReadFile = dbUserManager.ReadUser();

            LoadUser();
            refreshList();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text != "")
            {
                int b = int.Parse(searchTextBox.Text);

                List<UserModel> searchUser = new List<UserModel>();

                foreach (var index in _userReadFile)
                {
                    if (index.Value.Id == b)
                    {
                        searchUser.Add(_userReadFile[b.ToString()]);
                    }
                }

                dgUsers.ItemsSource = null;
                dgUsers.ItemsSource = searchUser;

                searchTextBox.Clear();

            }
            else
            {
                users.Clear();
                _userReadFile.Clear();

                _userReadFile = dbUserManager.ReadUser();

                LoadUser();
                refreshList();

                searchTextBox.Clear();
            }
        }

        private void ExportUser(object sender, RoutedEventArgs e)
        {
            exportUserButton.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x66));
            exportUserButton.Content = "Exporting...";

            var sfd = new SaveFileDialog();
            sfd.Filter = "Excel|*.xls|All files|*.*";
            sfd.Title = "Export Member";
            sfd.FileName = "Member";

            if (sfd.ShowDialog() == true)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                string data = String.Empty;

                int i = 0;
                int j = 0;

                xlWorkSheet.Cells[1, 1] = "UserID";
                xlWorkSheet.Cells[1, 2] = "Firstname";
                xlWorkSheet.Cells[1, 3] = "Lastname";
                xlWorkSheet.Cells[1, 4] = "Gender";
                xlWorkSheet.Cells[1, 5] = "Age";
                xlWorkSheet.Cells[1, 6] = "Weight";
                xlWorkSheet.Cells[1, 7] = "Height";
                xlWorkSheet.Cells[1, 8] = "Phone";
                xlWorkSheet.Cells[1, 9] = "Date";
                xlWorkSheet.Cells[1, 10] = "Time";

                foreach (var row in _userReadFile.Values)
                {
                    xlWorkSheet.Cells[i + 2, j + 1] = row.Id;
                    xlWorkSheet.Cells[i + 2, j + 2] = row.FirstName;
                    xlWorkSheet.Cells[i + 2, j + 3] = row.LastName;
                    xlWorkSheet.Cells[i + 2, j + 4] = row.Gender;
                    xlWorkSheet.Cells[i + 2, j + 5] = row.Age;
                    xlWorkSheet.Cells[i + 2, j + 6] = row.Weight;
                    xlWorkSheet.Cells[i + 2, j + 7] = row.Height;
                    xlWorkSheet.Cells[i + 2, j + 8] = row.Tel;
                    xlWorkSheet.Cells[i + 2, j + 9] = row.Date;
                    xlWorkSheet.Cells[i + 2, j + 10] = row.Time;

                    i++;
                }

                xlWorkBook.SaveAs(sfd.FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                MessageBox.Show("นำออกเป็น Excel ไฟล์สำเร็จ สามารถตรวจได้ที่ " + sfd.FileName);

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
            }
            exportUserButton.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x21, 0x96, 0xf3));
            exportUserButton.Content = "Export";
        }

        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
        #endregion



    }
}
