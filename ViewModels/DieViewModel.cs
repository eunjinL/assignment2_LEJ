using assignment2_LEJ.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace assignment2_LEJ.ViewModels
{
    public class DieViewModel : INotifyPropertyChanged
    {
        #region[필드]
        private Die die;
        public event PropertyChangedEventHandler PropertyChanged;
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
        #endregion

        #region[protected, private 메서드]
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion



        
        
    }

}
