using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using BodyPosition.MVVM.Model.UserModel;
using BodyPosition.MVVM.Model.TestModel;
using BodyPosition.MVVM.Model.AngleModel;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Media;
using Microsoft.Win32;
using System.Data;
using System.Data.OleDb;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using BodyPosition.Core;

namespace BodyPosition.MVVM.View
{
    /// <summary>
    /// Interaction logic for DatabaseView.xaml
    /// </summary>
    public partial class DatabaseView : Window
    {
        #region Parameter

        private UserModel _userModelDB;
        public UserModel UserModelDB
        {
            get { return _userModelDB; }
            set { _userModelDB = value; }
        }

        private TestModel _testModelDB;
        public TestModel TestModelDB
        {
            get { return _testModelDB; }
            set { _testModelDB = value; }
        }

        private AngleModel _angleModelDB;
        private AngleModel _angleBackupReader;

        private List<Angle> angle = new List<Angle>();

        private readonly string PATH_TEST_SELECTED;
        private readonly string PATH_TEST_BACKUP;

        private Database dbAngleMAnager;
        private Database dbAngleBackup;

        private int count = 0;

        #endregion

        public DatabaseView(UserModel user, TestModel test, AngleModel angleList, string path_test_selected, string path_test_backup)
        {
            InitializeComponent();

            UserModelDB = user;
            TestModelDB = test;

            PATH_TEST_SELECTED = path_test_selected;
            PATH_TEST_BACKUP = path_test_backup;

            dbAngleMAnager = new Database(PATH_TEST_SELECTED);
            dbAngleBackup = new Database(PATH_TEST_BACKUP);

            _angleModelDB = angleList;

            count = angleList.Angle.Count;

            LoadAngle();
            refreshDataGrid();

        }

        #region Search & Load data
        private void SearchData(object sender, RoutedEventArgs e)
        {
            if (searchTextBlock.Text != "")
            {
                List<Angle> searchAngle = new List<Angle>();
                string EventIndex = searchTextBlock.Text.ToString();

                foreach (var dic in _angleModelDB.Angle)
                {
                    if (dic.Key == EventIndex)
                    {
                        searchAngle.Add(_angleModelDB.Angle[EventIndex]);
                    }
                }

                if (searchAngle.Count > 0)
                {
                    DGAngle.ItemsSource = null;
                    DGAngle.ItemsSource = searchAngle;

                    tblPelvisFront.Text = searchAngle[0].FrontPelvis.ToString();
                    tblShoulderRight.Text = searchAngle[0].RightShoulder.ToString();
                    tblShoulderLeft.Text = searchAngle[0].LeftShoulder.ToString();
                    tblPelvisSideAngle.Text = searchAngle[0].Pelvis.ToString();
                    tblKneeAngle.Text = searchAngle[0].Knee.ToString();
                    tblAnkleAngle.Text = searchAngle[0].Ankle.ToString();

                    searchTextBlock.Clear();
                }
                else
                {
                    angle.Clear();

                    tblPelvisFront.Text = "";
                    tblShoulderRight.Text = "";
                    tblShoulderLeft.Text = "";
                    tblPelvisSideAngle.Text = "";
                    tblKneeAngle.Text = "";
                    tblAnkleAngle.Text = "";

                    MessageBox.Show("ไม่พบเลข ID ที่คุณต้องการ");

                    LoadAngle();
                    refreshDataGrid();

                    searchTextBlock.Clear();
                }
            }
            else
            {
                angle.Clear();

                tblPelvisFront.Text = "";
                tblShoulderRight.Text = "";
                tblShoulderLeft.Text = "";
                tblPelvisSideAngle.Text = "";
                tblKneeAngle.Text = "";
                tblAnkleAngle.Text = "";

                LoadAngle();
                refreshDataGrid();

                searchTextBlock.Clear();
            }
        }

        private void LoadAngle()
        {
            foreach (var dic in _angleModelDB.Angle)
            {
                angle.Add(dic.Value);
            }
        }

        private void refreshDataGrid()
        {
            DGAngle.ItemsSource = null;
            DGAngle.ItemsSource = angle;
        }

        #endregion

        #region Export & Import Excel

        private void exportTable(object sender, RoutedEventArgs e)
        {
            exportButton.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x66));
            exportButton.Content = "Exporting...";

            var sfd = new SaveFileDialog();
            sfd.Filter = "Excel|*.xls|All files|*.*";
            sfd.Title = "Export Angle " + TestModelDB.TestName;
            sfd.FileName = TestModelDB.TestName;

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

                xlWorkSheet.Cells[1, 1] = "Event ID";
                xlWorkSheet.Cells[1, 2] = "Front Pelvis Angle";
                xlWorkSheet.Cells[1, 3] = "Right Shoulder";
                xlWorkSheet.Cells[1, 4] = "Left Shoulder";
                xlWorkSheet.Cells[1, 5] = "Pelvis Angle";
                xlWorkSheet.Cells[1, 6] = "Knee Angle";
                xlWorkSheet.Cells[1, 7] = "Ankle Angle";

                foreach (var row in _angleModelDB.Angle)
                {
                    xlWorkSheet.Cells[i + 2, j + 1] = row.Value.Id;
                    xlWorkSheet.Cells[i + 2, j + 2] = row.Value.FrontPelvis;
                    xlWorkSheet.Cells[i + 2, j + 3] = row.Value.RightShoulder;
                    xlWorkSheet.Cells[i + 2, j + 4] = row.Value.LeftShoulder;
                    xlWorkSheet.Cells[i + 2, j + 5] = row.Value.Pelvis;
                    xlWorkSheet.Cells[i + 2, j + 6] = row.Value.Knee;
                    xlWorkSheet.Cells[i + 2, j + 7] = row.Value.Ankle;
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
            exportButton.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x21, 0x96, 0xf3));
            exportButton.Content = "Export";
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

        private void ImportTable(object sender, RoutedEventArgs e)
        {
            if(count > 0)
            {
                MessageBox.Show("มีข้อมูลอยู่ภายในระบบแล้ว");
                return;
            }

            importButton.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x66));
            importButton.Content = "Importing...";

            string filePath = string.Empty;
            string fileExt = string.Empty;

            OpenFileDialog file = new OpenFileDialog(); ;
            file.Filter = "Excel|*.xls|All files|*.*";
            file.Title = "Import Angle";

            if (file.ShowDialog() == true)
            {
                filePath = file.FileName;
                fileExt = Path.GetExtension(filePath);
                if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                {
                    try
                    {
                        // read data
                        Excel.Application xlApp = new Excel.Application();
                        Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filePath);
                        Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                        Excel.Range xlRange = xlWorksheet.UsedRange;

                        int rowCount = xlRange.Rows.Count;
                        int colCount = xlRange.Columns.Count;

                        // map to AngleModel
                        for (int i = 2; i <= rowCount; i++)
                        {
                            Angle nAngle = new Angle()
                            {
                                Id = (int)xlRange.Cells[i, 1].Value2,
                                FrontPelvis = (double)xlRange.Cells[i, 2].Value2,
                                LeftShoulder = (double)xlRange.Cells[i, 4].Value2,
                                RightShoulder = (double)xlRange.Cells[i, 3].Value2,
                                Pelvis = (double)xlRange.Cells[i, 5].Value2,
                                Ankle = (double)xlRange.Cells[i, 6].Value2,
                                Knee = (double)xlRange.Cells[i, 7].Value2,
                            };

                            _angleModelDB.Angle.Add(nAngle.Id.ToString(), nAngle);
                        }

                        //cleanup
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        Marshal.ReleaseComObject(xlRange);
                        Marshal.ReleaseComObject(xlWorksheet);

                        //close and release
                        xlWorkbook.Close();
                        Marshal.ReleaseComObject(xlWorkbook);

                        //quit and release
                        xlApp.Quit();
                        Marshal.ReleaseComObject(xlApp);

                        // save data to json
                        dbAngleMAnager.WriteJson(_angleModelDB);
                        dbAngleBackup.WriteJson(_angleModelDB);

                        MessageBox.Show("Import Complete!");

                        LoadAngle();
                        refreshDataGrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("กรุณาเลือกไฟล์ประเภท Excel");
                }
            }

            importButton.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x21, 0x96, 0xf3));
            importButton.Content = "Import";
        }

        #endregion

        private void RecoveryTable(object sender, RoutedEventArgs e)
        {
            if (count > 0)
            {
                MessageBox.Show("มีข้อมูลอยู่ภายในระบบแล้ว");
                return;
            }

            //read backup
            _angleBackupReader = dbAngleBackup.ReadAngle();

            count = _angleBackupReader.Angle.Count;
            if (count == 0)
            {
                MessageBox.Show("ไม่มีข้อมูลสำรอง");
                return;
            }

            //save backup to jsonDatabase
            dbAngleMAnager.WriteJson(_angleBackupReader);
            MessageBox.Show("Recovery Complete!");

            //display data
            _angleModelDB = _angleBackupReader;

            LoadAngle();
            refreshDataGrid();
        }
    }
}
