﻿using assignment2_LEJ.Models;
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

namespace assignment2_LEJ.ViewModels
{
    public class FileService : INotifyPropertyChanged
    {
        public ICommand OpenCommand { get; private set; }
        public ObservableCollection<string> FileList { get; private set; }
        public string SelectedFile { get; set; }

        public FileService()
        {
            OpenCommand = new RelayCommand(OpenFolder);
            FileList = new ObservableCollection<string>();
        }

        private void OpenFolder(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = false; // 파일이 실제로 존재하는지 검사하지 않습니다.
            openFileDialog.FileName = "[폴더 선택]"; // 기본 파일 이름을 제공합니다.
            openFileDialog.ValidateNames = false; // 유효한 파일 이름만을 허용하지 않습니다.

            if (openFileDialog.ShowDialog() == true)
            {
                // 폴더 경로 가져오기
                string folderPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                LoadFiles(folderPath);
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
                        wafer.SetupDate = DateTime.Parse(parts[2] + " " + parts[3].TrimEnd(';'));
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
                        int count = int.Parse(sr.ReadLine().Trim());
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

                            Die die = new Die
                            {
                                Coordinate = new Tuple<int, int>(xCoord, yCoord)
                            };
                            wafer.Dies.Add(die);
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
                                XREL = double.Parse(parts[1]),
                                YREL = double.Parse(parts[2]),
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
                                string[] imageParts = line.Split(' ');
                                defect.IMAGELIST.Add(int.Parse(imageParts[1])); 
                            }

                            Die dieWithDefect = new Die
                            {
                                Coordinate = new Tuple<int, int>(defect.XINDEX, defect.YINDEX),
                                DefectInfo = defect
                            };

                            wafer.Dies.Add(dieWithDefect);
                            line = sr.ReadLine(); 
                        }
                    }

                }
            }

            return wafer;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
