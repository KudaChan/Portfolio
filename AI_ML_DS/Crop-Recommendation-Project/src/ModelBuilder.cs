using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
using Crop_Recommendation_Project.src.DataStructure;
using Crop_Recommendation_Project.src.DBManupulator;
using Keras.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using ScottPlot;
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

        public ITransformer SdcaNonCalibratedModelTrainer(IDataView trainingDataView)
        {
            var pipeline = _context.Transforms.Conversion.MapValueToKey(inputColumnName: "Label_Idx", outputColumnName: "Label")
                .Append(_context.Transforms.Concatenate("Features", "N", "P", "K", "Temperature", "Humidity", "Ph", "Rainfall"))
                .Append(_context.MulticlassClassification.Trainers.SdcaNonCalibrated())
                .Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            return pipeline.Fit(trainingDataView);
        }

        public void SdcaNonCalibratedModelValidator(IDataView trainingDataView)
        {
            var pipeline = _context.Transforms.Conversion.MapValueToKey(inputColumnName: "Label_Idx", outputColumnName: "Label")
                .Append(_context.Transforms.Concatenate("Features", "N", "P", "K", "Temperature", "Humidity", "Ph", "Rainfall"))
                .Append(_context.MulticlassClassification.Trainers.SdcaNonCalibrated())
                .Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var cvResults = _context.MulticlassClassification.CrossValidate(trainingDataView, pipeline);
            var microAccuracy = cvResults.Average(r => r.Metrics.MicroAccuracy);
            var macroAccuracy = cvResults.Average(r => r.Metrics.MacroAccuracy);
            Console.WriteLine();
            Console.WriteLine($"Cross-Validated MicroAccuracy: {microAccuracy}");
            Console.WriteLine($"Cross-Validated MacroAccuracy: {macroAccuracy}");
            Console.WriteLine();
        }

        public ITransformer OneVersusAllModelTrainer(IDataView trainingDataView)
        {
            var pipeline = _context.Transforms.Conversion.MapValueToKey(inputColumnName: "Label_Idx", outputColumnName: "Label")
                .Append(_context.Transforms.Concatenate("Features", "N", "P", "K", "Temperature", "Humidity", "Ph", "Rainfall"))
                .Append(_context.MulticlassClassification.Trainers.OneVersusAll(_context.BinaryClassification.Trainers.LdSvm()))
                .Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            return pipeline.Fit(trainingDataView);
        }

        public void OneVersusAllModelValidator(IDataView trainingDataView)
        {
            var pipeline = _context.Transforms.Conversion.MapValueToKey(inputColumnName: "Label_Idx", outputColumnName: "Label")
                .Append(_context.Transforms.Concatenate("Features", "N", "P", "K", "Temperature", "Humidity", "Ph", "Rainfall"))
                .Append(_context.MulticlassClassification.Trainers.OneVersusAll(_context.BinaryClassification.Trainers.LdSvm()))
                .Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var cvResults = _context.MulticlassClassification.CrossValidate(trainingDataView, pipeline);
            var microAccuracy = cvResults.Average(r => r.Metrics.MicroAccuracy);
            var macroAccuracy = cvResults.Average(r => r.Metrics.MacroAccuracy);
            Console.WriteLine();
            Console.WriteLine($"Cross-Validated MicroAccuracy: {microAccuracy}");
            Console.WriteLine($"Cross-Validated MacroAccuracy: {macroAccuracy}");
            Console.WriteLine();
        }

        public ITransformer LbfgsMaximumEntropyModelTrainer(IDataView trainingDataView)
        {
            var pipeline = _context.Transforms.Conversion.MapValueToKey(inputColumnName: "Label_Idx", outputColumnName: "Label")
                .Append(_context.Transforms.Concatenate("Features", "N", "P", "K", "Temperature", "Humidity", "Ph", "Rainfall"))
                .Append(_context.MulticlassClassification.Trainers.LbfgsMaximumEntropy(new Microsoft.ML.Trainers.LbfgsMaximumEntropyMulticlassTrainer.Options
                {
                    L1Regularization = 0.01f,
                    L2Regularization = 0.01f,
                    NumberOfThreads = 1,
                    HistorySize = 20,
                    MaximumNumberOfIterations = 1000,
                    InitialWeightsDiameter = 0.1f,
                    OptimizationTolerance = 1e-8f,
                    DenseOptimizer = true,
                    StochasticGradientDescentInitilaizationTolerance = 1e-6f,
                    ShowTrainingStatistics = true,
                    LabelColumnName = "Label",
                    FeatureColumnName = "Features"
                }))
                .Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            return pipeline.Fit(trainingDataView);
        }

        public void LbfgsMaximumEntropyModelValidator(IDataView trainingDataView)
        {
            var pipeline = _context.Transforms.Conversion.MapValueToKey(inputColumnName: "Label_Idx", outputColumnName: "Label")
                .Append(_context.Transforms.Concatenate("Features", "N", "P", "K", "Temperature", "Humidity", "Ph", "Rainfall"))
                .Append(_context.MulticlassClassification.Trainers.LbfgsMaximumEntropy(new Microsoft.ML.Trainers.LbfgsMaximumEntropyMulticlassTrainer.Options
                {
                    L1Regularization = 0.01f,
                    L2Regularization = 0.01f,
                    NumberOfThreads = 1,
                    HistorySize = 20,
                    MaximumNumberOfIterations = 1000,
                    InitialWeightsDiameter = 0.1f,
                    OptimizationTolerance = 1e-8f,
                    DenseOptimizer = true,
                    StochasticGradientDescentInitilaizationTolerance = 1e-6f,
                    ShowTrainingStatistics = true,
                    LabelColumnName = "Label",
                    FeatureColumnName = "Features"
                }))
                .Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var cvResults = _context.MulticlassClassification.CrossValidate(trainingDataView, pipeline);
            var microAccuracy = cvResults.Average(r => r.Metrics.MicroAccuracy);
            var macroAccuracy = cvResults.Average(r => r.Metrics.MacroAccuracy);
            Console.WriteLine();
            Console.WriteLine($"Cross-Validated MicroAccuracy: {microAccuracy}");
            Console.WriteLine($"Cross-Validated MacroAccuracy: {macroAccuracy}");
            Console.WriteLine();
        }

        [Obsolete]
        public void EvaluateModelAndCreateGraph(ITransformer model, IDataView testDataView, string grphName)
        {
            var predictions = model.Transform(testDataView);
            var metrics = _context.MulticlassClassification.Evaluate(predictions);

            Console.WriteLine($"LogLoss: {metrics.LogLoss}");
            Console.WriteLine($"PerClassLogLoss:");
            foreach (var logLoss in metrics.PerClassLogLoss)
            {
                Console.Write(logLoss + ", ");
            }
            Console.WriteLine();
            Console.WriteLine($"MicroAccuracy: {metrics.MicroAccuracy}");
            Console.WriteLine($"MacroAccuracy: {metrics.MacroAccuracy}");
            Console.WriteLine($"LogLossReduction: {metrics.LogLossReduction}");

            // Log confusion matrix
            Console.WriteLine(metrics.ConfusionMatrix.GetFormattedConfusionTable());

            var actualValues = testDataView.GetColumn<Int32>("Label_Idx").ToArray();
            var predictedValues = predictions.GetColumn<Int32>("PredictedLabel").ToArray();

            var fitPlot = new ScottPlot.Plot();

            var sp1 = fitPlot.Add.Scatter(Enumerable.Range(0, actualValues.Length).Select(x => (double)x).ToArray(), actualValues);
            sp1.LegendText = "Acual";
            sp1.LineWidth = 3;
            sp1.Color = Colors.Magenta;
            sp1.MarkerSize = 15;

            // Add the predicted values to the plot as a scatter plot with red markers
            var sp2 = fitPlot.Add.Scatter(Enumerable.Range(0, predictedValues.Length).Select(x => (double)x).ToArray(), predictedValues);
            sp2.LegendText = "Predicted";
            sp2.LineWidth = 2;
            sp2.Color = Colors.Green;
            sp2.MarkerSize = 10;

            // Customize the plot
            fitPlot.Title("Actual vs. Predicted Crop Labels");
            fitPlot.XLabel("Value");
            fitPlot.YLabel("Crop Label");
            fitPlot.ShowLegend(Alignment.UpperLeft);

            // Save the plot as a JPEG image
            fitPlot.SaveJpeg($"actual_vs_predicted{grphName}.png", 1080, 720, 100);

            // Create a plot
            var plt = new ScottPlot.Plot();

            // Add a bar plot to the plot
            // Add a bar plot to the plot
            var barPlot1 = plt.Add.Bar(position: 1, value: metrics.LogLoss);
            barPlot1.Label = (metrics.LogLoss * 100).ToString();
            var barPlot2 = plt.Add.Bar(position: 2, value: metrics.MicroAccuracy * 100);
            barPlot2.Label = (metrics.MicroAccuracy * 100).ToString();
            var barPlot3 = plt.Add.Bar(position: 3, value: metrics.MacroAccuracy * 100);
            barPlot3.Label = (metrics.MacroAccuracy * 100).ToString();
            var barPlot4 = plt.Add.Bar(position: 4, value: metrics.LogLossReduction);
            barPlot4.Label = (metrics.LogLossReduction).ToString();

            Tick[] ticks =
            {
                new(1, "LogLoss"),
                new(2, "MicroAccuracy"),
                new(3, "MacroAccuracy"),
                new(4, "LogLossReduction")
            };

            // Customize the plot
            barPlot1.ValueLabelStyle.Bold = true;
            barPlot1.ValueLabelStyle.FontSize = 12;
            barPlot2.ValueLabelStyle.Bold = true;
            barPlot2.ValueLabelStyle.FontSize = 12;
            barPlot3.ValueLabelStyle.Bold = true;
            barPlot3.ValueLabelStyle.FontSize = 12;
            barPlot4.ValueLabelStyle.Bold = true;
            barPlot4.ValueLabelStyle.FontSize = 12;

            plt.Title("Model Evaluation Metrics");
            plt.YLabel("Value");
            plt.XLabel("Metric");
            plt.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            plt.Axes.Bottom.MajorTickStyle.Length = 0;
            plt.ShowLegend(Alignment.UpperLeft);
            plt.HideGrid();

            plt.Axes.Margins(bottom: 0);

            // Customize the plot
            plt.Title("Model Evaluation Metrics");
            plt.YLabel("Value");
            plt.XLabel("Metric");

            // Save the plot as a PNG image
            plt.SaveJpeg($"evaluation_metrics{grphName}.png", 1080, 720, 100);
        }

        public void optimizemodel(IDataView trainingDataView)
        {
            var Iterations = new[] { 1000, 750, 500, 200, 100, 10 }; // Change this to control the number of iterations.
            var Tolerance = new[] { 0.001f, 0.0001f }; // Change this to control the convergence tolerance.
            var learningRates = new[] { 0.0001f, 0.001f, 0.01f, 0.1f, 1f };
            var l2Regularizations = new[] { 0.0001f, 0.001f, 0.01f, 0.1f };
            var l1Regularizations = new[] { 0.0001f, 0.001f, 0.01f, 0.1f };

            double bestAccuracy = 0;
            Microsoft.ML.Trainers.SdcaNonCalibratedMulticlassTrainer.Options bestOptions = null;

            foreach (var iteration in Iterations)
            {
                foreach (var tolerance in Tolerance)
                {
                    foreach (var learningRate in learningRates)
                    {
                        foreach (var l2Regularization in l2Regularizations)
                        {
                            foreach (var l1Regularization in l1Regularizations)
                            {
                                var options = new Microsoft.ML.Trainers.SdcaNonCalibratedMulticlassTrainer.Options
                                {
                                    LabelColumnName = "Label",
                                    FeatureColumnName = "Features",
                                    NumberOfThreads = 1, // Change this to control the degree of parallelism.
                                    MaximumNumberOfIterations = iteration, // Change this to control the number of iterations.
                                    ConvergenceTolerance = tolerance, // Change this to control the convergence tolerance.
                                    BiasLearningRate = learningRate, // Change this to control the bias learning rate.
                                    L2Regularization = l2Regularization, // Change this to control the L2 regularization weight.
                                    L1Regularization = l1Regularization, // Change this to control the L1 regularization weight.
                                };

                                var pipeline = _context.Transforms.Conversion.MapValueToKey(inputColumnName: "Label_Idx", outputColumnName: "Label")
                .Append(_context.Transforms.Concatenate("Features", "N", "P", "K", "Temperature", "Humidity", "Ph", "Rainfall"))
                .Append(_context.MulticlassClassification.Trainers.SdcaNonCalibrated(options))
                .Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

                                var cvResults = _context.MulticlassClassification.CrossValidate(trainingDataView, pipeline);
                                var microAccuracy = cvResults.Average(r => r.Metrics.MicroAccuracy);

                                // If this model has better accuracy than the previous best, update the best accuracy and options
                                if (microAccuracy > bestAccuracy)
                                {
                                    bestAccuracy = microAccuracy;
                                    bestOptions = options;

                                    Console.WriteLine("*********************************************************************");
                                    Console.WriteLine($"Best options found:");
                                    Console.WriteLine($"Iterations: {bestOptions.MaximumNumberOfIterations}");
                                    Console.WriteLine($"Tolerance: {bestOptions.ConvergenceTolerance}");
                                    Console.WriteLine($"BiasLearningRate: {bestOptions.BiasLearningRate}");
                                    Console.WriteLine($"L2Regularization: {bestOptions.L2Regularization}");
                                    Console.WriteLine($"L1Regularization: {bestOptions.L1Regularization}");
                                    Console.WriteLine($"Best accuracy: {bestAccuracy}");
                                    Console.WriteLine("**********************************************************************");
                                }
                                Console.WriteLine($"");
                                Console.WriteLine($"Iterations: {iteration}");
                                Console.WriteLine($"Tolerance: {tolerance}");
                                Console.WriteLine($"BiasLearningRate: {learningRate}");
                                Console.WriteLine($"L2Regularization: {l2Regularization}");
                                Console.WriteLine($"L1Regularization: {l1Regularization}");
                                Console.WriteLine("*********************************************************************");
                                Console.WriteLine(bestAccuracy);
                                Console.WriteLine("**********************************************************************");
                                Console.WriteLine($"Best accuracy: {microAccuracy}");

                                Console.WriteLine("---------------------------------------------------------------------------------------------------------");
                            }
                        }
                    }
                }
            }
            if (bestOptions != null)
            {
                Console.WriteLine($"Best options found:");
                Console.WriteLine($"Iterations: {bestOptions.MaximumNumberOfIterations}");
                Console.WriteLine($"Tolerance: {bestOptions.ConvergenceTolerance}");
                Console.WriteLine($"BiasLearningRate: {bestOptions.BiasLearningRate}");
                Console.WriteLine($"L2Regularization: {bestOptions.L2Regularization}");
                Console.WriteLine($"L1Regularization: {bestOptions.L1Regularization}");
                Console.WriteLine($"Best accuracy: {bestAccuracy}");
            }
            else
            {
                Console.WriteLine("No options were evaluated.");
            }
        }
    }
}