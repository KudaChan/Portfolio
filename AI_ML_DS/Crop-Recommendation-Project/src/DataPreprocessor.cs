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
            OutlierDetector();
            //Normalizer();
            //DataSplitter();
        }

        private static void DataInfoExtractor()
        {
            DatabaseConnector dbConnector = new DatabaseConnector();
            string query = " Create table if not exists DataStatInfo (field_name varchar(50), data_type varchar(50), mean real, median real, mode real, varience real, std_deviation real)";
            dbConnector.Create(query);

            string selectQuery = "Select * from CropData;";
            var reader = dbConnector.Read(selectQuery);

            List<string> fieldNames = new List<string>();
            List<string> dataTypes = new List<string>();
            List<double> data = new List<double>();
            List<double> mean = new List<double>();
            List<double> median = new List<double>();
            List<double> mode = new List<double>();
            List<double> varience = new List<double>();
            List<double> stdDeviation = new List<double>();

            DataTable dt = new DataTable();
            dt.Load(reader);

            foreach (DataColumn column in dt.Columns)
            {
                fieldNames.Add(column.ColumnName);
                dataTypes.Add(column.DataType.Name);
            }

            for (int i = 0; i < dt.Columns.Count - 1; i++)
            {
                data.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    data.Add(Convert.ToDouble(row[i]));
                }

                mean.Add(data.Average());
                median.Add(data[data.Count / 2]);
                mode.Add(data.GroupBy(v => v).OrderByDescending(g => g.Count()).Select(g => g.Key).First());
                varience.Add(data.Select(v => (v - mean[i]) * (v - mean[i])).Sum() / data.Count);
                stdDeviation.Add(Math.Sqrt(varience[i]));
            }

            for (int i = 0; i < dt.Columns.Count - 1; i++)
            {
                string insertQuery = String.Format("Insert into DataStatInfo (field_name, data_type, mean, median, mode, varience, std_deviation) values ('{0}', '{1}', {2}, {3}, {4}, {5}, {6})", fieldNames[i], dataTypes[i], mean[i], median[i], mode[i], varience[i], stdDeviation[i]);
                dbConnector.Create(insertQuery);
            }

            dbConnector.CloseConnection();
        }

        private static void OutlierDetector()
        {
            DatabaseConnector dbConnector = new DatabaseConnector();

            string selectQuery = "Select label_idx from lbidx;";
            var reader = dbConnector.Read(selectQuery);

            DataTable dt = new DataTable();
            dt.Load(reader);

            List<int> labelIdx = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                labelIdx.Add(Convert.ToInt32(row[0]));
            }

            foreach (var idx in labelIdx)
            {
                selectQuery = "Select * from idxlbcropdb where label_idx = " + idx + ";";
                reader = dbConnector.Read(selectQuery);

                dt = new DataTable();
                dt.Load(reader);

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.DataType.Name != "Double")
                    {
                        continue;
                    }

                    List<double> data = new List<double>();
                    foreach (DataRow row in dt.Rows)
                    {
                        data.Add(Convert.ToDouble(row[column.ColumnName]));
                    }

                    data.Sort();
                    double q1 = data[data.Count / 4];
                    double q3 = data[data.Count * 3 / 4];
                    double iqr = q3 - q1;
                    double lowerBound = q1 - 1.5 * iqr;
                    double upperBound = q3 + 1.5 * iqr;

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
        }

        private static void Normalizer()
        {
            DatabaseConnector dbconnector = new DatabaseConnector();
            string createQuery = "Create table if not exists NormalizedDB (N real, P real, K real, temperature real, humidity real, ph real, rainfall real, label_idx real);";
            dbconnector.Create(createQuery);
            string selectQuery = "Select * from idxlbcropdb;";
            using (var reader = dbconnector.Read(selectQuery))
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

                    var data = dt.AsEnumerable().Select(r => r.Field<double>(column.ColumnName)).ToList();

                    double min = data.Min();
                    double max = data.Max();

                    var normalizedData = data.Select(value => (value - min) / (max - min)).ToList();

                    // Update the data in the DataTable
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i][column] = normalizedData[i];
                    }
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string insertQuery = String.Format("Insert into NormalizedDB (N, P, K, temperature, humidity, ph, rainfall, label_idx) values ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", dt.Rows[i][0], dt.Rows[i][1], dt.Rows[i][2], dt.Rows[i][3], dt.Rows[i][4], dt.Rows[i][5], dt.Rows[i][6], dt.Rows[i][7]);
                    dbconnector.Create(insertQuery);
                }
            }

            dbconnector.CloseConnection();
        }

        private static void DataSplitter()
        {
            DatabaseConnector dbconnector = new DatabaseConnector();
            string createQuery = "Create table if not exists TrainData (N real, P real, K real, temperature real, humidity real, ph real, rainfall real, label_idx real);";
            dbconnector.Create(createQuery);
            createQuery = "Create table if not exists TestData (N real, P real, K real, temperature real, humidity real, ph real, rainfall real, label_idx real);";
            dbconnector.Create(createQuery);

            string selectQuery = "Select * from NormalizedDB;";
            using (var reader = dbconnector.Read(selectQuery))
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

                foreach (var idx in trainIndices)
                {
                    Console.WriteLine("Train: " + idx);
                    string insertQuery = String.Format("Insert into TrainData (N, P, K, temperature, humidity, ph, rainfall, label_idx) values ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", dt.Rows[idx][0], dt.Rows[idx][1], dt.Rows[idx][2], dt.Rows[idx][3], dt.Rows[idx][4], dt.Rows[idx][5], dt.Rows[idx][6], dt.Rows[idx][7]);
                    dbconnector.Create(insertQuery);
                }

                foreach (var idx in testIndices)
                {
                    Console.WriteLine("Test: " + idx);
                    string insertQuery = String.Format("Insert into TestData (N, P, K, temperature, humidity, ph, rainfall, label_idx) values ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", dt.Rows[idx][0], dt.Rows[idx][1], dt.Rows[idx][2], dt.Rows[idx][3], dt.Rows[idx][4], dt.Rows[idx][5], dt.Rows[idx][6], dt.Rows[idx][7]);
                    dbconnector.Create(insertQuery);
                }
            }

            dbconnector.CloseConnection();
        }
    }
}