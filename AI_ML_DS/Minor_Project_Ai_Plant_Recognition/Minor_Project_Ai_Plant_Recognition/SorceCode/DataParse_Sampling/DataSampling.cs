using Emgu.CV;
using Minor_Project_Ai_Plant_Recognition.SorceCode.DataParse_Sampling.ImageWriter;
using System.Data;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.DataParse_Sampling
{
    internal class DataSampling
    {
        public void SampleDataInitiator(string basePath, string textFilePath)
        {
            string plantDirName = Path.Combine(basePath, "Medicinal_plant_dataset");
            string leafDirName = Path.Combine(basePath, "Medicinal_leaf_dataset");

            string randPlantTextFile = Path.Combine(textFilePath, "plant_dataset.txt");
            string randLeafTextFile = Path.Combine(textFilePath, "leaf_dataset.txt");

            Task plantParsing = Task.Run(() => DirSelector(plantDirName, randPlantTextFile));
            Task leafParsing = Task.Run(() => DirSelector(leafDirName, randLeafTextFile));

            Task.WaitAll(plantParsing, leafParsing);
        }

        private void DirSelector(string basePath, string textFilePath)
        {
            List<string> dirName = new();

            foreach (string dir in Directory.GetDirectories(Path.Combine(basePath)))
            {
                try
                {
                    dirName!.Add(dir);
                }
                catch (Exception e)
                {
                    WriteLine(e.Message);
                }
            }

            RandomDirectorySelector(5, dirName!, textFilePath);
        }

        private void RandomDirectorySelector(int count, List<string> dirName, string textFilePath)
        {
            Random rand = new();
            List<string> randDirName = new();
            if (dirName.Count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    randDirName.Add(dirName[rand.Next(0, dirName.Count)]);
                }
            }
            else
            {
                Exception e = new("Plant and Leaf Directories are empty.");
                WriteLine(e.Message);
            }

            WritingDataToTxtFile(randDirName, textFilePath);
        }

        private void WritingDataToTxtFile(List<string> dirName, string textFilePath)
        {
            string filePath = textFilePath;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Use StreamWriter for more efficient writing
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                Parallel.ForEach(dirName, dir =>
                {
                    if (!Directory.Exists(dir))
                    {
                        throw new DirectoryNotFoundException($"The directory '{dir}' does not exist.");
                    }
                    else
                    {
                        foreach (string file in Directory.EnumerateFiles(dir))
                        {
                            string extension = Path.GetExtension(file);
                            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                            {
                                try
                                {
                                    lock (sw)
                                    {
                                        sw.WriteLine(file);
                                    }
                                }
                                catch (Exception e)
                                {
                                    WriteLine(e.Message);
                                }
                            }
                        }
                    }
                });
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Failed to create the file at '{filePath}'.");
            }
            WritingImageToDirectory(filePath);
        }

        private void WritingImageToDirectory(string textFilePath)
        {
            string outputDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataSampled\\Dataset(5species)";

            if (textFilePath.Contains("plant_dataset.txt"))
            {
                outputDir = Path.Combine(outputDir, "Medicinal_plant_dataset");
            }
            else if (textFilePath.Contains("leaf_dataset.txt"))
            {
                outputDir = Path.Combine(outputDir, "Medicinal_leaf_dataset");
            }

            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }
            using (StreamReader sr = new(textFilePath))
            {
                // Use Parallel.ForEach for parallel processing
                Parallel.ForEach(File.ReadLines(textFilePath), line =>
                {
                    string fileParentInfo = Directory.GetParent(line)!.Name;
                    Mat img = CvInvoke.Imread(line);
                    string specificDirPath = Path.Combine(outputDir, fileParentInfo);

                    //WriteLine(fileParentInfo);
                    //WriteLine(specificDirPath);
                    //WriteLine(Path.GetFileName(line));

                    NewImageWrite.DirrectoryCreate(specificDirPath, img, Path.GetFileName(line));
                });
            }
        }
    }

    internal class DataSplit
    {
        public void DataSplitFactory()
        {
            string srcDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataPreProcessed";
            string DistDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataTestTrainValid";

            if (Directory.Exists(DistDir))
            {
                Directory.Delete(DistDir, true);
            }

            DataDirSplitter(srcDir, Path.Combine(DistDir, "DataSet(train)"), 0.7f);
            DataDirSplitter(srcDir, Path.Combine(DistDir, "DataSet(test)"), 0.2f);
            DataDirSplitter(srcDir, Path.Combine(DistDir, "DataSet(validate)"), 0.1f);
        }

        private void DataDirSplitter(string srcDir, string DistDir, float size)
        {
            var subDirs = Directory.EnumerateDirectories(srcDir, "*", SearchOption.AllDirectories);

            foreach (string subDir in subDirs)
            {
                var files = Directory.GetFiles(subDir);
                if (files.Any(file => Path.GetExtension(file) == ".jpg"))
                {
                    if (!srcDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    {
                        srcDir += Path.DirectorySeparatorChar;
                    }

                    if (subDir.StartsWith(srcDir))
                    {
                        string relativePath = subDir.Substring(srcDir.Length);
                        DataSplitter(subDir, DistDir, size, relativePath);
                    }
                    else
                    {
                        throw new ArgumentException("The path is not a subdirectory of the base directory.");
                    }
                }
            }
        }

        private void DataSplitter(string subDir, string distDir, float size, string dirName)
        {
            distDir = Path.Combine(distDir, dirName);
            if (Directory.Exists(distDir))
            {
                Directory.Delete(distDir, true);
                Directory.CreateDirectory(distDir);
            }
            else
            {
                Directory.CreateDirectory(distDir);
            }

            var files = Directory.GetFiles(subDir);
            int fileCount = files.Length;
            int count = (int)(fileCount * size);

            Random rand = new();
            List<string> randFiles = new();
            if (fileCount > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    randFiles.Add(files[rand.Next(0, fileCount)]);
                }
            }
            else
            {
                Exception e = new("The directory is empty.");
                WriteLine(e.Message);
            }

            foreach (var file in randFiles)
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(distDir, fileName);
                File.Copy(file, destFile, true);
            }
        }
    }
}