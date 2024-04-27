using Emgu.CV;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.DataParse_Sampling.ImageWriter
{
    /// <summary>
    /// This class is responsible for writing images to a directory.
    /// </summary>
    internal class NewImageWrite
    {
        /// <summary>
        /// Creates a directory at the specified path and writes an image to it.
        /// </summary>
        /// <param name="path">The path where the file will be created.</param>
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
                //Directory.Delete(path, true);

                //try
                //{
                //    Directory.CreateDirectory(path);
                //}
                //catch (Exception e)
                //{
                //    WriteLine($"Failed to create directory: {e}");
                //}

                WriteImage(path, newImg, imgName);
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(path);
                    WriteImage(path, newImg, imgName);
                }
                catch (Exception e)
                {
                    WriteLine($"Failed to create directory: {e}");
                }
            }
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
}