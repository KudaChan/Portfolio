using Python.Runtime;
using System;

// calling python script in the C# code
namespace pyhtonTemp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic np = Py.Import("numpy");
                Console.WriteLine(np.cos(np.pi * 2));
            }
        }
    }
}