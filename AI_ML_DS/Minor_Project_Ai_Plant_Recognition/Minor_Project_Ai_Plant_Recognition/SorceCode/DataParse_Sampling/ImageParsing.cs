namespace Minor_Project_Ai_Plant_Recognition.SorceCode.DataParse_Sampling.ImageParsing
{
    /// <summary>
    /// This class is responsible for Accessing Image directory and write the paths into txt file.
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
}