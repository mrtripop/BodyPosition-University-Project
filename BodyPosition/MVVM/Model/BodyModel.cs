using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace BodyPosition.MVVM.Model
{
    class BodyModel
    {
        public Vector3D Head { get; set; }
        public Vector3D Neck { get; set; } 
        public Vector3D SpineShoulder { get; set; }
        public Vector3D ShoulderRight { get; set; }
        public Vector3D ShoulderLeft { get; set; }
        public Vector3D SpineMid { get; set; }
        public Vector3D SpineBase { get; set; }
        public Vector3D HipLeft { get; set; }
        public Vector3D HipRight { get; set; }
        public Vector3D KneeLeft { get; set; }
        public Vector3D KneeRight { get; set; }
        public Vector3D AnkleLeft { get; set; }
        public Vector3D AnkleRight { get; set; }
        public Vector3D FootLeft { get; set; }
        public Vector3D FootRight { get; set; }
    }

}
