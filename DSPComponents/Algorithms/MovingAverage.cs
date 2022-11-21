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
            OutputAverageSignal = new Signal(new List<float>(), false);
            int itr = InputSignal.Samples.Count;
            int sides = (InputWindowSize - 1) / 2;
            for (int i = 0; i < itr; i++)
            {
                if (i >= sides && i < itr - sides)
                {
                    float sum = InputSignal.Samples[i];
                    for (int j = 1; j <= sides; j++)
                    {
                        sum += InputSignal.Samples[i - j];
                        sum += InputSignal.Samples[i + j];
                    }
                    float avg = sum / InputWindowSize;
                    OutputAverageSignal.Samples.Add(avg);
                }
            }
        }
    }
}
