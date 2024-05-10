using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.ModelTraining
{
    internal class PythonRunner
    {
        public void RunPythonScript()
        {
            WriteLine("Python script is running...");
        }
    }

    internal class PythonMain
    {
        public void PyMain()
        {
            PythonRunner pythonRunner = new PythonRunner();

            pythonRunner.RunPythonScript();
        }
    }
}