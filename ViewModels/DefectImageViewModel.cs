using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace assignment2_LEJ.ViewModels
{
    public class DefectImageViewModel : INotifyPropertyChanged
    {
        #region[필드]
        private int receivedDefectID = 2;
        private string receivedFolderPath;
        private BitmapSource loadedImage;
        private TiffBitmapDecoder tiffDecoder;
        private bool receivedDefectShow = false;
        public event PropertyChangedEventHandler PropertyChanged;
        private double scale = 1;
        private Point imageStartPoint;
        private Point lineStartPoint;
        private Point lineEndPoint;
        private bool isDrawing = false;
        private bool isDragging = false;
        private double translateX = 0.0;
        private double translateY = 0.0;
        private string lineLengthText;
        private ObservableCollection<Line> drawnLines = new ObservableCollection<Line>();
        #endregion

        #region[속성]
        public ICommand ZoomInCommand => new RelayCommand(ZoomIn);
        public ICommand ZoomOutCommand => new RelayCommand(ZoomOut);
        public ICommand MouseLeftButtonDownCommand => new RelayCommand(MouseLeftButtonDown);
        public ICommand MouseLeftButtonUpCommand => new RelayCommand(MouseLeftButtonUp);
        public ICommand MouseMoveCommand => new RelayCommand(MouseButtonMove);
        public ICommand StartDrawingCommand => new RelayCommand(StartDrawing);
        public ICommand FinishDrawingCommand => new RelayCommand(FinishDrawing);
        public double Scale
        {
            get { return scale; }
            set
            {
                if (scale != value)
                {
                    scale = value;
                    Application.Current.Dispatcher.Invoke(() => OnPropertyChanged("Scale"));
                }
            }
        }
        public string LineLengthText
        {
            get { return lineLengthText; }
            set
            {
                lineLengthText = value;
                OnPropertyChanged(nameof(LineLengthText));
            }
        }
        public ObservableCollection<Line> DrawnLines
        {
            get { return drawnLines; }
            set
            {
                drawnLines = value;
                OnPropertyChanged(nameof(DrawnLines));
            }
        }
        public Point StartPoint
        {
            get { return lineStartPoint; }
            set
            {
                lineStartPoint = value;
                OnPropertyChanged(nameof(StartPoint));
            }
        }

        public Point EndPoint
        {
            get { return lineEndPoint; }
            set
            {
                lineEndPoint = value;
                OnPropertyChanged(nameof(EndPoint));
            }
        }

        public bool IsDrawing
        {
            get { return isDrawing; }
            set
            {
                isDrawing = value;
                OnPropertyChanged(nameof(IsDrawing));
            }
        }
        public double TranslateX
        {
            get { return translateX; }
            set
            {
                translateX = value;
                Application.Current.Dispatcher.Invoke(() => OnPropertyChanged("TranslateX"));
                // OnPropertyChanged("TranslateX");
            }
        }
        public double TranslateY
        {
            get { return translateY; }
            set
            {
                translateY = value;
                OnPropertyChanged("TranslateY");
            }
        }
        private void ZoomIn()
        {
            Scale *= 1.02; 
        }

        private void ZoomOut()
        {
            Scale /= 1.02; 
        }

        /**
        * @brief 받은 폴더 경로 속성
        * @return 현재의 폴더 경로
        * 2023-08-28|이은진|첫 버전 작성
        */
        public string ReceivedFolderPath
        {
            get { return receivedFolderPath; }
            set
            {
                if (receivedFolderPath != value)
                {
                    receivedFolderPath = value;
                    OnPropertyChanged(nameof(ReceivedFolderPath));
                    LoadTiffImage();
                }
            }
        }
        /**
        * @brief 받은 결함 ID 속성
        * @return 현재의 결함 ID
        * 2023-08-28|이은진|첫 버전 작성
        */
        public int ReceivedDefectID
        {
            get { return receivedDefectID; }
            set
            {
                if (receivedDefectID != value)
                {
                    receivedDefectID = value;
                    OnPropertyChanged(nameof(receivedDefectID));
                    LoadTiffImage();
                }
            }
        }
        /**
        * @brief 로드된 이미지 속성
        * @return 현재 로드된 이미지
        * 2023-08-28|이은진|첫 버전 작성
        */
        public BitmapSource LoadedImage
        {
            get { return loadedImage; }
            set
            {
                if (loadedImage != value)
                {
                    loadedImage = value;
                    OnPropertyChanged(nameof(LoadedImage));
                }
            }
        }
        /**
        * @brief 결함 표시 여부 속성
        * @return 현재의 결함 표시 여부
        * 2023-08-28|이은진|첫 버전 작성
        */
        public bool ReceivedDefectShow
        {
            get { return receivedDefectShow; }
            set
            {
                if (receivedDefectShow != value)
                {
                    receivedDefectShow = value;
                    OnPropertyChanged(nameof(receivedDefectShow));
                    LoadTiffImage();
                }
            }
        }
        /**
        * @brief TIFF 이미지 로드 메서드
        * @return 없음
        * 2023-08-28|이은진|첫 버전 작성
        */
        private void LoadTiffImage()
        {
            if (!Directory.Exists(receivedFolderPath))
                return;

            var tifFiles = Directory.GetFiles(receivedFolderPath, "*.tif");
            if (tifFiles.Length == 0)
                return;

            var tifFile = tifFiles[0];
            tiffDecoder = new TiffBitmapDecoder(new Uri(tifFile, UriKind.Absolute), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            if (tiffDecoder.Frames.Count > 0 && receivedDefectShow)
            {
                LoadedImage = tiffDecoder.Frames[receivedDefectID];
            }
        }
        #endregion

        #region[생성자]
        public DefectImageViewModel()
        {
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
            if (e.PropertyName == "DefectIndexData")
            {
                ReceivedDefectID = SharedData.Instance.DefectIndexData;
            }
            else if (e.PropertyName == "DefectShowData")
            {
                ReceivedDefectShow = SharedData.Instance.DefectShowData;
            }
            else if (e.PropertyName == "FolderPath")
            {
                ReceivedFolderPath = SharedData.Instance.FolderPath;
            }
        }
        private void StartDrawing()
        {
            DrawnLines.Clear();
            IsDrawing = true;
            StartPoint = Mouse.GetPosition(null);
            EndPoint = StartPoint;
        }
        private void FinishDrawing()
        {
            IsDrawing = false;
            EndPoint = Mouse.GetPosition(null);

            // 여기서 DrawnLines에 선 정보를 추가하도록 처리
            DrawnLines.Add(CreateLine(StartPoint, EndPoint));
            CalculateLineLength(StartPoint, EndPoint);
        }
        private void MouseLeftButtonDown()
        {
            Mouse.OverrideCursor = Cursors.ScrollAll;
            isDragging = true;
            imageStartPoint = Mouse.GetPosition(null);
        }

        private void MouseLeftButtonUp()
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            isDragging = false;
        }

        private void MouseButtonMove()
        {
            if (isDragging)
            {
                Point newPoint = Mouse.GetPosition(null);
                double deltaX = newPoint.X - imageStartPoint.X;
                double deltaY = newPoint.Y - imageStartPoint.Y;

                TranslateX += deltaX;
                TranslateY += deltaY;

                imageStartPoint = newPoint;
            }
            else if (IsDrawing)
            {
                EndPoint = Mouse.GetPosition(null);
            }
        }
        private Line CreateLine(Point startPoint, Point endPoint)
        {
            return new Line
            {
                X1 = startPoint.X,
                Y1 = startPoint.Y,
                X2 = endPoint.X,
                Y2 = endPoint.Y,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
        }
        private void CalculateLineLength(Point startPoint, Point endPoint)
        {
            double lengthInPixels = Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2) + Math.Pow(endPoint.Y - startPoint.Y, 2));
            double lengthInMicrometers = lengthInPixels * 0.74;

            LineLengthText = $"{lengthInMicrometers:F2} µm";
        }
        /**
        * @brief 속성 변경 이벤트 발생 메서드
        * @param propertyName 변경된 속성의 이름
        * @return 없음
        * 2023-08-28|이은진|첫 버전 작성
        */
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

}
