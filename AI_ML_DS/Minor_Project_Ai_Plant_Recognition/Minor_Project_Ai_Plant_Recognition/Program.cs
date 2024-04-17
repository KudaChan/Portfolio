using Minor_Project_Ai_Plant_Recognition.SorceCode.Preprocessing;

namespace Minor_Project_Ai_Plant_Recognition
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Directory of orignal dataset
            string path = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(orignal)";

            ImageAccess imageAccess = new ImageAccess();
            imageAccess.ParseImage(path);
        }
    }
}