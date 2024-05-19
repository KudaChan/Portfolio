using Crop_Recommendation_Model.src;

namespace Crop_Recommendation_Project
{
    internal class Program
    {
        [Obsolete]
        private static void Main(string[] args)
        {
            Console.WriteLine("Crop Recommendation Project.");

            //ModelTrainer.Train();

            DataPrepare.DataRetriver();
            DataPrepare.GetUserInput();
            DataPrepare.PrepareData();
            Predictor.Predict();
        }
    }
}