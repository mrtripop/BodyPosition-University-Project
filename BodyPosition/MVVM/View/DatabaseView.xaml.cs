using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Data.SQLite;
using LightBuzz.Vitruvius;
using BodyPosition.MVVM.Model;

namespace BodyPosition.MVVM.View
{
    /// <summary>
    /// Interaction logic for DatabaseView.xaml
    /// </summary>
    public partial class DatabaseView : Window
    {
        #region Parameter
        private UserModel _user;
        public UserModel User
        {
            get { return _user; }
            set { _user = value; }
        }

        private TestModel _test;
        public TestModel Test
        {
            get { return _test; }
            set { _test = value; }
        }

        #endregion

        #region Initialize
        public DatabaseView(UserModel user, TestModel test)
        {
            InitializeComponent();

            User = user;
            Test = test;

            userUID.Text = User.ID.ToString();
            userName.Text = User.FirstName + " " + User.LastName;
            testName.Text = Test.TestName;
        }

        #endregion

        #region Record and Play video
        private void playPauseVideo(object sender, RoutedEventArgs e)
        {

        }

        private void OpenRecording()
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Kinect Eventstream|*.xef|All files|*.*";
            ofd.Title = "Open a recording";

            if (ofd.ShowDialog() == true)
            {
                KinectManager.Instance.RecordingStopped += RecordingStopped;
                KinectManager.Instance.OpenRecording(ofd.FileName);

                //if (!IsRunning) IsRunning = true;
            }
        }
        private void RecordingStopped()
        {
            IsRunning = false;
        }

        private bool isRunning = false;
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }

            set
            {
                isRunning = value;
            }
        }

        #endregion

        #region Open data table
        private void openTable(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Export to Excel
        private void exportTable(object sender, RoutedEventArgs e)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            string cs = "URI=file: D:/Learning/Kinect Project/BodyPosition/BodyPosition/BodyPosition/Database.db";
            string data = String.Empty;

            int i = 0;
            int j = 0;

            xlWorkSheet.Cells[1, 1] = "ID";
            xlWorkSheet.Cells[1, 2] = "Time";
            xlWorkSheet.Cells[1, 3] = "Front Pelvis Angle";
            xlWorkSheet.Cells[1, 4] = "Right Shoulder";
            xlWorkSheet.Cells[1, 5] = "Left Shoulder";
            xlWorkSheet.Cells[1, 6] = "Pelvis Angle";
            xlWorkSheet.Cells[1, 7] = "Knee Angle";
            xlWorkSheet.Cells[1, 8] = "Ankle Angle";

            string fileName = Test.TestName+".xls";

            using (SQLiteConnection con = new SQLiteConnection(cs))
            {
                con.Open();

                string stm = "SELECT * FROM "+Test.TestName;

                using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read()) // Reading Rows
                        {
                            for (j = 0; j <= rdr.FieldCount - 1; j++) // Looping throw colums
                            {
                                data = rdr.GetValue(j).ToString();
                                xlWorkSheet.Cells[i + 2, j + 1] = data;  
                            }
                            i++;
                        }
                    }
                }
                con.Close();
            }
            xlWorkBook.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
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
