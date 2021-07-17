using BodyPosition.MVVM.Model;
using BodyPosition.MVVM.ViewModel;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyPosition.Core
{
    class SqliteDataAccess
    {

        private static string LoadConnetionString()
        {
            return "URI=file: D:/Learning/Kinect Project/BodyPosition/BodyPosition/BodyPosition/Database.db";
        }

        // beam requirement UID 4 หลัก 0001  //////////////////////////////


        // Get all person
        public static List<PersonModel> LoadPeople()
        {
            string sql = "select * from Person";

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                var output = cnn.Query<PersonModel>(sql, new DynamicParameters());
                return output.ToList();
            }
        }

        public static List<CustomListViewModel> LoadPeopleCustomListView()
        {
            string sql = "select * from Person";

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                var output = cnn.Query<CustomListViewModel>(sql, new DynamicParameters());
                return output.ToList();
            }
        }

        // Get person by Name
        public static List<PersonModel> LoadPeopleByName(string name)
        {
            string sql = "select * from Person where name="+name;

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                var output = cnn.Query<PersonModel>(sql, new DynamicParameters());
                return output.ToList();
            }
        }

        // Get person by ID
        public static List<PersonModel> LoadPeopleByID(int id)
        {
            string sql = "select * from Person where id=" + id;

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                var output = cnn.Query<PersonModel>(sql, new DynamicParameters());
                return output.ToList();
            }
        }

        // Add new person
        public static void SavePerson(PersonModel person)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                cnn.Execute("insert into Person (name, gender, tel, weight, height, date) values (@name, @gender, @tel, @weight, @height, @date)", person);
            }
        }

        // Edit person information
        //public static void EditPerson(int id)
        //{
        //    string sql = "delete from Person where id =" + id;

        //    using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
        //    {
        //        cnn.Execute(sql);
        //    }
        //}

        // Delete person
        public static void DeletePerson(int uid)
        {
            string sql = "delete from Person where id =" + uid;

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                cnn.Execute(sql);
            }
        }

        /// <summary>
        /// GetAllAngle ดึงข้อมูลมุมต่างๆของตาราง โดยใช้พารามิเตอร์เป็นชื่อตาราง
        /// </summary>
        /// <param name="table_name"></param>
        public static void GetAllAngle(string table_name)
        {
            string sql = "select * from " + table_name;

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                cnn.Execute(sql);
            }
        }

        // Add angle 
        //: no edit, delete because delete old scan and renew scan instead.
        /// <summary>
        /// AddAngle ใช้เพื่อเพิ่มข้อมูลของมุมเข้าสู่ฐานข้อมูล โดยใช้พารามิเตอร์เป็น AngleModel
        /// </summary>
        /// <param name="angle"></param>
        public static void AddAngle(AngleModel angle, string table_name)
        {
            string sql = "insert into " + table_name+ " (pelvis, knee, ankle) values (@pelvis, @knee, @ankle) ";

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                cnn.Execute(sql, angle);
            }
        }

        // Test table
        // Create table by person id
        /// <summary>
        /// CreateTestTableByPersonID ใช้เพื่อสร้างตารางใหม่ ในการบันทึกข้อมูลของ Angle 
        /// </summary>
        /// <param name="person_id"></param>
        public static void CreateTestTableByPersonID(int person_id)
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection(LoadConnetionString());
            m_dbConnection.Open();

            string sql = "create table test_" + person_id + " (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, time REAL NOT NULL, pelvis REAL NOT NULL, knee REAL NOT NULL, ankle REAL NOT NULL)";

            //string sql = "create table highscores (name varchar(20), score int)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);

            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        // Get person test by person id
        // Return from Test table ---> List of AngleModel
        public static List<AngleModel> GetPersonTestByName(string test_name)
        {
            string sql = "select * from " + test_name;

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                var output = cnn.Query<AngleModel>(sql, new DynamicParameters());
                return output.ToList();
            }
        }

        // Get person test by test id
        public static void GetPersonTestByID()
        {

        }
    }
}
