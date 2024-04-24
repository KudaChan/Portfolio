using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Python.Runtime;
using System.Drawing;

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

            if (action == "Augment" || action == "BckgrndRemove" || action == "Normalization")
            {
                DirParserByImgDim(directories, pathTextFile);
            }
            else if (action == "Resize" || action == "parse")
            {
                DirParserByClass(directories, pathTextFile);
            }
            else
            {
                throw new ArgumentOutOfRangeException($"The action '{action}' does not exist.");
            }
        }

        public void DirParserByClass(string[] directories, string pathTextFile)
        {
            if (File.Exists(Path.Combine(pathTextFile, "dataset_base.txt")))
            {
                File.Delete(Path.Combine(pathTextFile, "dataset_base.txt"));
            }
            using (StreamWriter writer = new(Path.Combine(pathTextFile, "dataset_base.txt"), true))
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

        public void DirParserByImgDim(string[] directories, string pathTextFile)
        {
            //using (StreamWriter writer = new(Path.Combine(pathTextFile, $"dataset_.{parentName}txt"), false))
            if (File.Exists(Path.Combine(pathTextFile, $"dataset_size224_224.txt")))
            {
                File.Delete(Path.Combine(pathTextFile, $"dataset_size224_224.txt"));
            }
            if (File.Exists(Path.Combine(pathTextFile, $"dataset_size299_299.txt")))
            {
                File.Delete(Path.Combine(pathTextFile, $"dataset_size299_299.txt"));
            }

            foreach (string directory in directories)
            {
                if (new DirectoryInfo(directory).Parent!.Name == "size224_224")
                {
                    if (!Directory.Exists(directory))
                    {
                        throw new DirectoryNotFoundException($"The directory '{directory}' does not exist.");
                    }
                    using (StreamWriter writer = new(Path.Combine(pathTextFile, $"dataset_size224_224.txt"), true))
                    {
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
                else if (new DirectoryInfo(directory).Parent!.Name == "size299_299")
                {
                    if (!Directory.Exists(directory))
                    {
                        throw new DirectoryNotFoundException($"The directory '{directory}' does not exist.");
                    }
                    using (StreamWriter writer = new(Path.Combine(pathTextFile, $"dataset_size299_299.txt"), true))
                    {
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
                else
                {
                    throw new DirectoryNotFoundException($"The directory does not exist.");
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
            List<string> tempDir = new List<string>();

            if (action == "Resize" || action == "parse")
            {
                directories.Add(Path.Combine(path, "Medicinal_Leaf_dataset"));
                directories.Add(Path.Combine(path, "Medicinal_plant_dataset"));
            }
            else if (action == "Augment")
            {
                tempDir.Add(Path.Combine(path, "size224_224"));
                tempDir.Add(Path.Combine(path, "size299_299"));

                foreach (string dir in tempDir.ToList()) // Use ToList to create a copy for iteration
                {
                    directories.AddRange(pathMakerBasedOnAction(dir, "parse"));
                }
            }
            else if (action == "BckgrndRemove")
            {
                List<string> dirNames = new List<string> { "flipped", "noised", "resized", "rotated", "translated" };
                List<string> dimension = new List<string> { "size224_224", "size299_299" };
                foreach (string dirName in dirNames)
                {
                    foreach (string dim in dimension)
                    {
                        tempDir.Add(Path.Combine(path, dirName, dim));
                    }
                }

                foreach (string dir in tempDir.ToList()) // Use ToList to create a copy for iteration
                {
                    directories.AddRange(pathMakerBasedOnAction(dir, "parse"));
                }
            }
            else if (action == "Normalization")
            {
                List<string> dirNames = new List<string> { "flipped", "noised", "resized", "rotated", "translated" };
                List<string> dimension = new List<string> { "size224_224", "size299_299" };
                foreach (string dirName in dirNames)
                {
                    foreach (string dim in dimension)
                    {
                        tempDir.Add(Path.Combine(path, dirName, dim));
                    }
                }

                foreach (string dir in tempDir.ToList()) // Use ToList to create a copy for iteration
                {
                    directories.AddRange(pathMakerBasedOnAction(dir, "parse"));
                }
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
        public static void DirrectoryCreate(string path, Mat newImg, string imgName)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to create directory: {e}");
                }
            }
            else
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
        private static void WriteImage(string path, Mat newImg, string imgName)
        {
            string newPath = Path.Combine(path, imgName);

            try
            {
                CvInvoke.Imwrite(newPath, newImg);
            }
            catch (IOException e)
            {
                Console.WriteLine("In NewImageWriter");
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
            string textPath = Path.Combine(path, "dataset_base.txt");

            ResizeDirectory(textPath, 224, 224);
            ResizeDirectory(textPath, 299, 299);
        }

        public void ResizeDirectory(string path, int width, int height)
        {
            var lines = File.ReadLines(path);
            Parallel.ForEach(lines, (line) =>
            {
                Resizer(line, width, height);
            });
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

            NewImageWrite.DirrectoryCreate(specificOutputDirectory, newImg, imgName);
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
            string path224 = Path.Combine(path, "dataset_size224_224.txt");
            string path299 = Path.Combine(path, "dataset_size299_299.txt");

            Task path224Task = Task.Run(() => AugmentDirectory(path224));
            Task path299Task = Task.Run(() => AugmentDirectory(path299));

            Task.WaitAll(path224Task, path299Task);
        }

        public void AugmentDirectory(string path)
        {
            int totalLines = File.ReadLines(path).Count();
            {
                using (StreamReader reader = new StreamReader(path))
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

            NewImageWrite.DirrectoryCreate(specificOutputDirectory, newImg, imgName);
        }
    }

    /// <summary>
    /// This class is responsible for removing the background from images.
    /// </summary>
    internal class BckgrndRemover
    {
        private readonly NewImageWrite _newImageWrite = new NewImageWrite();

        public void RemoveBackgroundFactory(string path)
        {
            string path224 = Path.Combine(path, "dataset_size224_224.txt");
            string path299 = Path.Combine(path, "dataset_size299_299.txt");

            RemoveBackgroundDirectory(path224);
            RemoveBackgroundDirectory(path299);
        }

        /// <summary>
        /// Removes the background from the images specified in the text file at the given path.
        /// </summary>
        /// <param name="path">The path of the text file containing the image paths.</param>
        public void RemoveBackgroundDirectory(string path)
        {
            // Path to the Python DLL
            Runtime.PythonDLL = @"C:\Users\kumar\AppData\Local\Programs\Python\Python312\python312.dll";
            // Setting the PYTHONHOME environment variable
            Environment.SetEnvironmentVariable("PYTHONHOME", @"C:\Users\kumar\AppData\Local\Programs\Python\Python312", EnvironmentVariableTarget.Process);

            try
            {
                // Initialize the Python Engine
                PythonEngine.Initialize();

                int totalLines = File.ReadLines(path).Count();

                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    while ((line = reader.ReadLine()!) != null)
                    {
                        PythonScriptRemoveBackground(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to remove background: {e.Message}");
            }
            finally
            {
                // Shutdown the Python Engine
                PythonEngine.Shutdown();
            }
        }

        /// <summary>
        /// Executes a Python script to remove the background from an image.
        /// </summary>
        /// <param name="path">The path of the image to process.</param>
        private void PythonScriptRemoveBackground(string path)
        {
            using (Py.GIL())
            {
                // Get the output path for the processed image
                string outputPath = ImageWriterAssistance(path);

                // Read the Python script
                string pythonScript = System.IO.File.ReadAllText(@"D:\Project\AI_ML_DS\Minor_Project_Ai_Plant_Recognition\Minor_Project_Ai_Plant_Recognition\SorceCode\remove_background.py");
                // Run the Python script
                PythonEngine.RunSimpleString(pythonScript);
                // Call the remove_backgrnd function from the Python script
                dynamic removeBackground = PythonEngine.RunSimpleString($"remove_backgrnd(r'{path}', r'{outputPath}')");
            }
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
        private string ImageWriterAssistance(string path)
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

            return Path.Combine(specificOutputDirectory, imgName);
            // Regular image write is not needed.
            //_newImageWrite.DirrectoryCreate(specificOutputDirectory, newImg, imgName);
        }
    }

    /// <summary>
    /// This class is responsble for normalization of images data.
    /// </summary>
    internal class Normalization
    {
        private readonly NewImageWrite _newImageWrite = new NewImageWrite();

        /// <summary>
        /// Normalizes the images specified in the text file at the given path.
        /// </summary>
        /// <param name="path">The path of the text file containing the image paths.</param>
        public void NormalizationFactor(string path)
        {
            string path224 = Path.Combine(path, "dataset_size224_224.txt");
            string path299 = Path.Combine(path, "dataset_size299_299.txt");

            Task path224Task = Task.Run(() => NormalizeDirectory(path224, "224Task"));
            Task path299Task = Task.Run(() => NormalizeDirectory(path299, "299Task"));

            Task.WaitAll(path224Task, path299Task);
            //int totalLines = File.ReadLines(Path.Combine(path, "dataset.txt")).Count();

            //using (var pbar = new ProgressBar(totalLines, "Resizing", new ProgressBarOptions { ProgressCharacter = '#' }))
            //{
            //    using (StreamReader reader = new StreamReader(Path.Combine(path, "dataset.txt")))
            //    {
            //        string line;
            //        while ((line = reader.ReadLine()!) != null)
            //        {
            //            NormalizeColorWise(line);
            //            pbar.Tick();
            //        }
            //    }
            //}
        }

        private void NormalizeDirectory(string path, string taskName)
        {
            if (taskName == "224Task")
            {
                int totalLines = File.ReadLines(path).Count();
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        string line;
                        while ((line = reader.ReadLine()!) != null)
                        {
                            NormalizeColorWise(line);
                        }
                    }
                }
            }
            else if (taskName == "299Task")
            {
                int totalLines = File.ReadLines(path).Count();
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        string line;
                        while ((line = reader.ReadLine()!) != null)
                        {
                            NormalizeColorWise(line);
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException($"The task '{taskName}' does not exist.");
            }
        }

        /// <summary>
        /// Normalizes an image using min-max.
        /// </summary>
        /// <param name="path">The path of the image to process.</param>
        public void NormalizeColorWise(string path)
        {
            Mat image = CvInvoke.Imread(path, ImreadModes.Color);

            Mat normalizedImage = image.Clone();
            VectorOfMat vm = new VectorOfMat();
            CvInvoke.Split(image, vm);

            for (int i = 0; i < vm.Size; i++)
            {
                Mat channel = vm[i];
                CvInvoke.Normalize(channel, channel, 0, 255, NormType.MinMax);
            }
            CvInvoke.Merge(vm, normalizedImage);

            ImageWriterAssistance(path, normalizedImage);
        }

        /// <summary>
        /// The directory to output the normalized images to the directory.
        /// </summary>
        public string OutputDirectory { get; } = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(normalized)";

        /// <summary>
        /// Assists in writing the normalized image to the output directory.
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

            NewImageWrite.DirrectoryCreate(specificOutputDirectory, newImg, imgName);
        }
    }
}