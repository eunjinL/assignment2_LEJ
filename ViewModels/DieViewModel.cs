using assignment2_LEJ.Models;
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
    public class DieViewModel : NotifierBase
    {
        #region[필드]
        private Die die;
        private bool isSelected;
        #endregion
        #region[속성]
        private ICommand selectCommand;
        public ICommand SelectCommand
        {
            get
            {
                if (selectCommand == null)
                {
                    selectCommand = new RelayCommand((param) =>
                    {
                        WaferMapViewModel parentVM = param as WaferMapViewModel;
                        if (parentVM != null)
                        {
                            parentVM.SelectedDie = this;
                            Tuple<int, int> selectedCoordinate = this.Die.Coordinate;
                            SharedData.Instance.SelectedCoordinate = selectedCoordinate;
                        }
                    });
                }
                return selectCommand;
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public Die Die
        {
            get => die;
            set
            {
                die = value;
                DefectViewModels.Clear();
                foreach (var defect in die.Defects)
                {
                    DefectViewModels.Add(new DefectViewModel { Defect = defect });
                }
                OnPropertyChanged(nameof(Die));
            }
        }
        public Tuple<int, int> GridCoordinate
        {
            get
            {
                if (Die != null)
                {
                    return Die.GridCoordinate;
                }
                return null;
            }
        }
        public ObservableCollection<DefectViewModel> DefectViewModels { get; set; } = new ObservableCollection<DefectViewModel>();


        #endregion

        #region[protected, private 메서드]
        #endregion
    }

}
