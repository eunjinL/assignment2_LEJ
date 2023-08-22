﻿using assignment2_LEJ.Models;
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
        private Die die;
        public Die Die
        {
            get => die;
            set
            {
                die = value;
                OnPropertyChanged(nameof(Die));
            }
        }

        public Tuple<int, int> GridCoordinate => Die?.GridCoordinate;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
