using Crop_Recommendation_Project.src;
using Crop_Recommendation_Project.src.Preprocessor;
using Microsoft.ML;

namespace Crop_Recommendation_Model.src
{
    internal class ModelTrainer
    {
        [Obsolete]
        public static void Train()
        {
            Console.WriteLine("Model Training Started.");

            DataPreprocessor.Preprocessor();

            var modelBuilder = new ModelBuilder();
            var trainData = modelBuilder.LoadData("Select * from traindata order by label_idx;");
            var testData = modelBuilder.LoadData("Select * from testdata order by label_idx;");

            #region Model 1 - SdcaNonCalibrated Model

            var model1 = modelBuilder.SdcaNonCalibratedModelTrainer(trainData);
            modelBuilder.SdcaNonCalibratedModelValidator(trainData);
            modelBuilder.EvaluateModelAndCreateGraph(model1, testData, "model1");

            //MLContext mlContext = new MLContext();
            //mlContext.Model.Save(model1, trainData.Schema, "model_sdcaNC.zip");

            #endregion Model 1 - SdcaNonCalibrated Model

            #region Model2 - OneVersusAllLBFGS Model

            var model2 = modelBuilder.OneVersusAllModelTrainer(trainData);
            modelBuilder.OneVersusAllModelValidator(trainData);
            modelBuilder.EvaluateModelAndCreateGraph(model2, testData, "model2");

            //MLContext mlContext2 = new MLContext();
            //mlContext2.Model.Save(model2, trainData.Schema, "model_ovr.zip");

            #endregion Model2 - OneVersusAllLBFGS Model

            #region Model3 - LbfgsMaximumEntropy

            var model3 = modelBuilder.LbfgsMaximumEntropyModelTrainer(trainData);
            modelBuilder.LbfgsMaximumEntropyModelValidator(trainData);
            modelBuilder.EvaluateModelAndCreateGraph(model3, testData, "model3");

            //MLContext mlContext3 = new MLContext();
            //mlContext3.Model.Save(model3, trainData.Schema, "model_LbfgsME.zip");

            #endregion Model3 - LbfgsMaximumEntropy

            Console.WriteLine("Model Training Completed.");
        }
    }
}