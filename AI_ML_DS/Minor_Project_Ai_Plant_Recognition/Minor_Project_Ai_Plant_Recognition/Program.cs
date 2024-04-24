using Minor_Project_Ai_Plant_Recognition.SorceCode.Preprocessing;

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
        public string _textFileDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Temp\\dataset"!;
        public string _baseDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(test)";
        public string _resizedDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(resized)";
        public string _augmentedDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(augmented)";
        public string _bckgrndRemovedDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(background_removed)";
        public string _normalizedDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(normalized)";
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
        private string _basePath = new RequiredPaths()._baseDir;

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
            _imageAccess.DirectoryParser(_basePath, _textFile, action[0]);
            _imageResize.ResizeFactory(_textFile);
            WriteLine("Resizing Done");

            WriteLine("Augmentation Start");
            _imageAccess.DirectoryParser(_pathResized, _textFile, action[1]);
            _augmentation.AugmentFactory(_textFile);
            WriteLine("Augmentation Done");

            WriteLine("Background Removal Start");
            _imageAccess.DirectoryParser(_pathAugmented, _textFile, action[2]);
            _bckgrndRemover.RemoveBackgroundFactory();
            WriteLine("Background Removal Done");

            WriteLine("Normalization Start");
            _imageAccess.DirectoryParser(_pathBckgrndRemoved, _textFile, action[3]);
            _normalization.NormalizationFactor(_textFile);
            WriteLine("Normalization Done");
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
            WriteLine("Preprocessing Started");
            PreProcesser preProcesser = new PreProcesser();
            preProcesser.PreProcess();
            WriteLine("Preprocessing Done");
        }
    }
}