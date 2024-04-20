using Minor_Project_Ai_Plant_Recognition.SorceCode.Preprocessing;
using System.Drawing;

namespace Minor_Project_Ai_Plant_Recognition
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Directory of orignal dataset
            string path = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(test)";
            string pathTextFile = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Temp\\dataset";

            string[] action = ["resize", "Augment"];

            ImageAccess imageAccess = new ImageAccess();

            ImageResize imageResize = new ImageResize();

            DataAugmentation augmentation = new DataAugmentation();

            imageAccess.DirectoryParser(path, pathTextFile, action[0]);
            imageResize.ResizeFactory(pathTextFile);

            string pathResized = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(resized)";
            imageAccess.DirectoryParser(pathResized, pathTextFile, action[1]);

            augmentation.AugmentFactory(pathTextFile);
        }
    }
}