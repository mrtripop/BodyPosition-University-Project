using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using BodyPosition.MVVM.Model.UserModel;
using BodyPosition.MVVM.Model.TestModel;
using BodyPosition.MVVM.Model.AngleModel;
using Newtonsoft.Json.Linq;
using System.IO;

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

            string fileName = TestModelDB.TestName + ".xls";

            foreach(var row in exportData[UserModelDB.Id.ToString()][TestModelDB.Id.ToString()].Angle)
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

            xlWorkBook.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            MessageBox.Show("นำออกเป็น Excel ไฟล์สำเร็จ สามารถตรวจได้ที่ Document โฟลเดอร์");

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

        #region Json Method
        private Dictionary<string, Dictionary<string, AngleModel>> ReadAngle()
        {
            JObject json = JObject.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\AngleJson.json")));
            var angleModel = AngleModel.FromJson(json.ToString());
            return angleModel;
        }
        #endregion

    }
}
