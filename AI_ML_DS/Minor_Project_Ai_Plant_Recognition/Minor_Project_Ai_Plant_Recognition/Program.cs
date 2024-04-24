using Minor_Project_Ai_Plant_Recognition.SorceCode.Preprocessing;
using System.Drawing;

namespace Minor_Project_Ai_Plant_Recognition
{
    internal class RequiredPaths
    {
        public string _textFileDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Temp\\dataset"!;
        public string _baseDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(test)";
        public string _resizedDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(resized)";
        public string _augmentedDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(augmented)";
        public string _bckgrndRemovedDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(background_removed)";
        public string _normalizedDir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(normalized)";
    }

    internal class PreProcesser
    {
        private readonly ImageAccess _imageAccess = new ImageAccess();
        private readonly ImageResize _imageResize = new ImageResize();
        private readonly DataAugmentation _augmentation = new DataAugmentation();
        private readonly BckgrndRemover _bckgrndRemover = new BckgrndRemover();
        private readonly Normalization _normalization = new Normalization();

        private string[] action = ["Resize", "Augment", "BckgrndRemove", "Normalization"];

        private string _textFile = new RequiredPaths()._textFileDir;
        private string _basePath = new RequiredPaths()._baseDir;
        private string _pathResized = new RequiredPaths()._resizedDir;
        private string _pathAugmented = new RequiredPaths()._augmentedDir;
        private string _pathBckgrndRemoved = new RequiredPaths()._bckgrndRemovedDir;

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
            _bckgrndRemover.RemoveBackgroundFactory(_textFile);
            WriteLine("Background Removal Done");

            WriteLine("Normalization Start");
            _imageAccess.DirectoryParser(_pathBckgrndRemoved, _textFile, action[3]);
            _normalization.NormalizationFactor(_textFile);
            WriteLine("Normalization Done");

            WriteLine("Preprocessing Done");
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            WriteLine("Preprocessing Started");
            PreProcesser preProcesser = new PreProcesser();
            preProcesser.PreProcess();
        }
    }
}