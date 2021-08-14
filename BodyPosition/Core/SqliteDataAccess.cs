using BodyPosition.MVVM.Model;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace BodyPosition.Core
{
    class SqliteDataAccess
    {

        private static string LoadConnetionString()
        {
            return "URI=file: D:/Learning/Kinect Project/BodyPosition/BodyPosition/BodyPosition/Database.db";
        }

        // Get all person
        public static List<UserModel> LoadPeople()
        {
            string sql = "select * from Person";

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                var output = cnn.Query<UserModel>(sql, new DynamicParameters());
                return output.ToList();
            }
        }

        // Get person by ID
        public static List<UserModel> LoadPeopleByID(int id)
        {
            string sql = "select * from Person where id=" + id;

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                var output = cnn.Query<UserModel>(sql, new DynamicParameters());
                return output.ToList();
            }
        }

        // Add new person
        public static void SavePerson(UserModel user)
        {
            string sql = "INSERT INTO Person (firstName, lastName, gender, tel, weight, height, date, time) " +
                         "VALUES  (@FirstName, @LastName, @Gender, @Tel, @Weight, @Height, @Date, @Time)";

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                cnn.Execute(sql, user);
            }
        }

        // Delete person
        public static void DeletePerson(UserModel user)
        {
            string sql = "delete from Person where id =" + user.ID;

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                cnn.Execute(sql);
            }
        }

        //// Get all test
        public static List<TestModel> GetTestByUID(UserModel user)
        {
            string sql = "select * from Test where uid=" + user.ID;

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                var output = cnn.Query<TestModel>(sql, new DynamicParameters());
                return output.ToList();
            }
        }

        // Add Test
        public static void AddTest(TestModel test)
        {
            string sql = "INSERT INTO Test (testName, uid, date, time) VALUES  (@TestName, @Uid, @Date, @Time)";

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                cnn.Execute(sql,test);
            }

            CreateTable(test);
        }

        private static void CreateTable(TestModel test)
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection(LoadConnetionString());
            m_dbConnection.Open();

            string sql = "create table IF NOT EXISTS " + test.TestName +
                "(id INTEGER NOT NULL UNIQUE, " +
                "time TEXT NOT NULL, " +
                "front_pelvis REAL NOT NULL, " +
                "right_shoulder REAL NOT NULL, " +
                "left_shoulder REAL NOT NULL, " +
                "pelvis_angle REAL NOT NULL," +
                "knee_angle REAL NOT NULL," +
                "ankle_angle REAL NOT NULL," +
                "PRIMARY KEY(id AUTOINCREMENT))";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);

            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        // Delete Test
        public static void DeleteTest(TestModel test)
        {
            string sql = "delete from Test where id =" + test.Id;

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                cnn.Execute(sql);
            }

            DropTable(test);
        }

        private static void DropTable(TestModel test)
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection(LoadConnetionString());
            m_dbConnection.Open();

            string sql = "DROP TABLE " + test.TestName;

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);

            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        // Get Angle
        public static void GetAngle(TestModel test)
        {
            string sql = "select * from " + test.TestName;

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                cnn.Execute(sql);
            }
        }

        // Add angle 
        public static void AddAngle(AngleModel angle, TestModel test)
        {
            string sql = "INSERT INTO " + test.TestName + " (time, front_pelvis, right_shoulder, left_shoulder, pelvis_angle, knee_angle, ankle_angle) " +
                "values (@Time, @FrontPelvis, @RightShoulder, @LeftShoulder, @Pelvis, @Knee, @Ankle) ";

            using (IDbConnection cnn = new SQLiteConnection(LoadConnetionString()))
            {
                cnn.Execute(sql, angle);
            }
        }
    }
}
