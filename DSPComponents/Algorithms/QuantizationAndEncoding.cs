using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();
            List<float> result = new List<float>();
            List<float> midpoint = new List<float>();
            float max = InputSignal.Samples.Max();
            float min = InputSignal.Samples.Min();
            List<float> samples = new List<float>();


            if (InputNumBits == 0)
            {
                InputNumBits = (int)Math.Log(InputLevel, 2);
            }
            if (InputLevel == 0)
            {
                InputLevel = (int)Math.Pow(2, InputNumBits);
            }
           

            float Delta = (max - min) / InputLevel;
    
            result.Add(min);
            for (int i = 1; i <= InputLevel; i++)
            {
                result.Add(result[i - 1] + Delta);
            }
            for (int i = 0; i < InputLevel; i++)
            {
                midpoint.Add((result[i] + result[i + 1]) / 2);
            }
            
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = 0; j < InputLevel; j++)
                {
                    if (InputSignal.Samples[i] >= result[j] && InputSignal.Samples[i] <= result[j + 1] + 0.0001)
                    {
                        samples.Add(midpoint[j]);
                        OutputEncodedSignal.Add(Convert.ToString(j, 2).PadLeft(InputNumBits, '0'));
                        OutputIntervalIndices.Add(j + 1);
                        break;

                    }
                }
            }
            OutputQuantizedSignal = new Signal(samples, false);

            List<float> samplerror = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                samplerror.Add(OutputQuantizedSignal.Samples[i] - InputSignal.Samples[i]);
            }
            OutputSamplesError = samplerror;
        }
    }
}
