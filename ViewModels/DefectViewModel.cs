using assignment2_LEJ.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private double cellWidth;
        private double cellHeight;

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
        public double CellWidth
        {
            get { return cellWidth; }
            set
            {
                cellWidth = value;
                OnPropertyChanged(nameof(CellWidth));
            }
        }
        public double CellHeight
        {
            get { return cellHeight; }
            set
            {
                cellHeight = value;
                OnPropertyChanged(nameof(CellHeight));
            }
        }
        #region[생성자]
        public DefectViewModel()
        {
            CellWidth = SharedData.Instance.CellWidth;
            CellHeight = SharedData.Instance.CellHeight;
            SharedData.Instance.PropertyChanged += SharedData_PropertyChanged;
        }
        #endregion

        #region[protected, private 메서드]
        /**
        * @brief 데이터 변경 이벤트 핸들러
        * @param sender 이벤트를 발생시킨 객체
        * @param e PropertyChangedEventArgs 객체로, 변경된 속성 정보 포함
        * @return 없음
        * 2023-08-28|이은진|첫 버전 작성
        */
        private void SharedData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CellWidth")
            {
                CellWidth = SharedData.Instance.CellWidth;
                NormalizeCoordinates();
            }
            else if (e.PropertyName == "CellHeight")
            {
                CellHeight = SharedData.Instance.CellHeight;
                NormalizeCoordinates();
            }
        }
        public void NormalizeCoordinates()
        {
            if (CellWidth != 0 && CellHeight != 0)
            {
                /*double xScaleCoordinate = Defect.XREL / SharedData.Instance.WaferData.DefectXMax;
                double yScaleCoordinate = (SharedData.Instance.WaferData.DefectYMax - Defect.YREL) / SharedData.Instance.WaferData.DefectYMax;*/
                double xScaleCoordinate = Defect.XREL / SharedData.Instance.WaferData.DefectWidth;
                double yScaleCoordinate = (SharedData.Instance.WaferData.DefectHeight - Defect.YREL) / SharedData.Instance.WaferData.DefectHeight;
                xPosition = (CellWidth - 6) * xScaleCoordinate;
                yPosition = (CellHeight - 6) * yScaleCoordinate;
            }
            else
            {
                xPosition = 0;
                yPosition = 0;
            }
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
#endregion