using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Drawing;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

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
        public void DirectoryParser(string path, string pathTextFile, string action)
        {
            string[] directories;
            List<string> directoriesList = pathMakerBasedOnAction(path, action);
            directories = directoriesList.ToArray();

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

        /// <summary>
        /// Constructs a list of directories based on the specified action.
        /// </summary>
        /// <param name="path">The base path to use when constructing the directories.</param>
        /// <param name="action">The action to base the directories on. Can be "resize", "parse", or "Augment".</param>
        /// <returns>A list of directories constructed based on the specified action.</returns>
        public List<string> pathMakerBasedOnAction(string path, string action)
        {
            List<string> directories = new List<string>();

            if (action == "Resize" || action == "parse")
            {
                Console.WriteLine("in resize or parse:");
                directories.Add(Path.Combine(path, "Medicinal_Leaf_dataset"));
                directories.Add(Path.Combine(path, "Medicinal_plant_dataset"));
            }
            else if (action == "Augment")
            {
                Console.WriteLine("in augment: ");
                directories.Add(Path.Combine(path, "size224_224"));
                directories.Add(Path.Combine(path, "size299_299"));

                foreach (string directory in directories.ToList()) // Use ToList to create a copy for iteration
                {
                    directories.AddRange(pathMakerBasedOnAction(directory, "parse"));
                }
            }
            else if (action == "BckgrndRemove")
            {
                Console.WriteLine("in background remover: ");
                List<string> dirNames = new List<string> { "flipped", "noised", "resized", "rotated", "translated" };
                List<string> dimension = new List<string> { "size224_224", "size299_299" };
                foreach (string dirName in dirNames)
                {
                    foreach (string dim in dimension)
                    {
                        directories.Add(Path.Combine(path, dirName, dim));
                    }
                }

                foreach (string directory in directories.ToList()) // Use ToList to create a copy for iteration
                {
                    directories.AddRange(pathMakerBasedOnAction(directory, "parse"));
                }
            }

            foreach (string directory in directories)
            {
                Console.WriteLine(directory);
            }

            return directories;
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
        private readonly NewImageWrite _newImageWrite = new NewImageWrite();

        /// <summary>
        /// Augments the images specified in the text file at the given path.
        /// </summary>
        /// <param name="path">The path of the text file containing the image paths.</param>
        public void AugmentFactory(string path)
        {
            using (StreamReader reader = new StreamReader(Path.Combine(path, "dataset.txt")))
            {
                string line;
                while ((line = reader.ReadLine()!) != null)
                {
                    FlipAugmenter(line);
                    RotateAugmenter(line);
                    NoiseAugmenter(line);
                    TranslateAugmenter(line);
                    CopyResizedImage(line);
                }
            }
        }

        /// <summary>
        /// Augments an image by flipping it.
        /// </summary>
        /// <param name="path">The path of the image to augment.</param>
        private void FlipAugmenter(string path)
        {
            string action = "flipped";
            Mat image = CvInvoke.Imread(path, ImreadModes.Color);
            Mat flippedImage = new Mat();
            CvInvoke.Flip(image, flippedImage, FlipType.Both);

            ImageWriterAssistance(path, flippedImage, action);
        }

        /// <summary>
        /// Augments an image by rotating it.
        /// </summary>
        /// <param name="path">The path of the image to augment.</param>
        private void RotateAugmenter(string path)
        {
            string action = "rotated";
            Mat image = CvInvoke.Imread(path, ImreadModes.Color);
            Mat rotatedImage = new Mat();
            CvInvoke.Rotate(image, rotatedImage, RotateFlags.Rotate90Clockwise);

            ImageWriterAssistance(path, rotatedImage, action);
        }

        /// <summary>
        /// Augments an image by adding noise to it.
        /// </summary>
        /// <param name="path">The path of the image to augment.</param>
        private void NoiseAugmenter(string path)
        {
            string action = "noised";
            Mat image = CvInvoke.Imread(path, ImreadModes.Color);
            Mat noisedImage = new Mat();
            CvInvoke.GaussianBlur(image, noisedImage, new Size(5, 5), 0);

            ImageWriterAssistance(path, noisedImage, action);
        }

        /// <summary>
        /// Augments an image by translating it.
        /// </summary>
        /// <param name="path">The path of the image to augment.</param>
        private void TranslateAugmenter(string path)
        {
            string action = "translated";
            Mat image = CvInvoke.Imread(path, ImreadModes.Color);
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            Mat translatedImage = new Mat();
            Matrix<float> shiftMatrix = new Matrix<float>(2, 3);
            Random rand = new Random();
            int shiftX = rand.Next(-50, 50);  // Random shift in x direction between -50 and 50
            int shiftY = rand.Next(-50, 50);  // Random shift in y direction between -50 and 50

            shiftMatrix[0, 0] = 1;  // Identity element
            shiftMatrix[1, 1] = 1;  // Identity element
            shiftMatrix[0, 2] = shiftX;  // Shift in x direction
            shiftMatrix[1, 2] = shiftY;  // Shift in y direction
            CvInvoke.WarpAffine(image, translatedImage, shiftMatrix, new Size(imageWidth, imageHeight), 0);

            ImageWriterAssistance(path, translatedImage, action);
        }

        /// <summary>
        /// Augments an image by copying it to the output directory.
        /// </summary>
        /// <param name="path"></param>
        private void CopyResizedImage(string path)
        {
            string action = "resized";
            Mat image = CvInvoke.Imread(path, ImreadModes.Color);
            Mat copyResizedImage = image;

            ImageWriterAssistance(path, copyResizedImage, action);
        }

        /// <summary>
        /// The directory to output the augmented images to.
        /// </summary>
        public string OutputDirectory { get; } = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(augmented)";

        /// <summary>
        /// Assists in writing the augmented image to the output directory.
        /// </summary>
        /// <param name="path">The path of the original image.</param>
        /// <param name="newImg">The augmented image.</param>
        private void ImageWriterAssistance(string path, Mat newImg, string action)
        {
            DirectoryInfo? directoryParentParentInfo = new DirectoryInfo(path).Parent?.Parent?.Parent;
            DirectoryInfo? dirParentInfo = new DirectoryInfo(path).Parent?.Parent;
            DirectoryInfo? dirInfo = new DirectoryInfo(path).Parent;
            string? dirName = dirInfo?.Name!;
            string? dirParentName = dirParentInfo?.Name!;
            string? dirParentParentName = directoryParentParentInfo?.Name!;
            string imgName = Path.GetFileName(path);
            string specificOutputDirectory = Path.Combine(OutputDirectory, action, dirParentParentName, dirParentName, dirName);

            _newImageWrite.DirrectoryCreate(specificOutputDirectory, newImg, imgName);
        }
    }

    /// <summary>
    /// This class is responsible for removing the background from images.
    /// </summary>
    internal class BckgrndRemover
    {
        private readonly NewImageWrite _newImageWrite = new NewImageWrite();

        /// <summary>
        /// Removes the background from the images specified in the text file at the given path.
        /// </summary>
        /// <param name="path">The path of the text file containing the image paths.</param>
        public void RemoveBackgroundFactory(string path)
        {
            using (StreamReader reader = new StreamReader(Path.Combine(path, "dataset.txt")))
            {
                string line;
                while ((line = reader.ReadLine()!) != null)
                {
                    PythonScriptRemoveBackground(line);
                }
            }
        }

        private void PythonScriptRemoveBackground(string path)
        {
            var engine = Python.CreateEngine();

            var pyEnvPath = engine.GetSearchPaths();
            pyEnvPath.Add("D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\proPyEnv\\Lib\\site-packages");
            engine.SetSearchPaths(pyEnvPath);

            var scope = engine.CreateScope();
            var source = engine.CreateScriptSourceFromFile("D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\SorceCode\\remove_background.py");
            var compilation = source.Compile();
            compilation.Execute(scope);
            dynamic removeBackground = scope.GetVariable("remove_background");
            Mat resultImage = removeBackground(path);

            ImageWriterAssistance(path, resultImage);
        }

        /// <summary>
        /// The directory to output the images with the background removed to.
        /// </summary>
        public string OutputDirectory { get; } = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(background_removed)";

        /// <summary>
        /// Assists in writing the image with the background removed to the output directory.
        /// </summary>
        /// <param name="path">The path of the original image.</param>
        /// <param name="newImg">The image with the background removed.</param>
        private void ImageWriterAssistance(string path, Mat newImg)
        {
            DirectoryInfo? dirParentParentParentInfo = new DirectoryInfo(path).Parent?.Parent?.Parent?.Parent;
            DirectoryInfo? dirParentParentInfo = new DirectoryInfo(path).Parent?.Parent?.Parent;
            DirectoryInfo? dirParentInfo = new DirectoryInfo(path).Parent?.Parent;
            DirectoryInfo? dirInfo = new DirectoryInfo(path).Parent;
            string? dirName = dirInfo?.Name!;
            string? dirParentName = dirParentInfo?.Name!;
            string? dirParentParentName = dirParentParentInfo?.Name!;
            string? dirParentParentParentName = dirParentParentParentInfo?.Name!;
            string imgName = Path.GetFileName(path);
            string specificOutputDirectory = Path.Combine(OutputDirectory, dirParentParentParentName, dirParentParentName, dirParentName, dirName);

            _newImageWrite.DirrectoryCreate(specificOutputDirectory, newImg, imgName);
        }
    }
}