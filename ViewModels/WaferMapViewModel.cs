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
        private ObservableCollection<Tuple<int, int>> dieCoordinates;
        public ObservableCollection<DieViewModel> DieViewModels { get; set; } = new ObservableCollection<DieViewModel>();

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
            Messenger.Default.Register<Wafer>(this, (wafer) => LoadWaferData(wafer));
        }

        private void LoadWaferData(Wafer loadedWafer)
        {
            Wafer = loadedWafer;
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

                Wafer.GridWidth = (Wafer.XMax - Wafer.XMin) / Wafer.GridResolution;
                Wafer.GridHeight = (Wafer.YMax - Wafer.YMin) / Wafer.GridResolution;

                foreach (var die in Wafer.Dies)
                {
                    int gridX = (die.Coordinate.Item1 - Wafer.XMin) / Wafer.GridResolution;
                    int gridY = (die.Coordinate.Item2 - Wafer.YMin) / Wafer.GridResolution;
                    die.GridCoordinate = new Tuple<int, int>(gridX, gridY);

                    dieCoordinates.Add(die.GridCoordinate);
                    DieViewModels.Add(new DieViewModel { Die = die });
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
