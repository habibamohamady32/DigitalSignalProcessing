using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            //for smoothing signal
            float sum = 0;
            List<float> outSignal = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count - InputWindowSize + 1; i++)
            {
                for (int j = 0; j < InputWindowSize; j++)
                {
                    sum += InputSignal.Samples[i + j];
                }
                outSignal.Add(sum / InputWindowSize);
                sum = 0;
            }
            OutputAverageSignal = new Signal(outSignal, false);
        }
    }
}
