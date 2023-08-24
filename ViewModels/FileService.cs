using assignment2_LEJ.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using System.Globalization;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Forms;

namespace assignment2_LEJ.ViewModels
{
    public class FileService : INotifyPropertyChanged
    {
        public ICommand OpenCommand { get; private set; }
        public ObservableCollection<string> FileList { get; private set; }
        private string selectedFile;
        private static FileService instance;
        private string folderPath;
        private Wafer currentLoadedWafer;
        public Wafer CurrentLoadedWafer
        {
            get { return currentLoadedWafer; }
            set
            {
                if (currentLoadedWafer != value)
                {
                    currentLoadedWafer = value;
                    OnPropertyChanged("CurrentLoadedWafer");
                }
            }
        }

        public static FileService Instance
        {
            get
            {
                if (instance == null)
                    instance = new FileService();
                return instance;
            }
        }
        public string SelectedFile
        {
            get { return selectedFile; }
            set
            {
                if (selectedFile != value)
                {
                    selectedFile = value;
                    OnPropertyChanged("SelectedFile");
                    LoadSelectedFile(value);
                }
            }
        }
        public FileService()
        {
            OpenCommand = new RelayCommand(OpenFolder);
            FileList = new ObservableCollection<string>();
        }

        private void OpenFolder(object parameter)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowser.SelectedPath;
                LoadFiles(folderPath);
                Messenger.Default.Send<string>(folderPath);
            }
        }
        private void LoadSelectedFile(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                LoadFromFile(filePath);
            }
        }

        private void LoadFiles(string folderPath)
        {
            var files = Directory.GetFiles(folderPath, "*.001");

            FileList.Clear();
            foreach (var file in files)
            {
                FileList.Add(file);
            }
        }

        public Wafer LoadFromFile(string filePath)
        {
            Wafer wafer = new Wafer();
            wafer.Dies = new List<Die>(); 

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {

                    if (line.StartsWith("FileVersion"))
                    {
                        line = line.TrimEnd(';');
                        string[] parts = line.Split(' ');
                        wafer.FileVersion = string.Join(" ", parts.Skip(1)); 
                    }
                    else if (line.StartsWith("ResultTimestamp"))
                    {
                        line = line.TrimEnd(';');
                        string[] parts = line.Split(' ');
                        wafer.ResultTimestamp = DateTime.Parse(string.Join(" ", parts.Skip(1))); 
                    }
                    else if (line.StartsWith("TiffSpec"))
                    {
                        line = line.TrimEnd(';');
                        string[] parts = line.Split(' ');
                        string version = parts[1];
                        List<string> colors = parts.Skip(2).ToList(); 
                        wafer.TiffSpec = new Tuple<string, List<string>>(version, colors);
                    }
                    else if (line.StartsWith("InspectionStationID"))
                    {
                        line = line.TrimEnd(';');
                        string[] ids = line.Split('"').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                        wafer.InspectionStationID.Clear();
                        for (int i = 1; i < ids.Length; i += 2)
                        {
                            wafer.InspectionStationID.Add(ids[i]);
                        }
                    }
                    else if (line.StartsWith("FileTimestamp"))
                    {
                        line = line.TrimEnd(';');
                        string[] parts = line.Split(' ');
                        wafer.FileTimestamp = DateTime.Parse(parts[1] + " " + parts[2].TrimEnd(';'));
                    }
                    else if (line.StartsWith("LotID"))
                    {
                        line = line.TrimEnd(';');
                        wafer.LotID = line.Split('"')[1];
                    }
                    else if (line.StartsWith("SampleSize"))
                    {
                        line = line.TrimEnd(';');
                        string[] parts = line.Split(' ');
                        wafer.SampleSize = Tuple.Create(int.Parse(parts[1]), int.Parse(parts[2].TrimEnd(';')));
                    }
                    else if (line.StartsWith("SetupID"))
                    {
                        line = line.TrimEnd(';');
                        string[] parts = line.Split(' ');
                        wafer.SetupID = parts[1].Trim('"'); 
                        string dateFormat = "MM-dd-yy HH:mm:ss";
                        wafer.SetupDate = DateTime.ParseExact(parts[2] + " " + parts[3].TrimEnd(';'), dateFormat, CultureInfo.InvariantCulture);
                    }
                    else if (line.StartsWith("StepID"))
                    {
                        line = line.TrimEnd(';');
                        wafer.StepID = line.Split('"')[1];
                    }
                    else if (line.StartsWith("SampleOrientationMarkType"))
                    {
                        line = line.TrimEnd(';');
                        wafer.SampleOrientationMarkType = line.Split(' ')[1].TrimEnd(';');
                    }
                    else if (line.StartsWith("OrientationMarkLocation"))
                    {
                        line = line.TrimEnd(';');
                        wafer.OrientationMarkLocation = line.Split(' ')[1].TrimEnd(';');
                    }
                    else if (line.StartsWith("TiffFilename"))
                    {
                        line = line.TrimEnd(';');
                        wafer.TiffFilename = line.Split(' ')[1].TrimEnd(';');
                    }
                    else if (line.StartsWith("DiePitch"))
                    {
                        line = line.TrimEnd(';');
                        string[] parts = line.Split(' ');
                        wafer.DiePitch = Tuple.Create(double.Parse(parts[1]), double.Parse(parts[2].TrimEnd(';')));
                    }
                    else if (line.StartsWith("DieOrigin"))
                    {
                        line = line.TrimEnd(';');
                        string[] parts = line.Split(' ');
                        wafer.DieOrigin = Tuple.Create(double.Parse(parts[1]), double.Parse(parts[2].TrimEnd(';')));
                    }
                    else if (line.StartsWith("WaferID"))
                    {
                        line = line.TrimEnd(';');
                        wafer.WaferID = line.Split('"')[1];
                    }
                    else if (line.StartsWith("Slot"))
                    {
                        line = line.TrimEnd(';');
                        wafer.Slot = line.Split(' ')[1].TrimEnd(';');
                    }
                    else if (line.StartsWith("SampleCenterLocation"))
                    {
                        line = line.TrimEnd(';');
                        string[] parts = line.Split(' ');
                        wafer.SampleCenterLocation = Tuple.Create(double.Parse(parts[1]), double.Parse(parts[2].TrimEnd(';')));
                    }
                    else if (line.StartsWith("InspectionTest"))
                    {
                        line = line.TrimEnd(';');
                        wafer.InspectionTest = int.Parse(line.Split(' ')[1].TrimEnd(';'));
                    }

                    else if (line.StartsWith("SampleTestPlan"))
                    {
                        string[] parts = line.Split(' ');
                        if (parts.Length > 1 && int.TryParse(parts[1], out int count))
                        {
                            wafer.Count = count;
                            wafer.XMin = int.MaxValue;
                            wafer.YMin = int.MaxValue;
                            wafer.XMax = int.MinValue;
                            wafer.YMax = int.MinValue;
                            for (int i = 0; i < count; i++)
                            {
                                string coordLine = sr.ReadLine();

                                if (i == count - 1)
                                {
                                    coordLine = coordLine.TrimEnd(';');
                                }

                                string[] coords = coordLine.Split(' ');
                                int xCoord = int.Parse(coords[0]);
                                int yCoord = int.Parse(coords[1]);

                                if (xCoord > wafer.XMax) wafer.XMax = xCoord;
                                if (xCoord < wafer.XMin) wafer.XMin = xCoord;
                                if (yCoord > wafer.YMax) wafer.YMax = yCoord;
                                if (yCoord < wafer.YMin) wafer.YMin = yCoord;

                                Die die = new Die
                                {
                                    Coordinate = new Tuple<int, int>(xCoord, yCoord)
                                };
                                wafer.Dies.Add(die);
                            }
                        }
                        else
                        {
                            Console.WriteLine("숫자 예외 처리");
                        }
                    }

                    else if (line.StartsWith("DefectList"))
                    {
                        line = sr.ReadLine();
                        while (line != null && !line.EndsWith(";"))
                        {
                            string[] parts = line.Split(' ');

                            Defect defect = new Defect
                            {
                                DefectID = int.Parse(parts[0]),
                                XREL = double.Parse(parts[1], CultureInfo.InvariantCulture),
                                YREL = double.Parse(parts[2], CultureInfo.InvariantCulture),
                                XINDEX = int.Parse(parts[3]),
                                YINDEX = int.Parse(parts[4]),
                                XSIZE = double.Parse(parts[5]),
                                YSIZE = double.Parse(parts[6]),
                                DEFECTAREA = double.Parse(parts[7]),
                                DSIZE = double.Parse(parts[8]),
                                CLASSNUMBER = int.Parse(parts[9]),
                                TEST = int.Parse(parts[10]),
                                CLUSTERNUMBER = int.Parse(parts[11]),
                                ROUGHBINNUMBER = int.Parse(parts[12]),
                                FINEBINNUMBER = int.Parse(parts[13]),
                                REVIEWSAMPLE = int.Parse(parts[14]),
                                IMAGECOUNT = int.Parse(parts[15]),
                                IMAGELIST = new List<int> { int.Parse(parts[16]) }
                            };

                            line = sr.ReadLine();
                            if (line != null)
                            {
                                if (line.EndsWith(";"))
                                {
                                    line = line.TrimEnd(';');
                                }
                                string[] imageParts = line.Split(' ');
                                defect.IMAGELIST.Add(int.Parse(imageParts[1]));
                            }

                            Die matchingDie = wafer.Dies.FirstOrDefault(d => d.Coordinate.Item1 == defect.XINDEX && d.Coordinate.Item2 == defect.YINDEX);

                            if (matchingDie != null)
                            { 
                                matchingDie.Defects.Add(defect);
                            }
                            line = sr.ReadLine(); 
                        }
                    }

                }
            }
            CurrentLoadedWafer = wafer;
            Messenger.Default.Send<Wafer>(wafer);
            return wafer;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
