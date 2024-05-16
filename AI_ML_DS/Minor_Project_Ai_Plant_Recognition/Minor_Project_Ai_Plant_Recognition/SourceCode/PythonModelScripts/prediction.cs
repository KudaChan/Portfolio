using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minor_Project_Ai_Plant_Recognition.SourceCode.PythonModelScripts
{
    internal class Prediction
    {
        public static void PredictMain()
        {
            try
            {
                // Path to the Python DLL
                Runtime.PythonDLL = @"C:\Users\kumar\AppData\Local\Programs\Python\Python312\python312.dll";
                // Setting the PYTHONHOME environment variable
                Environment.SetEnvironmentVariable("PYTHONHOME", @"C:\Users\kumar\AppData\Local\Programs\Python\Python312", EnvironmentVariableTarget.Process);

                // Initialize the Python Engine
                PythonEngine.Initialize();
                //PythonScriptRemoveBackground();

                using (Py.GIL())
                {
                    // Import os and sys modules
                    dynamic os = Py.Import("os");
                    dynamic sys = Py.Import("sys");

                    // Add the Python script's directory to sys.path
                    string scriptDir = @"D:\Project\AI_ML_DS\Minor_Project_Ai_Plant_Recognition\Minor_Project_Ai_Plant_Recognition\SourceCode\PythonModelScripts";
                    sys.path.append(scriptDir);

                    // Read the Python script
                    string pythonScript = File.ReadAllText(@"D:\Project\AI_ML_DS\Minor_Project_Ai_Plant_Recognition\Minor_Project_Ai_Plant_Recognition\SourceCode\PythonModelScripts\predict.py");
                    // Run the Python script
                    PythonEngine.RunSimpleString(pythonScript);
                }
            }
            catch (Exception e)
            {
                WriteLine($"Failed to Run Python Script in PythonRunner.cs: {e.Message}");
            }
            finally
            {
                // Shutdown the Python Engine
                PythonEngine.Shutdown();
            }
        }
    }
}