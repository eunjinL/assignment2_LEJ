using assignment2_LEJ.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment2_LEJ.ViewModels
{
    public class DieViewModel : INotifyPropertyChanged
    {
        #region[필드]
        private Die die;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region[속성]
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
