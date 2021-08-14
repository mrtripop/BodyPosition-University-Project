using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyPosition.MVVM.Model
{
    public class UserModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Tel { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

    }
}
