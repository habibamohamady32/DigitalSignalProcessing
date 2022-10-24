using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        public override void Run()
        {

            List<float> result = new List<float>();
            float output, maximum_signal;
            if (InputSignal1.Samples.Count > InputSignal2.Samples.Count)
            {
                maximum_signal = InputSignal1.Samples.Count;
            }
            else
            {
                maximum_signal = InputSignal2.Samples.Count;
            }
            for (int i = 0; i < maximum_signal; i++)
            {
                output = InputSignal1.Samples[i] - InputSignal2.Samples[i];
                result.Add(output);
            }
            OutputSignal = new Signal(result, false);
        }
    }
}