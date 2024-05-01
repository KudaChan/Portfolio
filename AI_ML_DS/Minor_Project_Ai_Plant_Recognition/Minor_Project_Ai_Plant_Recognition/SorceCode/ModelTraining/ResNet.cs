using Keras.PreProcessing.Image;
using Python.Runtime;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.ModelTraining
{
    internal class ResNetImp
    {
        public void ResNetStarter()
        {
        }

        public void ResNetImplementation()
        {
            ImageDataGenerator imageDataGenerator = new ImageDataGenerator();

            string trainDataPath = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataTestTrainValid\\Dataset(train)\\Dataset(normalized)\\resized\\size224_224";

            string testDataPath = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataTestTrainValid\\Dataset(test)\\Dataset(normalized)\\resized\\size224_224";

            string validDataPath = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Dataset\\DataTestTrainValid\\Dataset(validate)\\Dataset(normalized)\\resized\\size224_224";

            var trainData = imageDataGenerator.FlowFromDirectory(trainDataPath, target_size: null, batch_size: 32, class_mode: "categorical", interpolation: null);
            var testData = imageDataGenerator.FlowFromDirectory(testDataPath, target_size: null, batch_size: 32, class_mode: "categorical", interpolation: null);
            var validData = imageDataGenerator.FlowFromDirectory(validDataPath, target_size: null, batch_size: 32, class_mode: "categorical", interpolation: null);

            Console.WriteLine(trainData);
            Console.WriteLine(testData);
            Console.WriteLine(validData);
        }
    }
}