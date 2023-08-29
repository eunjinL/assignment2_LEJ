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
        #region[필드]
        private int receivedDefectID = 2;
        private string receivedFolderPath;
        private BitmapSource loadedImage;
        private TiffBitmapDecoder tiffDecoder;
        private bool receivedDefectShow = false;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region[속성]
        /**
        * @brief 받은 폴더 경로 속성
        * @return 현재의 폴더 경로
        * 2023-08-28|이은진|첫 버전 작성
        */
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
        /**
        * @brief 받은 결함 ID 속성
        * @return 현재의 결함 ID
        * 2023-08-28|이은진|첫 버전 작성
        */
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
        /**
        * @brief 로드된 이미지 속성
        * @return 현재 로드된 이미지
        * 2023-08-28|이은진|첫 버전 작성
        */
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
        /**
        * @brief 결함 표시 여부 속성
        * @return 현재의 결함 표시 여부
        * 2023-08-28|이은진|첫 버전 작성
        */
        public bool ReceivedDefectShow
        {
            get { return receivedDefectShow; }
            set
            {
                if (receivedDefectShow != value)
                {
                    receivedDefectShow = value;
                    OnPropertyChanged(nameof(receivedDefectShow));
                    LoadTiffImage();
                }
            }
        }
        /**
        * @brief TIFF 이미지 로드 메서드
        * @return 없음
        * 2023-08-28|이은진|첫 버전 작성
        */
        private void LoadTiffImage()
        {
            if (!Directory.Exists(receivedFolderPath))
                return;

            var tifFiles = Directory.GetFiles(receivedFolderPath, "*.tif");
            if (tifFiles.Length == 0)
                return;

            var tifFile = tifFiles[0];
            tiffDecoder = new TiffBitmapDecoder(new Uri(tifFile, UriKind.Absolute), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            if (tiffDecoder.Frames.Count > 0 && receivedDefectShow)
            {
                LoadedImage = tiffDecoder.Frames[receivedDefectID];
            }
        }
        #endregion
        #region[생성자]
        public DefectImageViewModel()
        {
            SharedData.Instance.PropertyChanged += SharedData_PropertyChanged;
        }
        #endregion

        #region[protected, private 메서드]
        /**
        * @brief 데이터 변경 이벤트 핸들러
        * @param sender 이벤트를 발생시킨 객체
        * @param e PropertyChangedEventArgs 객체로, 변경된 속성 정보 포함
        * @return 없음
        * 2023-08-28|이은진|첫 버전 작성
        */
        private void SharedData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DefectIndexData")
            {
                ReceivedDefectID = SharedData.Instance.DefectIndexData;
            }
            else if (e.PropertyName == "DefectShowData")
            {
                ReceivedDefectShow = SharedData.Instance.DefectShowData;
            }
            else if (e.PropertyName == "FolderPath")
            {
                ReceivedFolderPath = SharedData.Instance.FolderPath;
            }
        }
        /**
        * @brief 속성 변경 이벤트 발생 메서드
        * @param propertyName 변경된 속성의 이름
        * @return 없음
        * 2023-08-28|이은진|첫 버전 작성
        */
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        
        
    }

}
