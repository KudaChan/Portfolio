using Crop_Recommendation_Project.src.DBManupulator;
using Crop_Recommendation_Project.src.GeneralFunction;
using ScottPlot;
using System.Data;
using System.Data.Common;

namespace Crop_Recommendation_Project.src.Preprocessor
{
    internal class DataPreprocessor
    {
        public static void Preprocessor()
        {
            //DataInfoExtractor();
            //OutlierDetector();
            //Normalizer();
            //DataSplitter();
        }

        private static void DataInfoExtractor()
        {
            DatabaseConnector dbConnector = new DatabaseConnector();
            string query = "CREATE TABLE IF NOT EXISTS DataStatInfo (field_name varchar(50), data_type varchar(50), mean real, median real, mode real, variance real, std_deviation real)";
            dbConnector.Create(query);

            string selectQuery = "SELECT * FROM CropData;";
            var reader = dbConnector.Read(selectQuery);

            DataTable dt = new DataTable();
            dt.Load(reader);

            List<string> fieldNames = new List<string>();
            List<string> dataTypes = new List<string>();
            List<double> mean = new List<double>();
            List<double> median = new List<double>();
            List<double> mode = new List<double>();
            List<double> variance = new List<double>();
            List<double> stdDeviation = new List<double>();

            foreach (DataColumn column in dt.Columns)
            {
                fieldNames.Add(column.ColumnName);
                dataTypes.Add(column.DataType.Name);
            }

            for (int i = 0; i < dt.Columns.Count - 1; i++)
            {
                List<double> data = dt.AsEnumerable().Select(row => Convert.ToDouble(row[i])).ToList();

                mean.Add(data.Average());
                median.Add(data[data.Count / 2]);
                mode.Add(data.GroupBy(v => v).OrderByDescending(g => g.Count()).Select(g => g.Key).First());
                variance.Add(data.Select(v => (v - mean[i]) * (v - mean[i])).Sum() / data.Count);
                stdDeviation.Add(Math.Sqrt(variance[i]));
            }

            for (int i = 0; i < dt.Columns.Count - 1; i++)
            {
                string insertQuery = $"INSERT INTO DataStatInfo (field_name, data_type, mean, median, mode, variance, std_deviation) VALUES ('{fieldNames[i]}', '{dataTypes[i]}', {mean[i]}, {median[i]}, {mode[i]}, {variance[i]}, {stdDeviation[i]})";
                dbConnector.Create(insertQuery);
            }

            dbConnector.CloseConnection();
        }

        private static void OutlierDetector()
        {
            DatabaseConnector dbConnector = new DatabaseConnector();

            string selectQuery = "SELECT label_idx FROM lbidx;";
            var reader = dbConnector.Read(selectQuery);

            DataTable dt = new DataTable();
            dt.Load(reader);

            List<int> labelIdx = dt.AsEnumerable().Select(row => Convert.ToInt32(row[0])).ToList();

            foreach (var idx in labelIdx)
            {
                selectQuery = $"SELECT * FROM idxlbcropdb WHERE label_idx = {idx};";
                reader = dbConnector.Read(selectQuery);

                dt = new DataTable();
                dt.Load(reader);

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.DataType.Name != "Double")
                    {
                        continue;
                    }

                    List<double> data = dt.AsEnumerable().Select(row => Convert.ToDouble(row[column.ColumnName])).ToList();

                    data.Sort();
                    double q1 = data[data.Count / 4];
                    double q3 = data[data.Count * 3 / 4];
                    double iqr = q3 - q1;
                    double lowerBound = q1 - 1.5 * iqr;
                    double upperBound = q3 + 1.5 * iqr;
                    double median = data[data.Count / 2];

                    var outliers = data.Where(x => x < lowerBound || x > upperBound).ToList();

                    if (outliers.Count > 0)
                    {
                        Console.WriteLine($"Outliers in {column.ColumnName} in {idx} detected by IQR method: {string.Join(", ", outliers)}");
                    }
                    else
                    {
                        Console.WriteLine($"No outliers detected in {column.ColumnName} in {idx} by IQR method.");
                    }
                }
            }

            foreach (DataColumn column in dt.Columns)
            {
                if (column.DataType.Name != "Double")
                {
                    continue;
                }

                ScottPlot.Plot outlierPlot = new();
                double overallMin = double.MaxValue;
                double overallMax = double.MinValue;

                foreach (var idx in labelIdx)
                {
                    selectQuery = $"SELECT {column.ColumnName} FROM idxlbcropdb WHERE label_idx = {idx};";
                    reader = dbConnector.Read(selectQuery);

                    dt = new DataTable();
                    dt.Load(reader);

                    List<double> data = dt.AsEnumerable().Select(row => Convert.ToDouble(row[0])).ToList();

                    data.Sort();
                    double q1 = data[data.Count / 4];
                    double q3 = data[data.Count * 3 / 4];
                    double iqr = q3 - q1;
                    double lowerBound = q1 - 1.5 * iqr;
                    double upperBound = q3 + 1.5 * iqr;
                    double median = data[data.Count / 2];

                    ScottPlot.Box boxPlot = new()
                    {
                        Position = idx,
                        BoxMin = q1,
                        BoxMax = q3,
                        WhiskerMin = lowerBound,
                        WhiskerMax = upperBound,
                        BoxMiddle = median,
                    };

                    outlierPlot.Add.Box(boxPlot);
                }

                outlierPlot.Axes.SetLimits(0, labelIdx.Max() + 1, -1, 145);

                // Save the plot as a JPEG image
                outlierPlot.SaveJpeg($"boxplot_{column.ColumnName}.jpeg", 600, 400);
            }
        }

        private static void Normalizer()
        {
            DatabaseConnector dbConnector = new DatabaseConnector();
            string createQuery = "CREATE TABLE IF NOT EXISTS NormalizedDB (N real, P real, K real, temperature real, humidity real, ph real, rainfall real, label_idx real)";
            dbConnector.Create(createQuery);

            string selectQuery = "SELECT * FROM idxlbcropdb;";
            using (var reader = dbConnector.Read(selectQuery))
            {
                DataTable dt = new DataTable();
                dt.Load(reader);

                foreach (DataColumn column in dt.Columns)
                {
                    column.ReadOnly = false;
                    if (column.DataType != typeof(double))
                    {
                        continue;
                    }

                    var data = dt.AsEnumerable().Select(row => row.Field<double>(column.ColumnName)).ToList();

                    double min = data.Min();
                    double max = data.Max();

                    var normalizedData = data.Select(value => (value - min) / (max - min)).ToList();

                    // Update the data in the DataTable
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i][column] = normalizedData[i];
                    }
                }

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    string insertQuery = $"INSERT INTO NormalizedDB (N, P, K, temperature, humidity, ph, rainfall, label_idx) VALUES ({dt.Rows[i][0]}, {dt.Rows[i][1]}, {dt.Rows[i][2]}, {dt.Rows[i][3]}, {dt.Rows[i][4]}, {dt.Rows[i][5]}, {dt.Rows[i][6]}, {dt.Rows[i][7]})";
                //    dbConnector.Create(insertQuery);
                //}
            }

            dbConnector.CloseConnection();
        }

        private static void DataSplitter()
        {
            DatabaseConnector dbConnector = new DatabaseConnector();
            string createQuery = "CREATE TABLE IF NOT EXISTS TrainData (N real, P real, K real, temperature real, humidity real, ph real, rainfall real, label_idx real)";
            dbConnector.Create(createQuery);
            createQuery = "CREATE TABLE IF NOT EXISTS TestData (N real, P real, K real, temperature real, humidity real, ph real, rainfall real, label_idx real)";
            dbConnector.Create(createQuery);

            string selectQuery = "SELECT * FROM NormalizedDB;";
            using (var reader = dbConnector.Read(selectQuery))
            {
                DataTable dt = new DataTable();
                dt.Load(reader);

                Random random = new Random();
                List<int> trainIndices = new List<int>();
                List<int> testIndices = new List<int>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (random.NextDouble() < 0.8)
                    {
                        trainIndices.Add(i);
                    }
                    else
                    {
                        testIndices.Add(i);
                    }
                }

                //foreach (var idx in trainIndices)
                //{
                //    Console.WriteLine("Train: " + idx);
                //    string insertQuery = $"INSERT INTO TrainData (N, P, K, temperature, humidity, ph, rainfall, label_idx) VALUES ({dt.Rows[idx][0]}, {dt.Rows[idx][1]}, {dt.Rows[idx][2]}, {dt.Rows[idx][3]}, {dt.Rows[idx][4]}, {dt.Rows[idx][5]}, {dt.Rows[idx][6]}, {dt.Rows[idx][7]})";
                //    dbConnector.Create(insertQuery);
                //}

                //foreach (var idx in testIndices)
                //{
                //    Console.WriteLine("Test: " + idx);
                //    string insertQuery = $"INSERT INTO TestData (N, P, K, temperature, humidity, ph, rainfall, label_idx) VALUES ({dt.Rows[idx][0]}, {dt.Rows[idx][1]}, {dt.Rows[idx][2]}, {dt.Rows[idx][3]}, {dt.Rows[idx][4]}, {dt.Rows[idx][5]}, {dt.Rows[idx][6]}, {dt.Rows[idx][7]})";
                //    dbConnector.Create(insertQuery);
                //}
            }

            PlotData();

            dbConnector.CloseConnection();
        }

        private static void PlotData()
        {
            DatabaseConnector dbConnector = new DatabaseConnector();

            // Get the count of each label in the training dataset
            string selectQuery = "SELECT label_idx, COUNT(*) FROM TrainData GROUP BY label_idx;";
            var reader = dbConnector.Read(selectQuery);
            DataTable dtTrain = new DataTable();
            dtTrain.Load(reader);

            // Get the count of each label in the test dataset
            selectQuery = "SELECT label_idx, COUNT(*) FROM TestData GROUP BY label_idx;";
            reader = dbConnector.Read(selectQuery);
            DataTable dtTest = new DataTable();
            dtTest.Load(reader);

            // Create the plot
            var plt = new ScottPlot.Plot();

            // Add the bar series for the training dataset
            float[] trainLabels = dtTrain.AsEnumerable().Select(row => row.Field<float>("label_idx")).ToArray();
            long[] trainCounts = dtTrain.AsEnumerable().Select(row => row.Field<long>("count")).ToArray();
            var bars1 = plt.Add.Bars(trainLabels.Select(x => (double)x).ToArray(), trainCounts.Select(x => (double)x).ToArray());
            bars1.LegendText = "train";

            // Add the bar series for the test dataset
            float[] testLabels = dtTest.AsEnumerable().Select(row => row.Field<float>("label_idx")).ToArray();
            long[] testCounts = dtTest.AsEnumerable().Select(row => -row.Field<long>("count")).ToArray();
            var bars2 = plt.Add.Bars(testLabels.Select(x => (double)x).ToArray(), testCounts.Select(x => (double)x).ToArray());
            bars2.LegendText = "test";

            plt.HideLegend();

            ScottPlot.Panels.LegendPanel pan = new(plt.Legend)
            {
                Edge = Edge.Right,
                Alignment = Alignment.UpperCenter,
            };
            // Customize the plot
            plt.Title("Train/Test Dataset Entries by Label");
            plt.XLabel("Label");
            plt.YLabel("Number of Entries");
            //plt.ShowLegend(Alignment.UpperLeft, Orientation.Horizontal);
            plt.Axes.Margins(bottom: 0);
            plt.Axes.AddPanel(pan);

            // Save the plot as a PNG image
            plt.SaveJpeg("barplot.png", 1080, 720);

            dbConnector.CloseConnection();
        }
    }
}