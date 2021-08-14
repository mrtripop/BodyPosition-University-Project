using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyPosition.MVVM.Model
{
    class AngleModel
    {
        public int ID { get; set; }
        public string Time { get; set; }
        public double FrontPelvis { get; set; }
        public double RightShoulder { get; set; }
        public double LeftShoulder { get; set; }
        public double Pelvis { get; set; }
        public double Knee { get; set; }
        public double Ankle { get; set; }
    }
}
