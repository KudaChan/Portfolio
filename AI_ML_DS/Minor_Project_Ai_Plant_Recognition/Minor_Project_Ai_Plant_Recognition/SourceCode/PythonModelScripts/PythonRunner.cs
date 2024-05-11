using Python.Runtime;

namespace Minor_Project_Ai_Plant_Recognition.SourceCode.ModelTraining
{
    internal class PythonMain
    {
        public static void PyMain()
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
                    string pythonScript = File.ReadAllText(@"D:\Project\AI_ML_DS\Minor_Project_Ai_Plant_Recognition\Minor_Project_Ai_Plant_Recognition\SourceCode\PythonModelScripts\python_main.py");
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