using Crop_Recommendation_Project.src;
using Crop_Recommendation_Project.src.DBManupulator;
using Crop_Recommendation_Project.src.Preprocessor;

namespace Crop_Recommendation_Project
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Crop Recommendation Project.");

            //DataPreprocessor.Preprocessor();

            var modelBuilder = new ModelBuilder();
            var trainData = modelBuilder.LoadData("Select * from traindata");
            var model1 = modelBuilder.BuildAndTrainModel(trainData);

            modelBuilder.CrossValidateModel(trainData);

            var testData = modelBuilder.LoadData("Select * from testdata");
            modelBuilder.EvaluateModel(model1, testData);
        }
    }
}