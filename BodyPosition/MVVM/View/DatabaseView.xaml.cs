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
        public AngleModel AngleModelDB
        {
            get { return _angleModelDB; }
            set { _angleModelDB = value; }
        }

        private List<Angle> angle = new List<Angle>();

        private Dictionary<string, Dictionary<string, AngleModel>> _angleReadFile = new Dictionary<string, Dictionary<string, AngleModel>>();

        private Dictionary<string, Dictionary<string, AngleModel>> exportData = new Dictionary<string, Dictionary<string, AngleModel>>();
        #endregion

        #region Initialize
        public DatabaseView(UserModel user, TestModel test)
        {
            InitializeComponent();

            UserModelDB = user;
            TestModelDB = test;

            _angleReadFile = ReadAngle();
            exportData = _angleReadFile;

            LoadAngle();
            refreshDataGrid();

        }

        #endregion

        #region Open data table
        private void SearchData(object sender, RoutedEventArgs e)
        {
            if (searchTextBlock.Text != "")
            {
                List<Angle> searchAngle = new List<Angle>();
                string EventIndex = searchTextBlock.Text.ToString();
                foreach (var dic in AngleModelDB.Angle)
                {
                    if (dic.Key == EventIndex)
                    {
                        searchAngle.Add(AngleModelDB.Angle[EventIndex]);
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
                }
                else
                {
                    angle.Clear();
                    _angleReadFile.Clear();

                    tblPelvisFront.Text = "";
                    tblShoulderRight.Text = "";
                    tblShoulderLeft.Text = "";
                    tblPelvisSideAngle.Text = "";
                    tblKneeAngle.Text = "";
                    tblAnkleAngle.Text = "";

                    _angleReadFile = ReadAngle();
                    MessageBox.Show("ไม่พบเลข ID ที่คุณต้องการ");

                    LoadAngle();
                    refreshDataGrid();
                }
            }
            else
            {
                angle.Clear();
                _angleReadFile.Clear();

                tblPelvisFront.Text = "";
                tblShoulderRight.Text = "";
                tblShoulderLeft.Text = "";
                tblPelvisSideAngle.Text = "";
                tblKneeAngle.Text = "";
                tblAnkleAngle.Text = "";

                _angleReadFile = ReadAngle();

                LoadAngle();
                refreshDataGrid();
            }
        }
        private void LoadAngle()
        {
            _angleModelDB = _angleReadFile[UserModelDB.Id.ToString()][TestModelDB.Id.ToString()];

            foreach (var dic in AngleModelDB.Angle)
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

        #region Export to Excel

        private void exportTable(object sender, RoutedEventArgs e)
        {
            exportButton.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x66));
            exportButton.Content = "Exporting...";

            var sfd = new SaveFileDialog();
            sfd.Filter = "Excel|*.xls|All files|*.*";
            sfd.Title = "Export Angle "+TestModelDB.TestName;
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

                foreach (var row in exportData[UserModelDB.Id.ToString()][TestModelDB.Id.ToString()].Angle)
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

                MessageBox.Show("นำออกเป็น Excel ไฟล์สำเร็จ สามารถตรวจได้ที่ Document โฟลเดอร์");

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
        #endregion

        #region Json Method
        private Dictionary<string, Dictionary<string, AngleModel>> ReadAngle()
        {
            JObject json = JObject.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\AngleJson.json")));
            var angleModel = AngleModel.FromJson(json.ToString());
            return angleModel;
        }
        private void WriteAngle(Dictionary<string, Dictionary<string, AngleModel>> angle)
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\AngleJson.json"), angle.ToJson());
        }

        #endregion

        #region Read Excel
        private void ImportTable(object sender, RoutedEventArgs e)
        {
            exportButton.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x66));
            exportButton.Content = "Importing...";

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
                        DataTable dtExcel = new DataTable();
                        dtExcel = ReadExcel(filePath, fileExt);

                        // push to Angle Model
                        Angle newAngle = new Angle()
                        {
                            //Id = counter,
                            //FrontPelvis = anglePelvisFront.Angle,
                            //RightShoulder = durationRight,
                            //LeftShoulder = durationLeft,
                            //Pelvis = angleSidePelvis.Angle,
                            //Knee = angleKnee.Angle,
                            //Ankle = angleAnkle.Angle
                        };

                        //_angleReadFile[UserModelDB.Id.ToString()][TestModelDB.Id.ToString()].Angle.Add(counter.ToString(), newAngle);

                        // save to json

                        // update view by json data

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

            exportButton.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x21, 0x96, 0xf3));
            exportButton.Content = "Export";
        }
        private IEnumerable<Angle> ConvertToAngle(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new Angle
                {
                    Id = Convert.ToInt32(row["Event Index"]),
                    FrontPelvis = Convert.ToDouble(row["Front Pelvis Angle"]),
                    LeftShoulder = Convert.ToDouble(row["Left Shoulder"]),
                    RightShoulder = Convert.ToDouble(row["Right Shoulder"]),
                    Pelvis = Convert.ToDouble(row["Pelvis Angle"]),
                    Ankle = Convert.ToDouble(row["Knee Angle"]),
                    Knee = Convert.ToDouble(row["Ankle Angle"])
                };
            }
        }

        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Sheet1$]", con); //here we read data from sheet1  
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                catch { }
            }
            return dtexcel;
        }

        #endregion
    }
}
