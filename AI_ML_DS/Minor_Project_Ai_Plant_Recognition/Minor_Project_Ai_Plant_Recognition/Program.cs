using Minor_Project_Ai_Plant_Recognition.SorceCode.Preprocessing;

namespace Minor_Project_Ai_Plant_Recognition
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Directory of orignal dataset
            string path = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(test)";
            string pathTextFile = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Temp\\dataset";
            ImageAccess imageAccess = new ImageAccess();
            imageAccess.DirectoryParser(path, pathTextFile);

            ImageResize imageResize = new ImageResize();
            imageResize.ResizeFactory(pathTextFile);
        }
    }
}