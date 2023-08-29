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
        #region[필드]
        private ObservableCollection<Defect> defects;
        private Defect selectedDefect;
        private Die selectedDie;
        private Wafer wafer;
        private int currentDefectIndex;
        private int currentDieIndex;
        private int currentDieDefectIndex;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region[속성]
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
                return $"{CurrentDieIndex + 1} / {TotalDies}";
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
                return $"{CurrentDefectIndex + 1} / {TotalDefects}";
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
                return $"{CurrentDieDefectIndex + 1} / {TotalDieDefects}";
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
                    UpdateCurrentDieDefectIndex();

                    for (int i = 0; i < Wafer.Dies.Count; i++)
                    {
                        Die currentDie = Wafer.Dies[i];
                        if (currentDie.Defects.Contains(selectedDefect))
                        {
                            SelectedDie = currentDie;
                            CurrentDieIndex = i;
                            CurrentDieDefectIndex = currentDie.Defects.IndexOf(selectedDefect);
                            break;
                        }
                    }
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
        #endregion

        #region[생성자]
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
        #endregion

        #region[public 메서드]

        #endregion

        #region[protected, private 메서드]

        /**
        * @brief SharedData의 속성 변경 이벤트 핸들러입니다.
        * @param sender 이벤트를 발생시킨 객체
        * @param e PropertyChangedEventArgs 객체
        * @return 없음
        * 2023-08-28|이은진|초안 작성
        * 2023-08-29|이은진|SharedData와 좌표 주고 받기 기능 추가 
        */
        private void SharedData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WaferData")
            {
                LoadWaferData(SharedData.Instance.WaferData);
            }
            if (e.PropertyName == "SelectedCoordinate")
            {
                Tuple<int, int> selectedCoordinate = SharedData.Instance.SelectedCoordinate;

                for (int i = 0; i < Wafer.Dies.Count; i++)
                {
                    Die currentDie = Wafer.Dies[i];
                    if (currentDie.Coordinate.Item1 == selectedCoordinate.Item1 && currentDie.Coordinate.Item2 == selectedCoordinate.Item2)
                    {
                        SelectedDie = currentDie;
                        CurrentDieIndex = i;
                        CurrentDieDefectIndex = 0; 
                        UpdateCurrentDefectIndex();
                        UpdateCurrentDieDefectIndex();
                        OnPropertyChanged(nameof(DieDisplayText));
                        OnPropertyChanged(nameof(DieDefectDisplayText));
                        break;
                    }
                }
            }
        }
        /**
        * @brief 현재 선택된 결함 인덱스를 업데이트합니다.
        * @param 없음
        * @return 없음
        * 2023-08-28|이은진|초안 작성
        */
        private void UpdateCurrentDefectIndex()
        {
            if (Defects != null && selectedDefect != null)
            {
                CurrentDefectIndex = Defects.IndexOf(selectedDefect);
                SharedData.Instance.DefectIndexData = currentDefectIndex;
            }
        }
        /**
        * @brief 현재 선택된 다이 인덱스를 업데이트합니다.
        * @param 없음
        * @return 없음
        * 2023-08-28|이은진|초안 작성
        */
        private void UpdateCurrentDieIndex()
        {
            if (Wafer.Dies != null && selectedDie != null)
            {
                CurrentDieIndex = Wafer.Dies.IndexOf(selectedDie);
            }
        }
        /**
        * @brief 현재 선택된 다이의 결함 인덱스를 업데이트합니다.
        * @param 없음
        * @return 없음
        * 2023-08-28|이은진|초안 작성
        */
        private void UpdateCurrentDieDefectIndex()
        {
            if (SelectedDie?.Defects != null && SelectedDefect != null)
            {
                CurrentDieDefectIndex = SelectedDie.Defects.IndexOf(SelectedDefect);
            }
        }
        /**
        * @brief Wafer 데이터를 로드합니다.
        * @param loadedWafer 로드된 Wafer 객체
        * @return 없음
        * 2023-08-28|이은진|초안 작성
        */
        private void LoadWaferData(Wafer loadedWafer)
        {
            Wafer = loadedWafer;
        }
        /**
        * @brief Wafer에서 결함 데이터를 로드합니다.
        * @param 없음
        * @return 없음
        * 2023-08-28|이은진|초안 작성
        */
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
        /**
        * @brief 이전 결함을 선택합니다.
        * @param 없음
        * @return 없음
        * 2023-08-28|이은진|초안 작성
        */
        private void PreviousDefect()
        {
            if (CurrentDefectIndex > 0)
            {
                CurrentDefectIndex--;
                SelectedDefect = Defects[CurrentDefectIndex];
                SharedData.Instance.DefectIndexData = currentDefectIndex;
            }
        }
        /**
        * @brief 다음 결함을 선택합니다.
        * @param 없음
        * @return 없음
        * 2023-08-28|이은진|초안 작성
        */
        private void NextDefect()
        {
            if (CurrentDefectIndex < TotalDefects - 1)
            {
                CurrentDefectIndex++;
                SelectedDefect = Defects[CurrentDefectIndex];
                SharedData.Instance.DefectIndexData = currentDefectIndex;
            }
        }
        /**
        * @brief 이전 다이를 선택합니다.
        * @param 없음
        * @return 없음
        * 2023-08-28|이은진|초안 작성
        */
        private void PreviousDie()
        {
            if (CurrentDieIndex > 0)
            {
                CurrentDieIndex--;
                SelectedDie = Wafer.Dies[CurrentDieIndex];
                SharedData.Instance.DefectIndexData = currentDefectIndex;
            }
        }
        /**
        * @brief 다음 다이를 선택합니다.
        * @param 없음
        * @return 없음
        * 2023-08-28|이은진|초안 작성
        */
        private void NextDie()
        {
            if (CurrentDieIndex < TotalDies - 1)
            {
                CurrentDieIndex++;
                SelectedDie = Wafer.Dies[CurrentDieIndex];
                SharedData.Instance.DefectIndexData = currentDefectIndex;
            }
        }
        /**
        * @brief 이전 다이의 결함을 선택합니다.
        * @param 없음
        * @return 없음
        * 2023-08-28|이은진|초안 작성
        */
        private void PreviousDieDefect()
        {
            if (CurrentDieDefectIndex > 0)
            {
                CurrentDieDefectIndex--;
                SelectedDefect = SelectedDie.Defects[CurrentDieDefectIndex];
                UpdateCurrentDieDefectIndex();
            }
        }
        /**
        * @brief 다음 다이의 결함을 선택합니다.
        * @param 없음
        * @return 없음
        * 2023-08-28|이은진|초안 작성
        */
        private void NextDieDefect()
        {
            if (CurrentDieDefectIndex < TotalDieDefects - 1)
            {
                CurrentDieDefectIndex++;
                SelectedDefect = SelectedDie.Defects[CurrentDieDefectIndex];
                UpdateCurrentDieDefectIndex();
            }
        }
        /**
        * @brief 속성 변경 이벤트를 호출합니다.
        * @param propertyName 변경된 속성의 이름
        * @return 없음
        * 2023-08-28|이은진|초안 작성
        */
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }


}
