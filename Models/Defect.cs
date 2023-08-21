using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment2_LEJ.Models
{
    public class Defect
    {
        public int DefectID { get; set; }
        public double XREL { get; set; }
        public double YREL { get; set; }
        public int XINDEX { get; set; }
        public int YINDEX { get; set; }
        public double XSIZE { get; set; }
        public double YSIZE { get; set; }
        public double DEFECTAREA { get; set; }
        public double DSIZE { get; set; }
        public int CLASSNUMBER { get; set; }
        public int TEST { get; set; }
        public int CLUSTERNUMBER { get; set; }
        public int ROUGHBINNUMBER { get; set; }
        public int FINEBINNUMBER { get; set; }
        public int REVIEWSAMPLE { get; set; }
        public int IMAGECOUNT { get; set; }
        public List<int> IMAGELIST { get; set; } = new List<int>();
    }
}
