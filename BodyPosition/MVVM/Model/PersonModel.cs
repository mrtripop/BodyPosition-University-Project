using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyPosition.MVVM.Model
{
    class PersonModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string tel { get; set; }
        public string weight { get; set; }
        public string height { get; set; }
        public string date { get; set; }

        public override string ToString()
        {
            return this.id + " " + this.name + " " + this.gender + " " + this.tel + " " + this.weight + " " + this.height + " " + this.date;
        }
    }
}
