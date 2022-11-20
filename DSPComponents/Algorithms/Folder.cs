using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            List<float> sampleResult = new List<float>();
            for (int i = InputSignal.Samples.Count - 1; i >= 0; i--)
            {
                sampleResult.Add(InputSignal.Samples[i]);
            }
            OutputFoldedSignal = new Signal(sampleResult, InputSignal.SamplesIndices, false);
            if (InputSignal.Periodic == true)
            {
                OutputFoldedSignal.Periodic = false;
            }
            else
                OutputFoldedSignal.Periodic = true;
        }
    }
}
