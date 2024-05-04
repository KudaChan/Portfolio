using Crop_Recommendation_Project.src.DBManupulator;
using Crop_Recommendation_Project.src.DataStructure;
using Microsoft.ML;
using System.Data;

namespace Crop_Recommendation_Project.src
{
    internal class ModelBuilder
    {
        private readonly MLContext _context;

        public ModelBuilder()
        {
            _context = new MLContext();
        }

        public IDataView LoadData(string sqlQuery)
        {
            var dbConnector = new DatabaseConnector();
            var reader = dbConnector.Read(sqlQuery);

            DataTable dt = new DataTable();
            dt.Load(reader);

            List<CropIdxDataStructure> cropDataList = new List<CropIdxDataStructure>();
            foreach (DataRow row in dt.Rows)
            {
                cropDataList.Add(new CropIdxDataStructure
                {
                    N = Convert.ToSingle(row["N"]),
                    P = Convert.ToSingle(row["P"]),
                    K = Convert.ToSingle(row["K"]),
                    Temperature = Convert.ToSingle(row["Temperature"]),
                    Humidity = Convert.ToSingle(row["Humidity"]),
                    Ph = Convert.ToSingle(row["Ph"]),
                    Rainfall = Convert.ToSingle(row["Rainfall"]),
                    Label_Idx = Convert.ToInt32(row["Label_Idx"])
                });
            }

            reader.Close();
            dbConnector.CloseConnection();

            return _context.Data.LoadFromEnumerable(cropDataList);
        }

        public ITransformer BuildAndTrainModel(IDataView trainingDataView)
        {
            var pipeline = _context.Transforms.Conversion.MapValueToKey(inputColumnName: "Label_Idx", outputColumnName: "Label")
                .Append(_context.Transforms.Concatenate("Features", "N", "P", "K", "Temperature", "Humidity", "Ph", "Rainfall"))
                .Append(_context.MulticlassClassification.Trainers.SdcaNonCalibrated())
                .Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            return pipeline.Fit(trainingDataView);
        }

        public void CrossValidateModel(IDataView trainingDataView)
        {
            var pipeline = _context.Transforms.Conversion.MapValueToKey(inputColumnName: "Label_Idx", outputColumnName: "Label")
                .Append(_context.Transforms.Concatenate("Features", "N", "P", "K", "Temperature", "Humidity", "Ph", "Rainfall"))
                .Append(_context.MulticlassClassification.Trainers.SdcaNonCalibrated())
                .Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var cvResults = _context.MulticlassClassification.CrossValidate(trainingDataView, pipeline);
            var microAccuracy = cvResults.Average(r => r.Metrics.MicroAccuracy);
            Console.WriteLine($"Cross-Validated MicroAccuracy: {microAccuracy}");
        }

        public void EvaluateModel(ITransformer model, IDataView testDataView)
        {
            var predictions = model.Transform(testDataView);
            var metrics = _context.MulticlassClassification.Evaluate(predictions);
            // Here you can log the metrics, for example:
            Console.WriteLine($"LogLoss: {metrics.LogLoss}");
            Console.WriteLine($"PerClassLogLoss: {metrics.PerClassLogLoss}");
            Console.WriteLine($"MicroAccuracy: {metrics.MicroAccuracy}");
            Console.WriteLine($"MacroAccuracy: {metrics.MacroAccuracy}");
            Console.WriteLine($"LogLossReduction: {metrics.LogLossReduction}");

            // Log confusion matrix
            Console.WriteLine(metrics.ConfusionMatrix.GetFormattedConfusionTable());
        }
    }
}