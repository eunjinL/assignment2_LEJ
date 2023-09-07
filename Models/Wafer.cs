using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment2_LEJ.Models
{
    public class Wafer
    {
        private int gridResolution = 1;
        public string FileVersion { get; set; }
        public DateTime ResultTimestamp { get; set; }
        public Tuple<string, List<string>> TiffSpec { get; set; }
        public List<string> InspectionStationID { get; set; } = new List<string>();
        public string SampleType { get; set; }
        public DateTime FileTimestamp { get; set; }
        public string LotID { get; set; }
        public Tuple<int, int> SampleSize { get; set; }
        public string SetupID { get; set; }
        public DateTime SetupDate { get; set; }
        public string StepID { get; set; }
        public string SampleOrientationMarkType { get; set; }
        public string OrientationMarkLocation { get; set; }
        public string TiffFilename { get; set; }
        public Tuple<double, double> DiePitch { get; set; }
        public Tuple<double, double> DieOrigin { get; set; }
        public string WaferID { get; set; }
        public string Slot { get; set; }
        public Tuple<double, double> SampleCenterLocation { get; set; }
        public int InspectionTest { get; set; }
        public int Count { get; set; }
        public int XMax { get; set; }
        public int YMax { get; set; }
        public int XMin { get; set; }
        public int YMin { get; set; }
        public int GridWidth { get; set; }
        public int GridHeight { get; set; }
        public double DefectWidth { get; set; }
        public double DefectHeight { get; set; }
        public double DefectXMax { get; set; }
        public double DefectYMax { get; set; }
        public double DefectXMin { get; set; }
        public double DefectYMin { get; set; }
        public int GridResolution
        {
            get { return gridResolution; }
            set { gridResolution = value; }
        }
        public List<Die> Dies { get; set; } = new List<Die>();
    }
}
