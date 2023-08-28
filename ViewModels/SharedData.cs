using assignment2_LEJ.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment2_LEJ.ViewModels
{
    public class SharedData : INotifyPropertyChanged
    {
        private static SharedData instance = null;
        private Wafer waferData;
        private string folderPath;
        private bool defectShowData;
        private int defectIndexData;
        public event PropertyChangedEventHandler PropertyChanged;

        public static SharedData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SharedData();
                }

                return instance;
            }
        }

        public string FolderPath 
        {
            get { return folderPath; }
            set
            {
                folderPath = value;
                OnPropertyChanged("FolderPath");
            }
        }
        public Wafer WaferData
        {
            get { return waferData; }
            set
            {
                waferData = value;
                OnPropertyChanged("WaferData");
            }
        }
        public bool DefectShowData
        {
            get { return defectShowData; }
            set
            {
                defectShowData = value;
                OnPropertyChanged("DefectShowData");
            }
        }
        public int DefectIndexData
        {
            get { return defectIndexData; }
            set
            {
                defectIndexData = value;
                OnPropertyChanged("DefectIndexData");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

}
