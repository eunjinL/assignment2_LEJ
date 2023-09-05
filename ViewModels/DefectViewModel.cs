using assignment2_LEJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment2_LEJ.ViewModels
{
    public class DefectViewModel : NotifierBase
    {
        private Defect defect;
        private double xPosition;
        private double yPosition;

        public Defect Defect
        {
            get => defect;
            set
            {
                defect = value;
                OnPropertyChanged(nameof(Defect));
                NormalizeCoordinates();
                OnPropertyChanged(nameof(XPosition));
                OnPropertyChanged(nameof(YPosition));
            }
        }
        public void NormalizeCoordinates()
        {
            xPosition = Defect.XREL/1000;
            yPosition = Defect.YREL/1000;
        }
        public double XPosition
        {
            get 
            {
                return xPosition;
            }
            set
            {
                if (xPosition != value)  
                {
                    xPosition = value; 
                    OnPropertyChanged(nameof(XPosition)); 
                }
            }
        }
        public double YPosition
        {
            get
            {
                return yPosition;
            }
            set
            {
                if (yPosition != value)
                {
                    yPosition = value;
                    OnPropertyChanged(nameof(YPosition));
                }
            }
        }
    }

}
