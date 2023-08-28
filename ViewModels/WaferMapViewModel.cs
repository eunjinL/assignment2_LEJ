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
        private Wafer wafer;
        private double cellWidth;
        private double cellHeight;
        private double screenWidth;
        private double screenHeight;
        private ObservableCollection<Tuple<int, int>> dieCoordinates;
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
        public WaferMapViewModel()
        {
            dieCoordinates = new ObservableCollection<Tuple<int, int>>();
            LoadWaferData(SharedData.Instance.WaferData);
            UpdateDieCoordinates();
            SharedData.Instance.PropertyChanged += SharedData_PropertyChanged;
        }
        private void SharedData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WaferData")
            {
                LoadWaferData(SharedData.Instance.WaferData);
                UpdateDieCoordinates();
            }
        }
        private void LoadWaferData(Wafer loadedWafer)
        {
            Wafer = loadedWafer;
            UpdateDieSize();
            UpdateDieCoordinates();
        }

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
        private void UpdateDieSize()
        {
            if (Wafer == null) return;

            CellWidth = (ScreenWidth - 25) / (Wafer.GridWidth + 1);
            CellHeight = (ScreenHeight - 35) / (Wafer.GridHeight + 1);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}