using System;
using System.IO;

using Emgu.CV;
using Emgu.CV.CvEnum;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.Preprocessing
{
    internal class ImageAccess
    {
        public void ParseImage(string Path)
        {
            if (!Directory.Exists(Path))
            {
                throw new DirectoryNotFoundException($"The directory '{Path}' does not exist.");
            }

            WriteLine($"Processing Path.");
            ProcessDirectory(Path);
        }

        private void ProcessDirectory(string Path)
        {
            string[] subdirectories = Directory.GetDirectories(Path);
            foreach (string subdirectory in subdirectories)
            {
                ProcessImage(subdirectory);
            }
        }

        private void ProcessImage(string subDirectoryPath)
        {
            string[] filePaths = Directory.GetFiles(subDirectoryPath);
            foreach (string filePath in filePaths)
            {
                if (Path.GetExtension(filePath) == ".jpg" || Path.GetExtension(filePath) == ".png")
                {
                    // Load the image using Emgu CV
                    Mat image = CvInvoke.Imread(filePath, ImreadModes.Color);

                    WriteLine($"Processing {filePath}.");
                    image.Dispose();
                }
            }
        }
    }
}