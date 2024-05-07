using Minor_Project_Ai_Plant_Recognition.SorceCode.PreProcessing;
using Minor_Project_Ai_Plant_Recognition.SorceCode.DataParse_Sampling.ImageParsing;
using Minor_Project_Ai_Plant_Recognition.SorceCode.DataParse_Sampling;
using Minor_Project_Ai_Plant_Recognition.SorceCode.ModelTraining;
using TensorFlowNET.Examples;
using Emgu.CV.CvEnum;

namespace Minor_Project_Ai_Plant_Recognition
{
    /// <summary>
    /// The RequiredPaths class contains the paths required for the preprocessing of images.
    /// These paths include the directory of the text file containing the image paths (_textFileDir),
    /// the base directory of the images (_baseDir), the directory for the resized images (_resizedDir),
    /// the directory for the augmented images (_augmentedDir), the directory for the images with the background removed (_bckgrndRemovedDir),
    /// and the directory for the normalized images (_normalizedDir).
    /// </summary>
    internal class RequiredPaths
    {
        public string _textFileDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataTxtFile";
        public string _orignalImgDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataOrignal";
        public string _sampleImgDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataSampled\\Dataset(5species)";
        public string _resizedDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataPreProcessed\\Dataset(resized)";
        public string _augmentedDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataPreProcessed\\Dataset(augmented)";
        public string _bckgrndRemovedDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataPreProcessed\\Dataset(background_removed)";
        public string _normalizedDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataPreProcessed\\Dataset(normalized)";
    }

    /// <summary>
    /// The PreProcesser class is responsible for the preprocessing of images.
    /// </summary>
    internal class PreProcesser
    {
        /// <summary>
        /// The ImageAccess object is used to access the images for preprocessing.
        /// </summary>
        private readonly ImageAccess _imageAccess = new ImageAccess();

        /// <summary>
        /// The ImageResize object is used to resize the images.
        /// </summary>
        private readonly ImageResize _imageResize = new ImageResize();

        /// <summary>
        /// The DataAugmentation object is used to augment the images.
        /// </summary>
        private readonly DataAugmentation _augmentation = new DataAugmentation();

        /// <summary>
        /// The BckgrndRemover object is used to remove the background from the images.
        /// </summary>
        private readonly BckgrndRemover _bckgrndRemover = new BckgrndRemover();

        /// <summary>
        /// The Normalization object is used to normalize the images.
        /// </summary>
        private readonly Normalization _normalization = new Normalization();

        /// <summary>
        /// The action array contains the names of the preprocessing actions to be performed on the images.
        /// </summary>
        private string[] action = ["Resize", "Augment", "BckgrndRemove", "Normalization"];

        /// <summary>
        /// The _textFile is the path to the text file containing the image paths.
        /// </summary>
        private string _textFile = new RequiredPaths()._textFileDir;

        /// <summary>
        /// The _basePath is the base directory of the images.
        /// </summary>
        private string _basePath = new RequiredPaths()._sampleImgDir;

        /// <summary>
        /// The _pathResized is the directory for the resized images.
        /// </summary>
        private string _pathResized = new RequiredPaths()._resizedDir;

        /// <summary>
        /// The _pathAugmented is the directory for the augmented images.
        /// </summary>
        private string _pathAugmented = new RequiredPaths()._augmentedDir;

        /// <summary>
        /// The _pathBckgrndRemoved is the directory for the images with the background removed.
        /// </summary>
        private string _pathBckgrndRemoved = new RequiredPaths()._bckgrndRemovedDir;

        /// <summary>
        /// The _pathNormalized is the directory for the normalized images.
        /// </summary>
        private string _pathNormalized = new RequiredPaths()._normalizedDir;

        /// <summary>
        /// The PreProcess method is responsible for the preprocessing of images.
        /// It performs the following actions in order: Resizing, Augmentation, Background Removal, and Normalization.
        /// For each action, it first writes a start message to the console.
        /// It then calls the DirectoryParser method of the ImageAccess object to parse the directory of images.
        /// Depending on the action, it then calls the corresponding factory method to perform the action on the images.
        /// After the action is done, it writes a done message to the console.
        /// </summary>
        public void PreProcess()
        {
            WriteLine("Resizing Start");
            if (Directory.Exists(_pathResized))
            {
                Directory.Delete(_pathResized, true);
            }
            _imageAccess.DirectoryParser(_basePath, _textFile, action[0]);
            _imageResize.ResizeFactory(_textFile);
            WriteLine("Resizing Done");

            WriteLine("Augmentation Start");
            if (Directory.Exists(_pathAugmented))
            {
                Directory.Delete(_pathAugmented, true);
            }
            _imageAccess.DirectoryParser(_pathResized, _textFile, action[1]);
            _augmentation.AugmentFactory(_textFile);
            WriteLine("Augmentation Done");

            WriteLine("Background Removal Start");
            if (Directory.Exists(_pathBckgrndRemoved))
            {
                Directory.Delete(_pathBckgrndRemoved, true);
            }
            _imageAccess.DirectoryParser(_pathAugmented, _textFile, action[2]);
            _bckgrndRemover.RemoveBackgroundFactory();
            WriteLine("Background Removal Done");

            WriteLine("Normalization Start");
            if (Directory.Exists(_pathNormalized))
            {
                Directory.Delete(_pathNormalized, true);
            }
            _imageAccess.DirectoryParser(_pathBckgrndRemoved, _textFile, action[3]);
            _normalization.NormalizationFactor(_textFile);
            WriteLine("Normalization Done");
        }
    }

    internal class DataSampler
    {
        private readonly DataSampling _imageSampling = new DataSampling();

        private string _textFile = new RequiredPaths()._textFileDir;
        private string _basePath = new RequiredPaths()._orignalImgDir;

        public void SampleData()
        {
            _imageSampling.SampleDataInitiator(_basePath, _textFile);
        }
    }

    internal class DataSpliting
    {
        private readonly DataSplit _dataSplit = new DataSplit();

        public void SplitData()
        {
            _dataSplit.DataSplitFactory();
        }
    }

    internal class ModelTraining
    {
        private readonly ResNetImp _resNet = new ResNetImp();

        public void TrainModel()
        {
            _resNet.ResNetImplementation();
        }
    }

    /// <summary>
    /// The Program class is the entry point of the application.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The Main method is the entry point of the application.
        /// It starts the preprocessing of images by creating a PreProcesser object and calling its PreProcess method.
        /// It writes a start and done message to the console before and after the preprocessing respectively.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            //WriteLine("Data Sampling Started");
            //DataSampler dataSampler = new DataSampler();
            //dataSampler.SampleData();
            //WriteLine("Data Sampling Done");

            //WriteLine("Preprocessing Started");
            //PreProcesser preProcesser = new PreProcesser();
            //preProcesser.PreProcess();
            //WriteLine("Preprocessing Done");

            //WriteLine("Data Spliting Started");
            //DataSpliting dataSpliting = new DataSpliting();
            //dataSpliting.SplitData();
            //WriteLine("Data Spliting Done");

            //WriteLine("Model Training Started");
            //ModelTraining modelTraining = new ModelTraining();
            //modelTraining.TrainModel();
            //WriteLine("Model Training Done");

            ImageRecognitionInception tempimp = new ImageRecognitionInception();
            tempimp.Run();
        }
    }
}