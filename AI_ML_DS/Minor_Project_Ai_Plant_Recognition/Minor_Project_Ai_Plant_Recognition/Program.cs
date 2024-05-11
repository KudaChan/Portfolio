using Minor_Project_Ai_Plant_Recognition.SourceCode.DataBaseAction;
using Minor_Project_Ai_Plant_Recognition.SourceCode.PreProcessing;
using Minor_Project_Ai_Plant_Recognition.SourceCode.ModelTraining;
using Tensorflow;

namespace Minor_Project_Ai_Plant_Recognition
{
    /// <summary>
    /// The Program class is the entry point of the application.
    /// </summary>
    internal class Program
    {
        public static void Main(string[] args)
        {
            WriteLine("Database Connection Started");
            _ = new DBMain();
            //dBMain.DataParserFromOrignalDirAndFeeder();

            //DataAugmentation preprocessing = new DataAugmentation();
            //preprocessing.DataAugmentationMain();

            //BckRemove bckRemove = new BckRemove();
            //bckRemove.RemoveBackground();

            //Augmentation_BckRem augmentation_BckRem = new Augmentation_BckRem();
            //augmentation_BckRem.DataAugmentationMain();

            //dBMain.DataParserFromPreprocessedDirAndFeeder();

            //dBMain.DataParserFromOrignalDirAndFeeder();

            //DataAugmentation preprocessing = new DataAugmentation();
            //preprocessing.DataAugmentationMain();

            //BckRemove bckRemove = new BckRemove();
            //bckRemove.RemoveBackground();

            //Augmentation_BckRem augmentation_BckRem = new Augmentation_BckRem();
            //augmentation_BckRem.DataAugmentationMain();

            //dBMain.DataParserFromPreprocessedDirAndFeeder();

            _ = new PythonMain();
            PythonMain.PyMain();

            WriteLine("thanks for you corporation...");
        }
    }
}