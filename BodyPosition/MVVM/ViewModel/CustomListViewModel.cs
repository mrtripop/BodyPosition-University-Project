using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyPosition.MVVM.ViewModel
{
    class CustomListViewModel
    {

        #region Properties
        private int id;
        private string name;
        private string gender;
        private double weight;
        private double height;
        private string date;

        public string PersonName
        {
            get { return name; }
            set { name = value;}
        }

        public int PersonID
        {
            get { return id; }
            set { id = value;}
        }

        public string PersonGender
        {
            get { return gender; }
            set { gender = value; }
        }

        public double PersonWeight
        {
            get { return weight; }
            set { weight = value; }
        }

        public double PersonHeight
        {
            get { return height; }
            set { height = value; }
        }

        public string PersonDate
        {
            get { return date; }
            set { date = value; }
        }
        #endregion
    }
}
