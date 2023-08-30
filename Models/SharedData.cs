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
        #region[필드]
        private static SharedData instance = null;
        private Wafer waferData;
        private string folderPath;
        private bool defectShowData;
        private int defectIndexData;
        public event PropertyChangedEventHandler PropertyChanged;
        private Tuple<int, int> selectedCoordinate;
        #endregion
        #region[속성]
        /**
         * @brief 선택된 die의 좌표값 저장
         * @param value 새로운 die의 좌표값
         * @return 없음
         * 2023-08-29|이은진|선택된 die의 좌표값 저장
         */
        public Tuple<int, int> SelectedCoordinate
        {
            get => selectedCoordinate;
            set
            {
                if (selectedCoordinate != value)
                {
                    selectedCoordinate = value;
                    OnPropertyChanged("SelectedCoordinate");
                }
            }
        }
        /**
         * @brief 데이터 폴더 경로를 가져오거나 설정
         * @param value 새로운 데이터 폴더 경로
         * @return 없음
         * 2023-08-28|이은진|데이터 폴더 경로 string 값으로 처리
         */
        public string FolderPath 
        {
            get { return folderPath; }
            set
            {
                folderPath = value;
                OnPropertyChanged("FolderPath");
            }
        }
        /**
         * @brief 웨이퍼 데이터를 가져오거나 설정
         * @param value 새로운 웨이퍼 데이터
         * @return 없음
         */
        public Wafer WaferData
        {
            get { return waferData; }
            set
            {
                waferData = value;
                OnPropertyChanged("WaferData");
            }
        }
        /**
         * @brief 디팩 이미지 표시 여부를 가져오거나 설정
         * @param value 새로운 디팩 이미지 표시 여부
         * @return 없음
         */
        public bool DefectShowData
        {
            get { return defectShowData; }
            set
            {
                defectShowData = value;
                OnPropertyChanged("DefectShowData");
            }
        }
        /**
         * @brief 디팩 인덱스 데이터를 가져오거나 설정
         * @param value 새로운 디팩 인덱스 데이터
         * @return 없음
         */
        public int DefectIndexData
        {
            get { return defectIndexData; }
            set
            {
                defectIndexData = value;
                OnPropertyChanged("DefectIndexData");
            }
        }
        #endregion
        #region[생성자]
        /**
         * @brief SharedData의 인스턴스를 반환
         * @return SharedData의 인스턴스
         */
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
        #endregion
        #region[protected, private 메서드]
        /**
         * @brief 속성 변경을 알린다.
         * @param name 변경된 속성의 이름
         * @return 없음
         */
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
