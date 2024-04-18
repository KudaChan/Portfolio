using Minor_Project_Ai_Plant_Recognition.SorceCode.Preprocessing;

namespace Minor_Project_Ai_Plant_Recognition
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Directory of orignal dataset
            string pathLeaf = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(orignal)\\Medicinal_Leaf_dataset";
            string pathPlant = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(orignal)\\Medicinal_plant_dataset";

            ImageAccess imageAccess = new ImageAccess();
            //imageAccess.ParseImage(pathLeaf);
            imageAccess.ParseImage(pathPlant);
        }
    }
}