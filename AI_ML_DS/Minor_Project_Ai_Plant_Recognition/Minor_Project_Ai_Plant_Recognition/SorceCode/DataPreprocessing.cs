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
        /// Parses the directories based on the specified action and writes their paths to a text file.
        /// </summary>
        /// <param name="path">The path of the directory to parse.</param>
        /// <param name="pathTextFile">The path of the text file to write to.</param>
        /// <param name="action">The action to perform. Can be "Augment", "BckgrndRemove", "Normalization", "Resize", or "parse".</param>
        /// <remarks>
        /// This method first creates a list of directories based on the specified action using the pathMakerBasedOnAction method.
        /// Then, it creates a directory at the specified pathTextFile.
        /// Depending on the action, it calls either DirParserByImgDim or DirParserByClass to parse the directories and write their paths to the text file.
        /// If the action is not recognized, an exception is thrown.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified action is not recognized.</exception>
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

        /// <summary>
        /// Parses the directories for image files and writes their paths to a text file.
        /// </summary>
        /// <param name="directories">An array of directories to parse.</param>
        /// <param name="pathTextFile">The path of the text file to write to.</param>
        /// <remarks>
        /// This method first checks if a file named "dataset_base.txt" exists at the specified path. If it does, the file is deleted.
        /// Then, for each directory in the provided array, the method checks if the directory exists. If it does not, an exception is thrown.
        /// For each existing directory, the method enumerates its subdirectories and the files within them. If a file has the extension ".jpg" or ".png",
        /// the path of the file is written to the text file.
        /// </remarks>
        /// <exception cref="DirectoryNotFoundException">Thrown when a specified directory does not exist.</exception>
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

        /// <summary>
        /// Parses the directories for image files based on their dimensions and writes their paths to a text file.
        /// </summary>
        /// <param name="directories">An array of directories to parse.</param>
        /// <param name="pathTextFile">The path of the text file to write to.</param>
        /// <remarks>
        /// This method first checks if a file named "dataset_size224_224.txt" or "dataset_size299_299.txt" exists at the specified path. If it does, the file is deleted.
        /// Then, for each directory in the provided array, the method checks if the directory exists. If it does not, an exception is thrown.
        /// For each existing directory, the method enumerates its subdirectories and the files within them. If a file has the extension ".jpg" or ".png",
        /// the path of the file is written to the text file.
        /// </remarks>
        /// <exception cref="DirectoryNotFoundException">Thrown when a specified directory does not exist.</exception>
        public void DirParserByImgDim(string[] directories, string pathTextFile)
        {
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
        /// Creates a list of directories based on the specified action.
        /// </summary>
        /// <param name="path">The path of the directory to parse.</param>
        /// <param name="action">The action to perform. Can be "Resize", "parse", "Augment", "BckgrndRemove", or "Normalization".</param>
        /// <returns>A list of directories.</returns>
        /// <remarks>
        /// This method creates a list of directories based on the specified action.
        /// If the action is "Resize" or "parse", it adds the "Medicinal_Leaf_dataset" and "Medicinal_plant_dataset" directories to the list.
        /// If the action is "Augment", it adds the "size224_224" and "size299_299" directories to the list.
        /// If the action is "BckgrndRemove" or "Normalization", it adds the "flipped", "noised", "resized", "rotated", and "translated" directories to the list.
        /// </remarks>
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
        /// Creates a directory at the specified path and writes an image to it.
        /// </summary>
        /// <param name="path">The path where the directory will be created.</param>
        /// <param name="newImg">The image to write to the directory.</param>
        /// <param name="imgName">The name of the image file.</param>
        /// <remarks>
        /// This method first checks if a directory exists at the specified path. If it does, the directory is deleted.
        /// Then, it attempts to create a new directory at the same path. If the creation fails, an error message is written to the console.
        /// Finally, it calls the WriteImage method to write the specified image to the newly created directory.
        /// </remarks>
        /// <exception cref="Exception">Thrown when the directory creation fails.</exception>
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
                    WriteLine($"Failed to create directory: {e}");
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
                    WriteLine($"Failed to create directory: {e}");
                }
            }

            WriteImage(path, newImg, imgName);
        }

        /// <summary>
        /// Writes an image to the specified path.
        /// </summary>
        /// <param name="path">The path where the image will be written.</param>
        /// <param name="newImg">The image to write.</param>
        /// <param name="imgName">The name of the image file.</param>
        /// <remarks>
        /// This method first combines the specified path and image name to create a new path.
        /// Then, it attempts to write the specified image to the new path using the CvInvoke.Imwrite method.
        /// If the write operation fails, an error message is written to the console.
        /// </remarks>
        /// <exception cref="IOException">Thrown when the write operation fails.</exception>
        private static void WriteImage(string path, Mat newImg, string imgName)
        {
            string newPath = Path.Combine(path, imgName);

            try
            {
                CvInvoke.Imwrite(newPath, newImg);
            }
            catch (IOException e)
            {
                WriteLine("In NewImageWriter");
                WriteLine($"Failed to write image: {e}");
            }
        }
    }

    /// <summary>
    /// This class is responsible for resizing images.
    /// </summary>
    internal class ImageResize
    {
        /// <summary>
        /// The directory to output the resized images.
        /// </summary>
        public string OutputDirectory { get; } = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(resized)";

        private readonly NewImageWrite _newImageWrite = new NewImageWrite();

        /// <summary>
        /// Resizes the images in the specified path to the specified dimensions.
        /// </summary>
        /// <param name="path">The path of the images to resize.</param>
        /// <remarks>
        /// This method reads the "dataset_base.txt" file in the specified path line by line.
        /// Each line is assumed to be the path of an image. The image at that path is resized to 224x224 and 299x299 pixels.
        /// The resized images are saved in the same directory as the original image.
        /// </remarks>
        public void ResizeFactory(string path)
        {
            string textPath = Path.Combine(path, "dataset_base.txt");
            using (StreamReader reader = new StreamReader(textPath))
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
        /// Resizes the image at the specified path to the specified dimensions.
        /// </summary>
        /// <param name="path">The path of the image to resize.</param>
        /// <param name="width">The width to resize the image to.</param>
        /// <param name="height">The height to resize the image to.</param>
        /// <remarks>
        /// This method reads the image at the specified path into a Mat object.
        /// It then creates a new Mat object and resizes the image to the specified width and height using the CvInvoke.Resize method.
        /// The resized image is saved in the same directory as the original image.
        /// </remarks>
        private void Resizer(string path, int width, int height)
        {
            Mat image = CvInvoke.Imread(path, ImreadModes.Color);
            Mat resizedImage = new Mat();
            CvInvoke.Resize(image, resizedImage, new Size(width, height), 0, 0, Inter.Area);

            ImageWriterAssistance(path, resizedImage, width, height);
        }

        /// <summary>
        /// Assists in writing the resized image to a specific output directory.
        /// </summary>
        /// <param name="path">The path of the image to resize.</param>
        /// <param name="newImg">The resized image.</param>
        /// <param name="width">The width of the resized image.</param>
        /// <param name="height">The height of the resized image.</param>
        /// <remarks>
        /// This method first gets the parent directory of the specified path and its parent directory.
        /// It then constructs a specific output directory path by combining the OutputDirectory, the size of the resized image, and the names of the parent directories.
        /// Finally, it calls the DirrectoryCreate method of the NewImageWrite class to write the resized image to the specific output directory.
        /// </remarks>
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
        /// Performs data augmentation on the images at the specified paths.
        /// </summary>
        /// <param name="path">The path of the images to augment.</param>
        /// <remarks>
        /// This method first constructs two paths by appending "dataset_size224_224.txt" and "dataset_size299_299.txt" to the specified path.
        /// It then starts two tasks that run the AugmentDirectory method on the constructed paths.
        /// The method waits for both tasks to complete before returning.
        /// </remarks>
        public void AugmentFactory(string path)
        {
            string path224 = Path.Combine(path, "dataset_size224_224.txt");
            string path299 = Path.Combine(path, "dataset_size299_299.txt");

            Task path224Task = Task.Run(() => AugmentDirectory(path224));
            Task path299Task = Task.Run(() => AugmentDirectory(path299));

            Task.WaitAll(path224Task, path299Task);
        }

        /// <summary>
        /// Performs data augmentation on the images in the specified directory.
        /// </summary>
        /// <param name="path">The path of the directory containing the images to augment.</param>
        /// <remarks>
        /// This method first counts the total number of lines in the file at the specified path.
        /// It then reads the file line by line. Each line is assumed to be the path of an image.
        /// The image at that path is augmented by flipping, rotating, adding noise, translating, and copying.
        /// The augmented images are saved in the same directory as the original image.
        /// </remarks>
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
        /// Performs a flip augmentation on the image at the specified path.
        /// </summary>
        /// <param name="path">The path of the image to augment.</param>
        /// <remarks>
        /// This method first reads the image at the specified path into a Mat object.
        /// It then creates a new Mat object and flips the image using the CvInvoke.Flip method.
        /// The flipped image is saved in the same directory as the original image.
        /// </remarks>
        private void FlipAugmenter(string path)
        {
            string action = "flipped";
            Mat image = CvInvoke.Imread(path, ImreadModes.Color);
            Mat flippedImage = new Mat();
            CvInvoke.Flip(image, flippedImage, FlipType.Both);

            ImageWriterAssistance(path, flippedImage, action);
        }

        /// <summary>
        /// Performs a rotate augmentation on the image at the specified path.
        /// </summary>
        /// <param name="path">The path of the image to augment.</param>
        /// <remarks>
        /// This method first reads the image at the specified path into a Mat object.
        /// It then creates a new Mat object and rotates the image 90 degrees clockwise using the CvInvoke.Rotate method.
        /// The rotated image is saved in the same directory as the original image.
        /// </remarks>
        private void RotateAugmenter(string path)
        {
            string action = "rotated";
            Mat image = CvInvoke.Imread(path, ImreadModes.Color);
            Mat rotatedImage = new Mat();
            CvInvoke.Rotate(image, rotatedImage, RotateFlags.Rotate90Clockwise);

            ImageWriterAssistance(path, rotatedImage, action);
        }

        /// <summary>
        /// Performs a noise augmentation on the image at the specified path.
        /// </summary>
        /// <param name="path">The path of the image to augment.</param>
        /// <remarks>
        /// This method first reads the image at the specified path into a Mat object.
        /// It then creates a new Mat object and applies a Gaussian blur to the image using the CvInvoke.GaussianBlur method.
        /// The noised image is saved in the same directory as the original image.
        /// </remarks>
        private void NoiseAugmenter(string path)
        {
            string action = "noised";
            Mat image = CvInvoke.Imread(path, ImreadModes.Color);
            Mat noisedImage = new Mat();
            CvInvoke.GaussianBlur(image, noisedImage, new Size(5, 5), 0);

            ImageWriterAssistance(path, noisedImage, action);
        }

        /// <summary>
        /// Performs a translate augmentation on the image at the specified path.
        /// </summary>
        /// <param name="path">The path of the image to augment.</param>
        /// <remarks>
        /// This method first reads the image at the specified path into a Mat object.
        /// It then creates a new Mat object and a shift matrix for the translation.
        /// A random shift in the x and y directions between -50 and 50 is generated.
        /// The shift matrix is populated with the identity elements and the shift values.
        /// The image is then translated using the CvInvoke.WarpAffine method with the shift matrix.
        /// The translated image is saved in the same directory as the original image.
        /// </remarks>
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
        /// Copies the resized image at the specified path.
        /// </summary>
        /// <param name="path">The path of the image to copy.</param>
        /// <remarks>
        /// This method first reads the image at the specified path into a Mat object.
        /// It then makes a copy of the Mat object.
        /// The copied image is saved in the same directory as the original image.
        /// </remarks>
        private void CopyResizedImage(string path)
        {
            string action = "resized";
            Mat image = CvInvoke.Imread(path, ImreadModes.Color);
            Mat copyResizedImage = image;

            ImageWriterAssistance(path, copyResizedImage, action);
        }

        /// <summary>
        /// The directory to output the augmented images.
        /// </summary>
        public string OutputDirectory { get; } = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(augmented)";

        /// <summary>
        /// Assists in writing the augmented image to the specified path.
        /// </summary>
        /// <param name="path">The path of the original image.</param>
        /// <param name="newImg">The augmented image to write.</param>
        /// <param name="action">The type of augmentation performed.</param>
        /// <remarks>
        /// This method first gets the parent directory of the original image and constructs a specific output directory path.
        /// The output directory path is constructed by combining the OutputDirectory, action, and the names of the parent directories.
        /// The image name is extracted from the original path.
        /// The NewImageWrite.DirrectoryCreate method is then called with the specific output directory, the augmented image, and the image name.
        /// </remarks>
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

        /// <summary>
        /// Executes a Python script to remove the background of images.
        /// </summary>
        /// <remarks>
        /// This method sets the path to the Python DLL and the PYTHONHOME environment variable.
        /// It then initializes the Python Engine and reads the Python script from a file.
        /// The Python script is executed using the PythonEngine.RunSimpleString method.
        /// If an exception occurs during the execution of the Python script, it is caught and logged to the console.
        /// The Python Engine is shut down at the end of the method, regardless of whether an exception occurred.
        /// </remarks>
        public void RemoveBackgroundFactory()
        {
            try
            {
                // Path to the Python DLL
                Runtime.PythonDLL = @"C:\Users\kumar\AppData\Local\Programs\Python\Python312\python312.dll";
                // Setting the PYTHONHOME environment variable
                Environment.SetEnvironmentVariable("PYTHONHOME", @"C:\Users\kumar\AppData\Local\Programs\Python\Python312", EnvironmentVariableTarget.Process);

                // Initialize the Python Engine
                PythonEngine.Initialize();
                //PythonScriptRemoveBackground();

                using (Py.GIL())
                {
                    // Read the Python script
                    string pythonScript = System.IO.File.ReadAllText(@"D:\Project\AI_ML_DS\Minor_Project_Ai_Plant_Recognition\Minor_Project_Ai_Plant_Recognition\SorceCode\remove_background.py");
                    // Run the Python script
                    PythonEngine.RunSimpleString(pythonScript);
                }
            }
            catch (Exception e)
            {
                WriteLine($"Failed to remove background: {e.Message}");
            }
            finally
            {
                // Shutdown the Python Engine
                PythonEngine.Shutdown();
            }
        }
    }

    /// <summary>
    /// This class is responsble for normalization of images data.
    /// </summary>
    internal class Normalization
    {
        private readonly NewImageWrite _newImageWrite = new NewImageWrite();

        /// <summary>
        /// Normalizes the images at the specified path.
        /// </summary>
        /// <param name="path">The path of the images to normalize.</param>
        /// <remarks>
        /// This method first constructs the paths to the text files containing the paths of the images to normalize.
        /// It then starts two tasks to normalize the images at the paths specified in the text files.
        /// The method waits for both tasks to complete before returning.
        /// </remarks>
        public void NormalizationFactor(string path)
        {
            string path224 = Path.Combine(path, "dataset_size224_224.txt");
            string path299 = Path.Combine(path, "dataset_size299_299.txt");

            Task path224Task = Task.Run(() => NormalizeDirectory(path224));
            Task path299Task = Task.Run(() => NormalizeDirectory(path299));

            Task.WaitAll(path224Task, path299Task);
        }

        /// <summary>
        /// Normalizes the images in the directory specified by the path.
        /// </summary>
        /// <param name="path">The path of the directory containing the images to normalize.</param>
        /// <remarks>
        /// This method first counts the total number of lines in the file at the specified path.
        /// It then opens a StreamReader on the file and reads each line.
        /// Each line is passed to the NormalizeColorWise method to normalize the image at the path specified by the line.
        /// </remarks>
        private void NormalizeDirectory(string path)
        {
            int totalLines = File.ReadLines(path).Count();

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()!) != null)
                {
                    NormalizeColorWise(line);
                }
            }
        }

        /// <summary>
        /// Normalizes the colors of the image at the specified path.
        /// </summary>
        /// <param name="path">The path of the image to normalize.</param>
        /// <remarks>
        /// This method first reads the image at the specified path into a Mat object.
        /// It then clones the Mat object and splits it into its color channels.
        /// Each color channel is normalized to have a minimum value of 0 and a maximum value of 255.
        /// The normalized color channels are then merged back into a single image.
        /// The normalized image is saved in the same directory as the original image.
        /// </remarks>
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
        /// The directory to output the normalized images.
        /// </summary>
        public string OutputDirectory { get; } = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(normalized)";

        /// <summary>
        /// Assists in writing the normalized image to the specified path.
        /// </summary>
        /// <param name="path">The path of the original image.</param>
        /// <param name="newImg">The normalized image to write.</param>
        /// <remarks>
        /// This method first gets the parent directories of the original image and constructs a specific output directory path.
        /// The output directory path is constructed by combining the OutputDirectory and the names of the parent directories.
        /// The image name is extracted from the original path.
        /// The NewImageWrite.DirrectoryCreate method is then called with the specific output directory, the normalized image, and the image name.
        /// </remarks>
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