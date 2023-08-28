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
        private Die selectedDie;
        private Wafer wafer;
        private int currentDefectIndex;
        private int currentDieIndex;
        private int currentDieDefectIndex;

        public ICommand PreviousDefectCommand { get; }
        public ICommand NextDefectCommand { get; }
        public ICommand PreviousDieCommand { get; }
        public ICommand NextDieCommand { get; }
        public ICommand PreviousDieDefectCommand { get; }
        public ICommand NextDieDefectCommand { get; }
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
        public int CurrentDieIndex
        {
            get { return currentDieIndex; }
            set
            {
                if (currentDieIndex != value)
                {
                    currentDieIndex = value;
                    OnPropertyChanged(nameof(currentDieIndex));
                    OnPropertyChanged(nameof(DieDisplayText));
                    SelectedDie = Wafer.Dies[currentDieIndex];

                    if (SelectedDie.Defects.Any())
                    {
                        SelectedDefect = SelectedDie.Defects[0];
                    }
                    else
                    {
                        SelectedDefect = null;
                    }

                    OnPropertyChanged(nameof(TotalDieDefects));
                    OnPropertyChanged(nameof(DieDefectDisplayText));
                }
            }
        }
        public int CurrentDieDefectIndex
        {
            get { return currentDieDefectIndex; }
            set
            {
                if (currentDieDefectIndex != value)
                {
                    currentDieDefectIndex = value;
                    OnPropertyChanged(nameof(CurrentDieDefectIndex));
                    OnPropertyChanged(nameof(DieDefectDisplayText));
                }
            }
        }


        public int TotalDefects
        {
            get { return Defects.Count; }
        }
        public int TotalDies
        {
            get { return Wafer?.Dies?.Count ?? 0; }
        }
        public int TotalDieDefects
        {
            get { return SelectedDie?.Defects?.Count ?? 0; }
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
                OnPropertyChanged(nameof(DieDisplayText));
                OnPropertyChanged(nameof(TotalDefects));
                OnPropertyChanged(nameof(TotalDies));
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
        public string DieDisplayText
        {
            get
            {
                if (TotalDies == 0)
                {
                    return $"0/0";
                }
                return $"{CurrentDieIndex + 1}/{TotalDies}";
            }
        }
        public string DefectDisplayText
        {
            get
            {
                if (TotalDefects == 0)
                {
                    return $"0/0";
                }
                return $"{CurrentDefectIndex + 1}/{TotalDefects}";
            }
        }
        public string DieDefectDisplayText
        {
            get
            {
                if (TotalDieDefects == 0)
                {
                    return $"0/0";
                }
                return $"{CurrentDieDefectIndex + 1}/{TotalDieDefects}";
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
                    CurrentDieDefectIndex = 0;
                    UpdateCurrentDefectIndex();
                    UpdateCurrentDieDefectIndex();
                }
            }
        }
        public Die SelectedDie
        {
            get => selectedDie;
            set
            {
                if (selectedDie != value)
                {
                    selectedDie = value;
                    OnPropertyChanged(nameof(SelectedDie));
                    UpdateCurrentDieIndex();
                }
            }
        }
        public DefectInfoViewModel()
        {
            LoadWaferData(SharedData.Instance.WaferData);
            SharedData.Instance.PropertyChanged += SharedData_PropertyChanged;
            defects = new ObservableCollection<Defect>();
            PreviousDefectCommand = new RelayCommand(PreviousDefect);
            NextDefectCommand = new RelayCommand(NextDefect);
            PreviousDieCommand = new RelayCommand(PreviousDie);
            NextDieCommand = new RelayCommand(NextDie);
            PreviousDieDefectCommand = new RelayCommand(PreviousDieDefect);
            NextDieDefectCommand = new RelayCommand(NextDieDefect);
        }
        private void SharedData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WaferData")
            {
                LoadWaferData(SharedData.Instance.WaferData);
            }
        }
        private void UpdateCurrentDefectIndex()
        {
            if (Defects != null && selectedDefect != null)
            {
                CurrentDefectIndex = Defects.IndexOf(selectedDefect);
                SharedData.Instance.DefectIndexData = currentDefectIndex;
            }
        }
        private void UpdateCurrentDieIndex()
        {
            if (Wafer.Dies != null && selectedDie != null)
            {
                CurrentDieIndex = Wafer.Dies.IndexOf(selectedDie);
            }
        }
        private void UpdateCurrentDieDefectIndex()
        {
            if (SelectedDie?.Defects != null && SelectedDefect != null)
            {
                CurrentDieDefectIndex = SelectedDie.Defects.IndexOf(SelectedDefect);
            }
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
                SharedData.Instance.DefectIndexData = currentDefectIndex;
            }
        }

        private void NextDefect()
        {
            if (CurrentDefectIndex < TotalDefects - 1)
            {
                CurrentDefectIndex++;
                SelectedDefect = Defects[CurrentDefectIndex];
                SharedData.Instance.DefectIndexData = currentDefectIndex;
            }
        }
        private void PreviousDie()
        {
            if (CurrentDieIndex > 0)
            {
                CurrentDieIndex--;
                SelectedDie = Wafer.Dies[CurrentDieIndex];
            }
        }

        private void NextDie()
        {
            if (CurrentDieIndex < TotalDies - 1)
            {
                CurrentDieIndex++;
                SelectedDie = Wafer.Dies[CurrentDieIndex];
            }
        }
        private void PreviousDieDefect()
        {
            if (CurrentDieDefectIndex > 0)
            {
                CurrentDieDefectIndex--;
                SelectedDefect = SelectedDie.Defects[CurrentDieDefectIndex];
                UpdateCurrentDieDefectIndex();
            }
        }

        private void NextDieDefect()
        {
            if (CurrentDieDefectIndex < TotalDieDefects - 1)
            {
                CurrentDieDefectIndex++;
                SelectedDefect = SelectedDie.Defects[CurrentDieDefectIndex];
                UpdateCurrentDieDefectIndex();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
