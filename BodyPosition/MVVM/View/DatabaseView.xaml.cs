using BodyPosition.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System;
using System.Data.SQLite;
using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using System.Windows.Media;

namespace BodyPosition.MVVM.View
{
    /// <summary>
    /// Interaction logic for DatabaseView.xaml
    /// </summary>
    public partial class DatabaseView : UserControl
    {
 
        readonly string FOLDER_PATH = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "video");

        VitruviusPlayer _player;

        public DatabaseView()
        {
            InitializeComponent();
            //DataContext = new SelectViewModel();
        }

        private void OpenUserDataTest(object sender, RoutedEventArgs e)
        {

        }

        private void playPauseVideo(object sender, RoutedEventArgs e)
        {
            _player = new VitruviusPlayer();

            if (_player.IsPlaying)
            {
                _player.FrameArrived -= Player_FrameArrived;
                _player.Stop();
            }
            else
            {
                _player.Folder = FOLDER_PATH;
                _player.FrameArrived += Player_FrameArrived;
                _player.Start();
            }
        }

        private void Player_FrameArrived(object sender, VitruviusFrame frame)
        {
            if (frame != null)
            {
                viewer.Image = frame.Image.ToBitmap(frame.Visualization, PixelFormats.Bgr32);
                viewer.DrawBody(frame.Body);
            }
        }

        private void openTable(object sender, RoutedEventArgs e)
        {

        }

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
            xlWorkSheet.Cells[1, 2] = "Name";
            xlWorkSheet.Cells[1, 3] = "Gender";
            xlWorkSheet.Cells[1, 4] = "Phone";
            xlWorkSheet.Cells[1, 5] = "Weight";
            xlWorkSheet.Cells[1, 6] = "Height";
            xlWorkSheet.Cells[1, 7] = "Date";

            // เปลี่ยนชื่อไฟล์เป็นชื่อตารางการทดลองแต่ละครั้ง
            string fileName = "sqliteToExcel.xls";

            using (SQLiteConnection con = new SQLiteConnection(cs))
            {
                con.Open();

                // เปลี่ยน Person เป็นตารางการทดลองแต่ละครั้ง
                string stm = "SELECT * FROM Person";

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
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

        
    }
}
