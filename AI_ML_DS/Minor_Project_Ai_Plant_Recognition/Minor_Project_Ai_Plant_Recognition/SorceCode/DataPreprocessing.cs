using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.IO;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.Preprocessing
{
    /// <summary>
    /// This class is responsible for parsing images from the given directory path.
    /// It writes the paths of the images to a text file.
    /// </summary>
    internal class ImageAccess
    {
        /// <summary>
        /// Parses the directories for image files and writes their paths to a text file.
        /// </summary>
        /// <param name="path">The base path of the directories.</param>
        /// <param name="pathTextFile">The path of the text file to write to.</param>
        public void DirectoryParser(string path, string pathTextFile)
        {
            string[] directories = new string[]
            {
                    Path.Combine(path, "Medicinal_Leaf_dataset"),
                    Path.Combine(path, "Medicinal_plant_dataset")
            };

            Directory.CreateDirectory(pathTextFile);

            using (StreamWriter writer = new StreamWriter(Path.Combine(pathTextFile, "dataset.txt"), false))
            {
                foreach (string directory in directories)
                {
                    if (!Directory.Exists(directory))
                    {
                        throw new DirectoryNotFoundException($"The directory '{directory}' does not exist.");
                    }

                    foreach (string subdirectory in Directory.EnumerateDirectories(directory))
                    {
                        foreach (string filePath in Directory.EnumerateFiles(subdirectory))
                        {
                            string extension = Path.GetExtension(filePath);
                            if (extension == ".jpg" || extension == ".png")
                            {
                                writer.WriteLine(filePath);
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// This class is responsible for writing images to a directory.
    /// </summary>
    internal class NewImageWrite
    {
        /// <summary>
        /// Creates a directory and writes an image to it.
        /// </summary>
        /// <param name="path">The path of the directory to create.</param>
        /// <param name="newImg">The image to write.</param>
        /// <param name="imgName">The name of the image file.</param>
        public void DirrectoryCreate(string path, Mat newImg, string imgName)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to create directory: {e}");
                }
            }

            WriteImage(path, newImg, imgName);
        }

        /// <summary>
        /// Writes an image to a file.
        /// </summary>
        /// <param name="path">The path of the file to write to.</param>
        /// <param name="newImg">The image to write.</param>
        /// <param name="imgName">The name of the image file.</param>
        private void WriteImage(string path, Mat newImg, string imgName)
        {
            string newPath = Path.Combine(path, imgName);

            try
            {
                CvInvoke.Imwrite(newPath, newImg);
                Console.WriteLine($"Image written successfully: {newPath}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"Failed to write image: {e}");
            }
        }
    }

    /// <summary>
    /// This class is responsible for resizing images.
    /// </summary>
    internal class ImageResize
    {
        /// <summary>
        /// The directory to output the resized images to.
        /// </summary>
        public string OutputDirectory { get; } = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(resized)";

        private readonly NewImageWrite _newImageWrite = new NewImageWrite();

        /// <summary>
        /// Resizes the images specified in the text file at the given path.
        /// </summary>
        /// <param name="path">The path of the text file containing the image paths.</param>
        public void ResizeFactory(string path)
        {
            using (StreamReader reader = new StreamReader(Path.Combine(path, "dataset.txt")))
            {
                string line;
                while ((line = reader.ReadLine()!) != null)
                {
                    Resizer(line, 224, 224);
                    Resizer(line, 299, 299);
                }
            }
        }

        /// <summary>
        /// Resizes an image to the specified width and height.
        /// </summary>
        /// <param name="path">The path of the image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        private void Resizer(string path, int width, int height)
        {
            Mat image = CvInvoke.Imread(path, ImreadModes.Color);
            Mat resizedImage = new Mat();
            CvInvoke.Resize(image, resizedImage, new Size(width, height), 0, 0, Inter.Area);

            ImageWriterAssistance(path, resizedImage, width, height);
        }

        /// <summary>
        /// Assists in writing the resized image to the output directory.
        /// </summary>
        /// <param name="path">The path of the original image.</param>
        /// <param name="newImg">The resized image.</param>
        /// <param name="width">The width of the resized image.</param>
        /// <param name="height">The height of the resized image.</param>
        private void ImageWriterAssistance(string path, Mat newImg, int width, int height)
        {
            DirectoryInfo? dirParentInfo = new DirectoryInfo(path).Parent?.Parent;
            DirectoryInfo? dirInfo = new DirectoryInfo(path).Parent;
            string? dirName = dirInfo?.Name!;
            string? dirParentName = dirParentInfo?.Name!;
            string imgName = Path.GetFileName(path);
            string specificOutputDirectory = Path.Combine(OutputDirectory, $"size{width}_{height}", dirParentName, dirName);

            _newImageWrite.DirrectoryCreate(specificOutputDirectory, newImg, imgName);
        }
    }

    /// <summary>
    /// This class is responsible for Data Augmentation.
    /// </summary>
    internal class DataAugmentation
    {
    }
}