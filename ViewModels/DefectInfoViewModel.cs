using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using assignment2_LEJ.Models;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace assignment2_LEJ.ViewModels
{
    public class DefectInfoViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Defect> defects;
        private Defect selectedDefect;
        private Wafer wafer;
        private int currentDefectIndex;
        // private string defectDisplayText;

        public ICommand PreviousDefectCommand { get; }
        public ICommand NextDefectCommand { get; }
        public int CurrentDefectIndex
        {
            get { return currentDefectIndex; }
            set
            {
                if (currentDefectIndex != value)
                {
                    currentDefectIndex = value;
                    OnPropertyChanged(nameof(CurrentDefectIndex));
                    OnPropertyChanged(nameof(DefectDisplayText));
                }
            }
        }

        public int TotalDefects
        {
            get { return Defects.Count; }
        }


        public Wafer Wafer
        {
            get { return wafer; }
            set
            {
                wafer = value;
                OnPropertyChanged(nameof(Wafer));
                LoadDefectsFromWafer();
                OnPropertyChanged(nameof(DefectDisplayText));  
                OnPropertyChanged(nameof(TotalDefects));
            }
        }

        public ObservableCollection<Defect> Defects
        {
            get => defects;
            set
            {
                defects = value;
                OnPropertyChanged(nameof(Defects));
            }
        }
        public string DefectDisplayText
        {
            get
            {
                return $"{CurrentDefectIndex + 1}/{TotalDefects}";
            }
        }

        public Defect SelectedDefect
        {
            get => selectedDefect;
            set
            {
                if (selectedDefect != value)
                {
                    selectedDefect = value;
                    OnPropertyChanged(nameof(SelectedDefect));
                    UpdateCurrentDefectIndex();
                }
            }
        }
        private void UpdateCurrentDefectIndex()
        {
            if (Defects != null && selectedDefect != null)
            {
                CurrentDefectIndex = Defects.IndexOf(selectedDefect);
                Messenger.Default.Send<int>(currentDefectIndex);
            }
        }
        public DefectInfoViewModel()
        {
            Messenger.Default.Register<Wafer>(this, LoadWaferData);
            defects = new ObservableCollection<Defect>();
            PreviousDefectCommand = new RelayCommand(PreviousDefect);
            NextDefectCommand = new RelayCommand(NextDefect);
        }

        private void LoadWaferData(Wafer loadedWafer)
        {
            Wafer = loadedWafer;
        }

        private void LoadDefectsFromWafer()
        {
            if (Wafer != null)
            {
                List<Defect> allDefects = new List<Defect>();
                foreach (var die in Wafer.Dies)
                {
                    allDefects.AddRange(die.Defects);
                }

                allDefects.Sort((x, y) => x.DefectID.CompareTo(y.DefectID));

                Defects = new ObservableCollection<Defect>(allDefects);
            }
        }

        private void PreviousDefect()
        {
            if (CurrentDefectIndex > 0)
            {
                CurrentDefectIndex--;
                SelectedDefect = Defects[CurrentDefectIndex];
                Messenger.Default.Send<int>(currentDefectIndex);
            }
        }

        private void NextDefect()
        {
            if (CurrentDefectIndex < TotalDefects - 1)
            {
                CurrentDefectIndex++;
                SelectedDefect = Defects[CurrentDefectIndex];
                Messenger.Default.Send<int>(currentDefectIndex);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
