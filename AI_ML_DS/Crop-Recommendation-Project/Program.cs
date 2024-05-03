using Crop_Recommendation_Project.src.DBManupulator;
using Crop_Recommendation_Project.src.Preprocessor;

namespace Crop_Recommendation_Project
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Crop Recommendation Project.");

            DataPreprocessor.Preprocessor();
        }
    }
}