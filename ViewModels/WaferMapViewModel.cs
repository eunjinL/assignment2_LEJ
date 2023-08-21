using assignment2_LEJ.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment2_LEJ.ViewModels
{
    public class WaferMapViewModel : INotifyPropertyChanged
    {
        private Wafer wafer;
        public Wafer Wafer
        {
            get { return wafer; }
            set
            {
                wafer = value;
                OnPropertyChanged("Wafer");
            }
        }


        public void LoadData(string filePath)
        {
            FileService fileService = new FileService();
            Wafer = fileService.LoadFromFile(filePath);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
