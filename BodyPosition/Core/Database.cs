using BodyPosition.MVVM.Model.AngleModel;
using BodyPosition.MVVM.Model.TestModel;
using BodyPosition.MVVM.Model.UserModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyPosition.Core
{
    class Database
    {
        
        private string _path;
        public string path
        {
            get { return _path; }
        }

        public readonly string PATH_USER = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\UserJson.json");
        public readonly string PATH_TEST = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\TestJson.json");
        public readonly string PATH_ANGLE = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\Angle");

        public readonly string PATH_BACKUP_USER = Path.Combine(Environment.CurrentDirectory, @"BackupDatabase\UserJson.json");
        public readonly string PATH_BACKUP_TEST = Path.Combine(Environment.CurrentDirectory, @"BackupDatabase\TestJson.json");
        public readonly string PATH_BACKUP_ANGLE = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\Angle");

        public Database(string path)
        {
            _path = path;

        }

        public Database CreateUserInstance()
        {
            return new Database(PATH_USER);
        }

        public Database CreateTestInstance()
        {
            return new Database(PATH_TEST);
        }

        public Database CreateBackupUserInstance()
        {
            return new Database(PATH_BACKUP_USER);
        }

        public Database CreateBackupTestInstance()
        {
            return new Database(PATH_BACKUP_TEST);
        }

        private JObject ReadJson()
        {
            //JObject json = JObject.Parse(File.ReadAllText(_path));
            JObject json;
            using (StreamReader file = File.OpenText(_path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                json = (JObject)JToken.ReadFrom(reader);
            }
            return json;
        }

        public Dictionary<string, UserModel> ReadUser()
        {
            var json = ReadJson();
            var userModel = UserModel.FromJson(json.ToString());
            return userModel;
        }

        public AngleModel ReadAngle()
        {
            var json = ReadJson();
            var angleModel = AngleModel.FromJson(json.ToString());
            return angleModel;
        }

        public Dictionary<string, Dictionary<string, TestModel>> ReadTest()
        {
            var json = ReadJson();
            var testModel = TestModel.FromJson(json.ToString());
            return testModel;
        }

        public void WriteJson(AngleModel angle)
        {
            File.WriteAllText(_path, angle.ToJson());
        }

        public void WriteJson(Dictionary<string, Dictionary<string, TestModel>> test)
        {
            File.WriteAllText(_path, test.ToJson());
        }

        public void WriteJson(Dictionary<string, UserModel> user)
        {
            File.WriteAllText(_path, user.ToJson());
        }

        public void WriteJson(string json)
        {
            File.WriteAllText(_path, json);
        }
    }
}
