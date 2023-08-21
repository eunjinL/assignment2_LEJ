using assignment2_LEJ.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace assignment2_LEJ.ViewModels
{
    public class MainViewModel
    {
        public WaferMapViewModel WaferMapVM { get; set; }
        public DefectInfoViewModel DefectInfoVM { get; set; }
        public DefectImageViewModel DefectImageVM { get; set; }

        public ICommand LoadWaferDataCommand { get; set; }

        public MainViewModel()
        {
            LoadWaferDataCommand = new RelayCommand(LoadWaferData);
            // ... other initializations ...
        }

        private void LoadWaferData(object obj)
        {
            // Load data using the FileService and update the WaferMapVM and DefectInfoVM
        }
    }

}
