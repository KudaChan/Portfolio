using Emgu.CV;
using Emgu.CV.CvEnum;
using Python.Runtime;
using Minor_Project_Ai_Plant_Recognition.SorceCode.DataBaseAction;
using Minor_Project_Ai_Plant_Recognition.SorceCode.DataStructure;
using System.Drawing;
using Emgu.CV.Cuda;
using Tensorflow;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.PreProcessing
{
    internal class DataAugmentation
    {
        private Dictionary<int, string> speciesDict = new Dictionary<int, string>();
        private List<OrignalImgPath.ImgPathOrignal> imgData = new List<OrignalImgPath.ImgPathOrignal>();
        private List<PreprocessedPath.ImgPathPreprocessed> imgDataPreprocesseds = new List<PreprocessedPath.ImgPathPreprocessed>();

        public string _baseDir = "D:\\Dataset\\medai\\PreProcessed";

        private void DataLoaderFromDB()
        {
            DBMain dbMain = new DBMain();
            dbMain.SpeciesDictInit(speciesDict);

            dbMain.DataParseFromOrignalPathTable(imgData);

            WriteLine(imgData.Count);
        }

        private void RotationAugment(string process)
        {
            string action = "rotated";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat rotatedImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);

                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                for (int i = 0; i < 360; i += 90)
                {
                    var center = new PointF(image.Width / 2, image.Height / 2);
                    var rotationMatrix = new Mat();
                    CvInvoke.GetRotationMatrix2D(center, i, 1.0, rotationMatrix);
                    CvInvoke.WarpAffine(image, rotatedImage, rotationMatrix, image.Size);

                    string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                    if (!Directory.Exists(newImgdir))
                    {
                        Directory.CreateDirectory(newImgdir);
                    }

                    string newImgPath = Path.Combine(newImgdir, $"{imgName}_{i}.jpg");

                    var result = CvInvoke.Imwrite(newImgPath, rotatedImage);
                }
            });
            WriteLine("     Rotation Augmentation Done");
        }

        private void TranslationAugment(string process)
        {
            string action = "translated";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat translatedImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);
                //Matrix<float> shiftMatrix = new Matrix<float>(2, 3);
                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                for (int i = -50; i <= 50; i += 10)
                {
                    for (int j = -50; j <= 50; j += 10)
                    {
                        Matrix<float> shiftMatrix = new Matrix<float>(2, 3);
                        shiftMatrix[0, 0] = 1;  // Identity element
                        shiftMatrix[1, 1] = 1;  // Identity element
                        shiftMatrix[0, 2] = i;  // Shift in x direction
                        shiftMatrix[1, 2] = j;  // shift in y direction
                        CvInvoke.WarpAffine(image, translatedImage, shiftMatrix, image.Size);

                        string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                        if (!Directory.Exists(newImgdir))
                        {
                            Directory.CreateDirectory(newImgdir);
                        }

                        string newImgPath = Path.Combine(newImgdir, $"{imgName}_{i}_{j}.jpg");

                        var result = CvInvoke.Imwrite(newImgPath, translatedImage);
                    }
                }
            });
            WriteLine("     Translation Augmentation Done");
        }

        private void ScalingAugment(string process)
        {
            string action = "scaled";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat scaledImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);

                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                for (int i = 1; i <= 3; i++)
                {
                    var scaledMatrix = new Mat();
                    CvInvoke.Resize(image, scaledImage, new Size(image.Width * i, image.Height * i));

                    string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                    if (!Directory.Exists(newImgdir))
                    {
                        Directory.CreateDirectory(newImgdir);
                    }

                    string newImgPath = Path.Combine(newImgdir, $"{imgName}_{i}.jpg");

                    var result = CvInvoke.Imwrite(newImgPath, scaledImage);
                }
            });
            WriteLine("     Scaling Augmentation Done");
        }

        private void FlipAugment(string process)
        {
            string action = "flipped";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat flippedImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);

                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                CvInvoke.Flip(image, flippedImage, FlipType.Horizontal);

                string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                if (!Directory.Exists(newImgdir))
                {
                    Directory.CreateDirectory(newImgdir);
                }

                string newImgPath = Path.Combine(newImgdir, $"{imgName}.jpg");

                var result = CvInvoke.Imwrite(newImgPath, flippedImage);
            });
            WriteLine("     Flip Augmentation Done");
        }

        private void ContrastAugment(string process)
        {
            string action = "contrast";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat contrastImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);

                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                for (int i = -50; i <= 50; i += 10)
                {
                    var contrastMatrix = new Mat();
                    CvInvoke.ConvertScaleAbs(image, contrastImage, 1, i);

                    string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                    if (!Directory.Exists(newImgdir))
                    {
                        Directory.CreateDirectory(newImgdir);
                    }

                    string newImgPath = Path.Combine(newImgdir, $"{imgName}_{i}.jpg");

                    var result = CvInvoke.Imwrite(newImgPath, contrastImage);
                }
            });
            WriteLine("     Contrast Augmentation Done");
        }

        private void ZoomAugmentation(string process)
        {
            string action = "zoomed";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat zoomedImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);

                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                for (int i = 1; i <= 3; i++)
                {
                    var zoomMatrix = new Mat();
                    CvInvoke.Resize(image, zoomedImage, new Size(image.Width / i, image.Height / i));

                    string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                    if (!Directory.Exists(newImgdir))
                    {
                        Directory.CreateDirectory(newImgdir);
                    }

                    string newImgPath = Path.Combine(newImgdir, $"{imgName}_{i}.jpg");

                    var result = CvInvoke.Imwrite(newImgPath, zoomedImage);
                }
            });
            WriteLine("     Zoom Augmentation Done");
        }

        private void GrayscaleAugmentation(string process)
        {
            string action = "grayscaled";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat grayImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);

                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                if (CudaInvoke.HasCuda)  // Check for CUDA compatible GPU
                {
                    using (GpuMat gpuImage = new GpuMat(image))  // Upload image to GPU
                    {
                        using (GpuMat gpuResult = new GpuMat())  // Create GPU matrix for result
                        {
                            CudaInvoke.CvtColor(gpuImage, gpuResult, ColorConversion.Bgr2Gray);  // Perform operation on GPU
                            grayImage = gpuResult.ToMat();  // Download result from GPU
                        }
                    }
                }
                else
                {
                    CvInvoke.CvtColor(image, grayImage, ColorConversion.Bgr2Gray);
                }

                string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                if (!Directory.Exists(newImgdir))
                {
                    Directory.CreateDirectory(newImgdir);
                }

                string newImgPath = Path.Combine(newImgdir, $"{imgName}.jpg");

                var result = CvInvoke.Imwrite(newImgPath, grayImage);
            });
            WriteLine("     Grayscale Augmentation Done");
        }

        private void GaussianBlurAugmentation(string process)
        {
            string action = "gaussianBlurred";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat gaussianBlurredImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);

                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                CvInvoke.GaussianBlur(image, gaussianBlurredImage, new Size(5, 5), 0);

                string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                if (!Directory.Exists(newImgdir))
                {
                    Directory.CreateDirectory(newImgdir);
                }

                string newImgPath = Path.Combine(newImgdir, $"{imgName}.jpg");

                var result = CvInvoke.Imwrite(newImgPath, gaussianBlurredImage);
            });
            WriteLine("     Gaussian Blur Augmentation Done");
        }

        public void DataAugmentationMain()
        {
            string action = "augmented";
            DataLoaderFromDB();

            WriteLine("Data Augmentation Started");

            RotationAugment(action);
            TranslationAugment(action);
            ScalingAugment(action);
            FlipAugment(action);
            ContrastAugment(action);
            ZoomAugmentation(action);
            GrayscaleAugmentation(action);
            GaussianBlurAugmentation(action);

            WriteLine("Data Augmentation Done");
        }
    }

    internal class BckRemove()
    {
        public void RemoveBackground()
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
                    string pythonScript = File.ReadAllText(@"D:\Project\AI_ML_DS\Minor_Project_Ai_Plant_Recognition\Minor_Project_Ai_Plant_Recognition\SorceCode\PreProcessing\remove_background.py");
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

    internal class Augmentation_BckRem
    {
        private Dictionary<int, string> speciesDict = new Dictionary<int, string>();
        private List<PreprocessedPath.ImgPathPreprocessed> imgData = new List<PreprocessedPath.ImgPathPreprocessed>();

        public string _baseDir = "D:\\Dataset\\medai\\PreProcessed";

        private void DataLoaderFromDB()
        {
            DBMain dbMain = new DBMain();
            dbMain.SpeciesDictInit(speciesDict);

            dbMain.DataParseFromPreprocessedPathTable(imgData, 2);

            WriteLine(imgData.Count);
        }

        private void RotationAugment(string process)
        {
            string action = "rotated";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat rotatedImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);

                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                for (int i = 0; i < 360; i += 90)
                {
                    var center = new PointF(image.Width / 2, image.Height / 2);
                    var rotationMatrix = new Mat();
                    CvInvoke.GetRotationMatrix2D(center, i, 1.0, rotationMatrix);
                    CvInvoke.WarpAffine(image, rotatedImage, rotationMatrix, image.Size);

                    string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                    if (!Directory.Exists(newImgdir))
                    {
                        Directory.CreateDirectory(newImgdir);
                    }

                    string newImgPath = Path.Combine(newImgdir, $"{imgName}_{i}.jpg");

                    var result = CvInvoke.Imwrite(newImgPath, rotatedImage);
                }
            });
            WriteLine("     Rotation Augmentation Done");
        }

        private void TranslationAugment(string process)
        {
            string action = "translated";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat translatedImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);
                //Matrix<float> shiftMatrix = new Matrix<float>(2, 3);
                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                for (int i = -50; i <= 50; i += 10)
                {
                    for (int j = -50; j <= 50; j += 10)
                    {
                        Matrix<float> shiftMatrix = new Matrix<float>(2, 3);
                        shiftMatrix[0, 0] = 1;  // Identity element
                        shiftMatrix[1, 1] = 1;  // Identity element
                        shiftMatrix[0, 2] = i;  // Shift in x direction
                        shiftMatrix[1, 2] = j;  // shift in y direction
                        CvInvoke.WarpAffine(image, translatedImage, shiftMatrix, image.Size);

                        string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                        if (!Directory.Exists(newImgdir))
                        {
                            Directory.CreateDirectory(newImgdir);
                        }

                        string newImgPath = Path.Combine(newImgdir, $"{imgName}_{i}_{j}.jpg");

                        var result = CvInvoke.Imwrite(newImgPath, translatedImage);
                    }
                }
            });
            WriteLine("     Translation Augmentation Done");
        }

        private void ScalingAugment(string process)
        {
            string action = "scaled";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat scaledImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);

                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                for (int i = 1; i <= 3; i++)
                {
                    var scaledMatrix = new Mat();
                    CvInvoke.Resize(image, scaledImage, new Size(image.Width * i, image.Height * i));

                    string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                    if (!Directory.Exists(newImgdir))
                    {
                        Directory.CreateDirectory(newImgdir);
                    }

                    string newImgPath = Path.Combine(newImgdir, $"{imgName}_{i}.jpg");

                    var result = CvInvoke.Imwrite(newImgPath, scaledImage);
                }
            });
            WriteLine("     Scaling Augmentation Done");
        }

        private void FlipAugment(string process)
        {
            string action = "flipped";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat flippedImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);

                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                CvInvoke.Flip(image, flippedImage, FlipType.Horizontal);

                string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                if (!Directory.Exists(newImgdir))
                {
                    Directory.CreateDirectory(newImgdir);
                }

                string newImgPath = Path.Combine(newImgdir, $"{imgName}.jpg");

                var result = CvInvoke.Imwrite(newImgPath, flippedImage);
            });
            WriteLine("     Flip Augmentation Done");
        }

        private void ContrastAugment(string process)
        {
            string action = "contrast";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat contrastImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);

                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                for (int i = -50; i <= 50; i += 10)
                {
                    var contrastMatrix = new Mat();
                    CvInvoke.ConvertScaleAbs(image, contrastImage, 1, i);

                    string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                    if (!Directory.Exists(newImgdir))
                    {
                        Directory.CreateDirectory(newImgdir);
                    }

                    string newImgPath = Path.Combine(newImgdir, $"{imgName}_{i}.jpg");

                    var result = CvInvoke.Imwrite(newImgPath, contrastImage);
                }
            });
            WriteLine("     Contrast Augmentation Done");
        }

        private void ZoomAugmentation(string process)
        {
            string action = "zoomed";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat zoomedImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);

                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                for (int i = 1; i <= 3; i++)
                {
                    var zoomMatrix = new Mat();
                    CvInvoke.Resize(image, zoomedImage, new Size(image.Width / i, image.Height / i));

                    string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                    if (!Directory.Exists(newImgdir))
                    {
                        Directory.CreateDirectory(newImgdir);
                    }

                    string newImgPath = Path.Combine(newImgdir, $"{imgName}_{i}.jpg");

                    var result = CvInvoke.Imwrite(newImgPath, zoomedImage);
                }
            });
            WriteLine("     Zoom Augmentation Done");
        }

        private void GrayscaleAugmentation(string process)
        {
            string action = "grayscaled";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat grayImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);

                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                if (CudaInvoke.HasCuda)  // Check for CUDA compatible GPU
                {
                    using (GpuMat gpuImage = new GpuMat(image))  // Upload image to GPU
                    {
                        using (GpuMat gpuResult = new GpuMat())  // Create GPU matrix for result
                        {
                            CudaInvoke.CvtColor(gpuImage, gpuResult, ColorConversion.Bgr2Gray);  // Perform operation on GPU
                            grayImage = gpuResult.ToMat();  // Download result from GPU
                        }
                    }
                }
                else
                {
                    CvInvoke.CvtColor(image, grayImage, ColorConversion.Bgr2Gray);
                }

                string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                if (!Directory.Exists(newImgdir))
                {
                    Directory.CreateDirectory(newImgdir);
                }

                string newImgPath = Path.Combine(newImgdir, $"{imgName}.jpg");

                var result = CvInvoke.Imwrite(newImgPath, grayImage);
            });
            WriteLine("     Grayscale Augmentation Done");
        }

        private void GaussianBlurAugmentation(string process)
        {
            string action = "gaussianBlurred";
            Parallel.ForEach(imgData, img =>
            {
                string species = speciesDict[img.speciesId];
                Mat image = CvInvoke.Imread(img.imgPath, ImreadModes.Color);
                Mat gaussianBlurredImage = new Mat();
                var imgName = Path.GetFileNameWithoutExtension(img.imgPath);

                string curBaseDir;

                if (img.catagoryId == 1)
                {
                    curBaseDir = Path.Combine(_baseDir, "leaf");
                }
                else
                {
                    curBaseDir = Path.Combine(_baseDir, "plant");
                }

                CvInvoke.GaussianBlur(image, gaussianBlurredImage, new Size(5, 5), 0);

                string newImgdir = Path.Combine(curBaseDir, species, $"{process}", action);
                if (!Directory.Exists(newImgdir))
                {
                    Directory.CreateDirectory(newImgdir);
                }

                string newImgPath = Path.Combine(newImgdir, $"{imgName}.jpg");

                var result = CvInvoke.Imwrite(newImgPath, gaussianBlurredImage);
            });
            WriteLine("     Gaussian Blur Augmentation Done");
        }

        public void DataAugmentationMain()
        {
            string action = "aug_BckRem";

            DataLoaderFromDB();

            WriteLine("Data Augmentation Started on bck_rem data");

            RotationAugment(action);
            TranslationAugment(action);
            ScalingAugment(action);
            FlipAugment(action);
            ContrastAugment(action);
            ZoomAugmentation(action);
            GrayscaleAugmentation(action);
            GaussianBlurAugmentation(action);

            WriteLine("Data Augmentation Done on bck_rem data");
        }
    }
}