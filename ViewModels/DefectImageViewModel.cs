using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace assignment2_LEJ.ViewModels
{
    public class DefectImageViewModel : INotifyPropertyChanged
    {
        private int receivedDefectID = 2;
        private string receivedFolderPath;
        private BitmapSource loadedImage;
        private TiffBitmapDecoder tiffDecoder;
        public string ReceivedFolderPath
        {
            get { return receivedFolderPath; }
            set
            {
                if (receivedFolderPath != value)
                {
                    receivedFolderPath = value;
                    OnPropertyChanged(nameof(ReceivedFolderPath));
                    LoadTiffImage();
                }
            }
        }
        public int ReceivedDefectID
        {
            get { return receivedDefectID; }
            set
            {
                if (receivedDefectID != value)
                {
                    receivedDefectID = value;
                    OnPropertyChanged(nameof(receivedDefectID));
                    LoadTiffImage(); 
                }
            }
        }
        public BitmapSource LoadedImage
        {
            get { return loadedImage; }
            set
            {
                if (loadedImage != value)
                {
                    loadedImage = value;
                    OnPropertyChanged(nameof(LoadedImage));
                }
            }
        }
        private void LoadTiffImage()
        {
            if (!Directory.Exists(receivedFolderPath))
                return;

            var tifFiles = Directory.GetFiles(receivedFolderPath, "*.tif");
            if (tifFiles.Length == 0)
                return;

            var tifFile = tifFiles[0];
            tiffDecoder = new TiffBitmapDecoder(new Uri(tifFile, UriKind.Absolute), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            if (tiffDecoder.Frames.Count > 0)
            {
                LoadedImage = tiffDecoder.Frames[receivedDefectID];
            }
        }
        public DefectImageViewModel()
        {
            Messenger.Default.Register<int>(this, LoadDefectId);
            Messenger.Default.Register<string>(this, LoadFolderPath);
        }

        private void LoadDefectId(int currentDefectIndex)
        {
            ReceivedDefectID = currentDefectIndex;
        }
        private void LoadFolderPath(string folderPath)
        {
            ReceivedFolderPath = folderPath;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
