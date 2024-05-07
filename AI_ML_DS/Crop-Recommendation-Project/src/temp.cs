using Crop_Recommendation_Project.src.DBManupulator;
using Keras;
using Keras.Models;
using Numpy;
using NumSharp;
using System.Data;
using CNTK;
using System.Collections;

namespace Crop_Recommendation_Model.src
{
    internal class Temp
    {
        public (double[][], int[]) DataLoader(string sqlQuery)
        {
            var dbConnector = new DatabaseConnector();
            var reader = dbConnector.Read(sqlQuery);

            DataTable dt = new DataTable();
            dt.Load(reader);

            double[][] data = new double[dt.Rows.Count][];
            int[] labels = new int[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data[i] = new double[dt.Columns.Count];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    data[i][j] = Convert.ToDouble(dt.Rows[i][j]);
                }
                labels[i] = Convert.ToInt32(dt.Rows[i][dt.Columns.Count - 1]);
            }

            return (data, labels);
        }

        public void NNModel(double[][] inputData, int[] labels)
        {
            //Sequential model = new Sequential();
            //model.Add(new Dense(128, activation: "relu", input_shape: new Shape(input[0].Length)));
            //model.Add(new Dense(64, activation: "relu"));
            //model.Add(new Dense(32, activation: "relu"));
            //model.Add(new Dense(16, activation: "relu"));
            //model.Add(new Dense(8, activation: "relu"));
            //model.Add(new Dense(4, activation: "relu"));
            //model.Add(new Dense(1, activation: "sigmoid"));

            //model.Compile(optimizer: "adam", loss: "binary_crossentropy", metrics: new string[] { "accuracy" });

            //model.Fit(input, output, batch_size: 10, epochs: 100, verbose: 1);

            Variable inputVar = Variable.InputVariable(new int[] { inputData[0].Length }, CNTK.DataType.Float);
            Variable labelVar = Variable.InputVariable(new int[] { labels.Length }, CNTK.DataType.Float);

            Func<Variable, CNTK.Function> func = (X) => CNTKLib.PastValue(X, 1);
            Function relu1 = CNTKLib.ReLU(inputVar, "relu1");
            Function relu2 = CNTKLib.ReLU(relu1, "relu2");
            Function relu3 = CNTKLib.ReLU(relu2, "relu3");
            Function relu4 = CNTKLib.ReLU(relu3, "relu4");
            Function relu5 = CNTKLib.ReLU(relu4, "relu5");
            Function relu6 = CNTKLib.ReLU(relu5, "relu6");
            Function sigmoid = CNTKLib.Sigmoid(relu6, "sigmoid");

            Function loss = CNTKLib.BinaryCrossEntropy(sigmoid, labelVar, "loss");
            Function evalError = CNTKLib.ClassificationError(sigmoid, labelVar, "error");

            var learner = CNTKLib.AdamLearner(new ParameterVector((ICollection)loss.Parameters()), new TrainingParameterScheduleDouble(0.01), new TrainingParameterScheduleDouble(0.9));

            Console.WriteLine("Model Trained Successfully.");
        }
    }
}