using assignment2_LEJ.Models;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace assignment2_LEJ.ViewModels
{
    public class WaferMapViewModel : INotifyPropertyChanged
    {
        #region[필드]
        private Wafer wafer;
        private double cellWidth;
        private double cellHeight;
        private double screenWidth;
        private double screenHeight;
        private ObservableCollection<Tuple<int, int>> dieCoordinates;
        public event PropertyChangedEventHandler PropertyChanged;
        private DieViewModel selectedDie;
        #endregion

        #region[속성]
        public DieViewModel SelectedDie
        {
            get => selectedDie;
            set
            {
                if (selectedDie != value)
                {
                    if (selectedDie != null)
                        selectedDie.IsSelected = false; 

                    selectedDie = value;

                    if (selectedDie != null)
                        selectedDie.IsSelected = true;  

                    OnPropertyChanged(nameof(SelectedDie));
                }
            }
        }
        public ObservableCollection<DieViewModel> DieViewModels { get; set; } = new ObservableCollection<DieViewModel>();
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

        public double ScreenWidth
        {
            get { return screenWidth; }
            set
            {
                screenWidth = value;
                OnPropertyChanged(nameof(ScreenWidth));
                UpdateDieSize();
                UpdateDieCoordinates();
            }
        }
        public double ScreenHeight
        {
            get { return screenHeight; }
            set
            {
                screenHeight = value;
                OnPropertyChanged(nameof(ScreenHeight));
                UpdateDieSize();
                UpdateDieCoordinates();
            }
        }
        public Wafer Wafer
        {
            get { return wafer; }
            set
            {
                wafer = value;
                OnPropertyChanged("Wafer");
                UpdateDieCoordinates();
            }
        }

        public ObservableCollection<Tuple<int, int>> DieCoordinates
        {
            get => dieCoordinates;
            set
            {
                dieCoordinates = value;
                OnPropertyChanged("DieCoordinates");
            }
        }
        #endregion

        #region[생성자]
        public WaferMapViewModel()
        {
            dieCoordinates = new ObservableCollection<Tuple<int, int>>();
            LoadWaferData(SharedData.Instance.WaferData);
            UpdateDieCoordinates();
            SharedData.Instance.PropertyChanged += SharedData_PropertyChanged;
        }
        #endregion

        #region[public 메서드]
        #endregion
        #region[protected, private 메서드]
        /**
         * @brief SharedData의 속성 변경 이벤트 핸들러입니다.
         * @param sender 이벤트 발생 객체
         * @param e PropertyChangedEventArgs의 인스턴스로 속성 변경 정보를 담고 있습니다.
         * @return 없음
         * 2023-08-28|이은진|WaferData 속성 변경 시 Wafer 데이터를 로드하고 Die 좌표 및 ViewModel을 업데이트합니다.
        */
        private void SharedData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WaferData")
            {
                LoadWaferData(SharedData.Instance.WaferData);
                UpdateDieCoordinates();
            }
        }
        /**
         * @brief 로드된 Wafer 데이터를 처리하고 화면을 업데이트합니다.
         * @param loadedWafer 로드된 Wafer 객체
         * @return 없음
         * 2023-08-28|이은진|로드된 Wafer 데이터를 처리하고 화면 크기에 맞게 Die 크기 및 좌표를 업데이트합니다.
        */
        private void LoadWaferData(Wafer loadedWafer)
        {
            Wafer = loadedWafer;
            UpdateDieSize();
            UpdateDieCoordinates();
        }
        /**
         * @brief Die의 좌표를 업데이트하고 ViewModel을 업데이트합니다.
         * @param 없음
         * @return 없음
         * 2023-08-28|이은진|Die 좌표를 화면 크기와 맞게 계산하고 ViewModel을 업데이트합니다.
         */
        private void UpdateDieCoordinates()
        {
            if (Wafer == null)
            {
                return;
            }
            else
            {
                dieCoordinates.Clear();
                DieViewModels.Clear();

                Wafer.GridWidth = Wafer.XMax - Wafer.XMin;
                Wafer.GridHeight = Wafer.YMax - Wafer.YMin;

                foreach (var die in Wafer.Dies)
                {
                    int gridX = die.Coordinate.Item1 - Wafer.XMin;
                    int gridY = Math.Abs(die.Coordinate.Item2 - Wafer.YMax);

                    die.GridCoordinate = new Tuple<int, int>((int)((CellWidth + 1) * gridX), (int)((CellHeight + 1) * gridY));

                    dieCoordinates.Add(die.GridCoordinate);
                    DieViewModels.Add(new DieViewModel { Die = die });
                }
            }
        }
        /**
         * @brief Die의 크기를 업데이트합니다.
         * @param 없음
         * @return 없음
         * 2023-08-28|이은진|Die의 크기를 화면 크기에 맞게 업데이트합니다.
        */
        private void UpdateDieSize()
        {
            if (Wafer == null) return;

            CellWidth = (ScreenWidth - 25) / (Wafer.GridWidth + 1);
            CellHeight = (ScreenHeight - 50) / (Wafer.GridHeight + 1);
        }
        /**
         * @brief 속성 변경 이벤트를 호출하여 UI를 업데이트합니다.
         * @param propertyName 변경된 속성의 이름
         * @return 없음
         * 2023-08-28|이은진|속성 변경 시 UI를 업데이트합니다.
         */
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

}