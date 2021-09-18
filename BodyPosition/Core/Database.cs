using BodyPosition.MVVM.Model.AngleModel;
using BodyPosition.MVVM.Model.TestModel;
using BodyPosition.MVVM.Model.UserModel;
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
        string PATH_ANGLE_FOLDER = Path.Combine(Environment.CurrentDirectory, @"JsonDatabase\Angle");


        public Database(string path)
        {
            _path = path;

            // create path
            if (!Directory.Exists(PATH_ANGLE_FOLDER))
            {
                Directory.CreateDirectory(PATH_ANGLE_FOLDER);
            }
        }

        private JObject ReadJson()
        {
            JObject json = JObject.Parse(File.ReadAllText(_path));
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
    }
}
