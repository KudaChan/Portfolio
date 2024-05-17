using Crop_Recommendation_Project.src.DBManupulator;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Crop_Recommendation_Model.src
{
    internal class DataPrepare
    {
        public static Dictionary<string, Tuple<float, float>> DataDictionary = new Dictionary<string, Tuple<float, float>>();

        public static Dictionary<string, Tuple<float, float>> DataRetriver()
        {
            DatabaseConnector db = new DatabaseConnector();

            // Retrieve data from the database
            string sqlQuery = "SELECT field_name, field_min, field_max FROM datastatinfo;";

            using (var reader = db.Read(sqlQuery))
            {
                while (reader.Read())
                {
                    DataDictionary[reader.GetString(0)] = new Tuple<float, float>(reader.GetFloat(1), reader.GetFloat(2));
                }
            }

            Console.WriteLine("{0,-20} {1,-10} {2,-10}", "Field", "Min", "Max");
            foreach (var field in DataDictionary.Keys)
            {
                Console.WriteLine("{0,-20} {1,-10} {2,-10}", field, DataDictionary[field].Item1, DataDictionary[field].Item2);
            }

            db.CloseConnection();
            return DataDictionary;
        }

        public static Dictionary<string, float> UserInputData = new Dictionary<string, float>();

        public static void GetUserInput()
        {
            foreach (var field in DataDictionary.Keys)
            {
                Console.Write($"Please enter a value for {field}: ");
                float value = float.Parse(Console.ReadLine());
                UserInputData[field] = value;
            }
        }

        public static Dictionary<string, float> NormalizedData = new Dictionary<string, float>();

        public static void PrepareData()
        {
            foreach (var field in UserInputData.Keys)
            {
                float value = UserInputData[field];
                float min = DataDictionary[field].Item1;
                float max = DataDictionary[field].Item2;

                // Apply min-max normalization
                float normalizedValue = (value - min) / (max - min);

                NormalizedData[field] = normalizedValue;
            }
        }
    }

    internal class Predictor
    {
        public class ModelInput
        {
            public float N { get; set; }
            public float P { get; set; }
            public float K { get; set; }
            public float Temperature { get; set; }
            public float Humidity { get; set; }
            public float Ph { get; set; }
            public float Rainfall { get; set; }
            public int Label_Idx { get; set; }
        }

        private static Dictionary<int, string> nameLabel = new Dictionary<int, string>
        {
            { 1, "Apple" },
            { 2, "Banana" },
            { 3, "Blackgram" },
            { 4, "Chickpea" },
            { 5, "Coconut" },
            { 6, "Coffee" },
            { 7, "Cotton" },
            { 8, "Grapes" },
            { 9, "Jute" },
            { 10, "Kidneybeans" },
            { 11, "Lentil" },
            { 12, "Maize" },
            { 13, "Mango" },
            { 14, "Mothbeans" },
            { 15, "Mungbean" },
            { 16, "Muskmelon" },
            { 17, "Orange" },
            { 18, "Papaya" },
            { 19, "Pigeonpeas" },
            { 20, "Pomegranate" },
            { 21, "Rice" },
            { 22, "Watermelon" }
        };

        // Define your output data class
        public class ModelOutput
        {
            [ColumnName("PredictedLabel")]
            public int PredictedLabel { get; set; }

            public float[]? Score { get; set; }
        }

        public static void Predict()
        {
            Console.WriteLine("Prediction Started.");

            // Create new MLContext
            MLContext mlContext = new MLContext();

            // Load the models
            string modelPath1 = "D:\\Project\\AI_ML_DS\\Crop-Recommendation-Project\\model\\model_LbfgsME.zip";
            string modelPath2 = "D:\\Project\\AI_ML_DS\\Crop-Recommendation-Project\\model\\model_ovr.zip";
            string modelPath3 = "D:\\Project\\AI_ML_DS\\Crop-Recommendation-Project\\model\\model_sdcaNC.zip";
            ITransformer trainedModel1 = mlContext.Model.Load(modelPath1, out var modelSchema1);
            ITransformer trainedModel2 = mlContext.Model.Load(modelPath2, out var modelSchema2);
            ITransformer trainedModel3 = mlContext.Model.Load(modelPath3, out var modelSchema3);

            // Create prediction engines
            var predictionEngine1 = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(trainedModel1);
            var predictionEngine2 = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(trainedModel2);
            var predictionEngine3 = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(trainedModel3);

            // Create a new input data instance and fill it with normalized data
            ModelInput inputData = new ModelInput
            {
                N = DataPrepare.NormalizedData["n"],
                P = DataPrepare.NormalizedData["p"],
                K = DataPrepare.NormalizedData["k"],
                Temperature = DataPrepare.NormalizedData["temperature"],
                Humidity = DataPrepare.NormalizedData["humidity"],
                Ph = DataPrepare.NormalizedData["ph"],
                Rainfall = DataPrepare.NormalizedData["rainfall"],
            };

            // Make the predictions
            ModelOutput prediction1 = predictionEngine1.Predict(inputData);
            ModelOutput prediction2 = predictionEngine2.Predict(inputData);
            ModelOutput prediction3 = predictionEngine3.Predict(inputData);

            Console.WriteLine("Predictions:");
            Console.WriteLine($"    Model 1: {prediction1.PredictedLabel}");
            Console.WriteLine($"    Model 2: {prediction2.PredictedLabel}");
            Console.WriteLine($"    Model 3: {prediction3.PredictedLabel}");

            Console.WriteLine("Prediction Completed.");

            // Implement your voting system here
            // For example, you could simply choose the prediction with the highest confidence score:
            ModelOutput finalPrediction = new[] { prediction1, prediction2, prediction3 }
                .OrderByDescending(p => p.Score!.Max())
                .First();

            // Print the final prediction
            if (nameLabel.TryGetValue(finalPrediction.PredictedLabel, out string? predictedValue))
            {
                Console.WriteLine($"Final prediction for Given Condition: {predictedValue}");
            }
            else
            {
                Console.WriteLine($"Prediction for Given Condition is not found in nameLabel dictionary.");
            }
        }
    }
}