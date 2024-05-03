using Crop_Recommendation_Project.src.DBManupulator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crop_Recommendation_Project.src.GeneralFunction
{
    internal class GeneralFunc
    {
        private Dictionary<string, int> labelToIndexMap = new Dictionary<string, int>
        {
            { "apple", 1 },
            { "banana", 2 },
            { "blackgram", 3 },
            { "chickpea", 4 },
            { "coconut", 5 },
            { "coffee", 6 },
            { "cotton", 7 },
            { "grapes", 8 },
            { "jute", 9 },
            { "kidneybeans", 10 },
            { "lentil", 11 },
            { "maize", 12 },
            { "mango", 13 },
            { "mothbeans", 14 },
            { "mungbean", 15 },
            { "muskmelon", 16 },
            { "orange", 17 },
            { "papaya", 18 },
            { "pigeonpeas", 19 },
            { "pomegranate", 20 },
            { "rice", 21 },
            { "watermelon", 22 }
        };

        public void updater()
        {
            DatabaseConnector dbConnector = new DatabaseConnector();

            string query = "Select label from idxlbcropdb;";
            //var reader = dbConnector.Read(query);

            List<string> lables = new List<string>();
            using (var reader = dbConnector.Read(query))
            {
                while (reader.Read())
                {
                    lables.Add(reader.GetString(0));
                }
            }

            foreach (var label in lables)
            {
                if (labelToIndexMap.TryGetValue(label, out int index))
                {
                    string updateQuery = String.Format("Update idxlbcropdb set label_idx = {0} where label = '{1}';", index, label);
                    dbConnector.Update(updateQuery);
                }
                else
                {
                    Console.WriteLine($"Error: No index found for label {label}");
                }
            }

            dbConnector.CloseConnection();
        }

        public void outlierByIQR()
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

                    Console.WriteLine($"Outliers in {column.ColumnName} in {idx} detected by IQR method: {string.Join(", ", outliers)}");
                }
            }
        }
    }
}