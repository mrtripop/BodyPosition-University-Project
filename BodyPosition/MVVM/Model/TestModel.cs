using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyPosition.MVVM.Model
{
    class TestModel
    {
        public int id { get; set; }
        public int person_id { get; set; }
        public override string ToString()
        {
            return "test id: "+this.id+", person id: "+this.person_id;
        }
    }
}
