using Minor_Project_Ai_Plant_Recognition.SourceCode.DataBaseAction;
using Minor_Project_Ai_Plant_Recognition.SourceCode.PreProcessing;
using Minor_Project_Ai_Plant_Recognition.SourceCode.ModelTraining;
using Minor_Project_Ai_Plant_Recognition.SourceCode.PythonModelScripts;

namespace Minor_Project_Ai_Plant_Recognition
{
    /// <summary>
    /// The Program class is the entry point of the application.
    /// </summary>
    internal class Final
    {
        public static void FinalMain()
        {
            WriteLine("Database Connection Started");
            DBMain dbMain = new DBMain();
            dbMain.DataParserFromOrignalDirAndFeeder();

            DataAugmentation preprocessing = new();
            preprocessing.DataAugmentationMain();

            BckRemove.RemoveBackground();

            Augmentation_BckRem augmentation_BckRem = new();
            augmentation_BckRem.DataAugmentationMain();

            dbMain.DataParserFromPreprocessedDirAndFeeder();

            dbMain.DataParserFromOrignalDirAndFeeder();

            DataAugmentation preprocessing2 = new();
            preprocessing2.DataAugmentationMain();

            BckRemove.RemoveBackground();

            Augmentation_BckRem augmentation_BckRem2 = new();
            augmentation_BckRem2.DataAugmentationMain();

            dbMain.DataParserFromPreprocessedDirAndFeeder();

            PythonMain.PyMain();

            Prediction.PredictMain();

            WriteLine("thanks for you corporation...");
        }
    }
}