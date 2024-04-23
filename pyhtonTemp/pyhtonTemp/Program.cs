using Python.Runtime;
using System;

// calling python script in the C# code
namespace pyhtonTemp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Runtime.PythonDLL = @"C:\Users\kumar\AppData\Local\Programs\Python\Python312\python312.dll";
            // Set the Python HOME environment variable (replace with your actual path)
            //Environment.SetEnvironmentVariable("PYTHONHOME", @"C:\Users\kumar\AppData\Local\Programs\Python\Python312", EnvironmentVariableTarget.Process);

            // Set the Python PATH environment variable (replace with your actual path)
            //Environment.SetEnvironmentVariable("PYTHONPATH", @"C:\myPyEnv\testEnv\Lib\site-packages", EnvironmentVariableTarget.Process);

            try
            {
                PythonEngine.Initialize();
                using (Py.GIL())
                {
                    string pythoncode = System.IO.File.ReadAllText(@"D:\Project\pyhtonTemp\pyhtonTemp\test.py");
                    PythonEngine.RunSimpleString(pythoncode);

                    dynamic result = PythonEngine.RunSimpleString("my_sum()");
                }
            }
            catch (PythonException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                PythonEngine.Shutdown();
            }
        }
    }
}