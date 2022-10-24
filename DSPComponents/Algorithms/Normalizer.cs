using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            List<float> result = new List<float>();
            float output;
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                //x normalized = (x – x minimum) / (x maximum – x minimum)
                output = (InputMaxRange - InputMinRange)
                    * ((InputSignal.Samples[i] - InputSignal.Samples.Min()) / (InputSignal.Samples.Max() - InputSignal.Samples.Min()))
                    + InputMinRange;

                result.Add(output);
            }
            OutputNormalizedSignal = new Signal(result, false);
        }
    }
}
