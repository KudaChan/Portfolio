using Emgu.CV;
using Minor_Project_Ai_Plant_Recognition.SorceCode.DataParse_Sampling.ImageWriter;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.DataParse_Sampling
{
    internal class DataSampling
    {
        public void SampleDataInitiator(string basePath, string textFilePath)
        {
            string plantDirName = Path.Combine(basePath, "Medicinal_leaf_dataset");
            string leafDirName = Path.Combine(basePath, "Medicinal_plant_dataset");

            string randPlantTextFile = Path.Combine(textFilePath, "plant_dataset.txt");
            string randLeafTextFile = Path.Combine(textFilePath, "leaf_dataset.txt");

            Task plantParsing = Task.Run(() => SampleDataFactory(plantDirName, randPlantTextFile));
            Task leafParsing = Task.Run(() => SampleDataFactory(leafDirName, randLeafTextFile));

            Task.WaitAll(plantParsing, leafParsing);
        }

        private void SampleDataFactory(string basePath, string textFilePath)
        {
            DirSelector(basePath, textFilePath);
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
            if (File.Exists(Path.Combine(textFilePath, "dataset_base.txt")))
            {
                File.Delete(Path.Combine(textFilePath, "dataset_base.txt"));
            }

            foreach (string dir in dirName)
            {
                if (dirName.Count == 0)
                {
                    WriteLine("Directory is empty.");
                }
                else
                {
                    WriteLine(dir);
                }
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
                                File.AppendAllText(Path.Combine(textFilePath), $"{file}\n");
                            }
                            catch (Exception e)
                            {
                                WriteLine(e.Message);
                            }
                        }
                    }
                }
            }
            if (!File.Exists(textFilePath))
            {
                throw new FileNotFoundException($"Failed to create the file at '{textFilePath}'.");
            }
            WritingImageToDirectory(textFilePath);

            //string outputDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataSampled\\Dataset(5species)";

            //using (StreamReader sr = new(Path.Combine(textFilePath, "dataset_base.txt")))
            //{
            //    string line;
            //    while ((line = sr.ReadLine()!) != null)
            //    {
            //        DirectoryInfo dirGrandParent = Directory.GetParent(line)!.Parent!.Parent!;
            //        DirectoryInfo dirParent = Directory.GetParent(line)!.Parent!;
            //        string dirParentName = dirParent.Name;
            //        string dirGrandParentName = dirGrandParent.Name;
            //        Mat img = CvInvoke.Imread(line);
            //        string specificDirPath = Path.Combine(outputDir, dirGrandParentName, dirParentName, Path.GetFileName(line));
            //        NewImageWrite.DirrectoryCreate(specificDirPath, img, Path.GetFileName(line));
            //    }
            //}
        }

        private void WritingImageToDirectory(string textFilePath)
        {
            string outputDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataSampled\\Dataset(5species)";

            using (StreamReader sr = new(textFilePath))
            {
                string line;
                while ((line = sr.ReadLine()!) != null)
                {
                    DirectoryInfo dirGrandParent = Directory.GetParent(line)!.Parent!.Parent!;
                    DirectoryInfo dirParent = Directory.GetParent(line)!.Parent!;
                    string dirParentName = dirParent.Name;
                    string dirGrandParentName = dirGrandParent.Name;
                    Mat img = CvInvoke.Imread(line);
                    string specificDirPath = Path.Combine(outputDir, dirGrandParentName, dirParentName, Path.GetFileName(line));
                    NewImageWrite.DirrectoryCreate(specificDirPath, img, Path.GetFileName(line));
                }
            }
        }
    }
}