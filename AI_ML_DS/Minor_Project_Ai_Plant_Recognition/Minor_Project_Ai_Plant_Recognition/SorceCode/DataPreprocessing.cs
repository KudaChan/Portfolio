using System;
using System.Drawing;
using System.Globalization;
using System.IO;

using Emgu.CV;
using Emgu.CV.CvEnum;
using static System.Net.Mime.MediaTypeNames;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.Preprocessing
{
    /// <summary>
    /// This class is responsible for parsing images from the given directory path.
    /// </summary>
    internal class ImageAccess
    {
        /// <summary>
        /// This method is responsible for parsing images from the given directory path.
        /// It create paths for leaf and plant dataset and then pass the complete path to the ParseImage method.
        /// </summary>
        /// <param name="path"> The base path of the dataset. </param>
        public void BaseDirectory(string path)
        {
            string pathLeaf = path + "\\Medicinal_Leaf_dataset";
            string pathPlant = path + "\\Medicinal_plant_dataset";

            ImageAccess imageAccess = new ImageAccess();
            imageAccess.ParseImage(pathLeaf);
            imageAccess.ParseImage(pathPlant);
        }

        /// <summary>
        /// This method is responsible for checking if the given path is a correct directory or not. Means check for the existance of the path.
        /// If the directory is correct and exist then it will call the ProcessDirectory method.
        /// </summary>
        /// <param name="Path"></param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        private void ParseImage(string Path)
        {
            if (!Directory.Exists(Path))
            {
                throw new DirectoryNotFoundException($"The directory '{Path}' does not exist.");
            }

            WriteLine($" Stating point: Processing Path.");
            ProcessDirectory(Path);
        }

        /// <summary>
        /// This method is responsible for processing or chacking whether the given directory have and subdirectories or not and then call ProcessImage method.
        /// </summary>
        /// <param name="Path"></param>
        private void ProcessDirectory(string Path)
        {
            string[] subdirectories = Directory.GetDirectories(Path);
            foreach (string subdirectory in subdirectories)
            {
                ProcessImage(subdirectory);
            }
        }

        /// <summary>
        /// This method is responsible for processing the images from the given subdirectory path.
        /// </summary>
        /// <param name="subDirectoryPath"></param>
        private void ProcessImage(string subDirectoryPath)
        {
            string[] filePaths = Directory.GetFiles(subDirectoryPath);
            foreach (string filePath in filePaths)
            {
                if (Path.GetExtension(filePath) == ".jpg" || Path.GetExtension(filePath) == ".png")
                {
                    WriteLine($"Processing {filePath}.");

                    ImageResize imageResize = new ImageResize();
                    imageResize.Resizer(filePath);
                }
            }
        }
    }

    internal class ImageResize
    {
        public string outputDirectory = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(resized)";

        public void Resizer(string path)
        {
            WriteLine($"In ImageResize");
            Resizer_244_244(path);
            Resizer_299_299(path);
        }

        private void Resizer_244_244(string path)
        {
            int width = 244;
            int height = 244;

            Mat image = CvInvoke.Imread(path, ImreadModes.Color);
            Mat resizedImage = new Mat();
            CvInvoke.Resize(image, resizedImage, new Size(width, height), 0, 0, Inter.Lanczos4);

            DirectoryInfo? dirParentInfo = new DirectoryInfo(path).Parent?.Parent;
            DirectoryInfo? dirInfo = new DirectoryInfo(path).Parent;
            string? dirName = null;
            string? dirParentName = null;
            if (dirInfo is not null && dirParentInfo is not null)
            {
                dirParentName = dirParentInfo.Name;
                dirName = dirInfo.Name;
            }
            string imgName = Path.GetFileName(path);
            WriteLine(imgName);
            outputDirectory = outputDirectory + "\\size244_244" + "\\" + dirParentName + "\\" + dirName;

            NewImageWrite newImageWrite = new NewImageWrite();
            newImageWrite.DirrectoryCreate(outputDirectory, resizedImage, imgName);
        }

        private void Resizer_299_299(string path)
        {
            int width = 299;
            int height = 299;

            Mat image = CvInvoke.Imread(path, ImreadModes.Color);
            Mat resizedImage = new Mat();
            CvInvoke.Resize(image, resizedImage, new Size(width, height), 0, 0, Inter.Lanczos4);

            DirectoryInfo? dirParentInfo = new DirectoryInfo(path).Parent?.Parent;
            DirectoryInfo? dirInfo = new DirectoryInfo(path).Parent;
            string? dirName = null;
            string? dirParentName = null;
            if (dirInfo is not null && dirParentInfo is not null)
            {
                dirParentName = dirParentInfo.Name;
                dirName = dirInfo.Name;
            }
            string imgName = Path.GetFileName(path);
            WriteLine(imgName);
            outputDirectory = outputDirectory + "\\299_299" + "\\" + dirParentName + "\\" + dirName;

            NewImageWrite newImageWrite = new NewImageWrite();
            newImageWrite.DirrectoryCreate(outputDirectory, resizedImage, imgName);
        }
    }
}

internal class NewImageWrite
{
    public void DirrectoryCreate(string path, Mat newImg, string imgName)
    {
        if (!Directory.Exists(path))
        {
            try
            {
                Directory.CreateDirectory(path);
                WriteLine($"Directory Created Successfully: {path}");
            }
            catch (Exception e)
            {
                WriteLine($"Failed to create directory: {e}");
            }
        }
        else
        {
            WriteLine($"Directory Already Exists: {path}");
        }

        WriteImage(path, newImg, imgName);
    }

    private void WriteImage(string path, Mat newImg, string imgName)
    {
        string newPath = Path.Combine(path, imgName);

        // Write the image to a file
        try
        {
            CvInvoke.Imwrite(newPath, newImg);
            WriteLine($"Image written successfully: {newPath}");
        }
        catch (IOException e)
        {
            WriteLine($"Failed to write image: {e}");
        }
    }
}