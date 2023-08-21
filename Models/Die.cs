using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment2_LEJ.Models
{
    public class Die
    {
        public Tuple<int, int> Coordinate { get; set; } // SampleTestPlan coordinates
        public Defect DefectInfo { get; set; }  // Null if no defect
    }
}
