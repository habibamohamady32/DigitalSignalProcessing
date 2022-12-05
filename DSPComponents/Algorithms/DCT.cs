using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> output = new List<float>();
            //k
            for (int i = 0; i < InputSignal.Samples.Count(); i++)
            {
                float sum = 0;
                //n
                for (int j = 0; j < InputSignal.Samples.Count(); j++)
                {
                    float result = (float)(InputSignal.Samples[j] * Math.Cos((Math.PI / (4 * InputSignal.Samples.Count())) * (2 * j - 1) * (2 * i - 1)));
                    sum += result;
                }

                double factor = Math.Pow(2d / InputSignal.Samples.Count(), 0.5);
                sum *= (float)factor;
                output.Add(sum);
            }
            OutputSignal = new Signal(output, false);
        }
    }
}
