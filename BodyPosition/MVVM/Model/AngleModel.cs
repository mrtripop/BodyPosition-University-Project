using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyPosition.MVVM.Model
{
    class AngleModel
    {
        public int id { get; set; }
        public int time { get; set; }
        public double pelvis { get; set; }
        public double knee { get; set; }
        public double ankle { get; set; }
        public override string ToString()
        {
            return this.id + " " + this.time + " " + this.pelvis + " " + this.knee + " " + this.ankle;
        }
    }
}
