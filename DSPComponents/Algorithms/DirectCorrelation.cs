using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }
        public override void Run()
        {
            if (InputSignal2 == null)
                autoCorrelation();
            else
                crossCorrelation();
        }
        public void autoCorrelation()
        {
            InputSignal2 = new Signal(new List<float>(), false);
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                InputSignal2.Samples.Add(InputSignal1.Samples[i]);
            }
            crossCorrelation();
        }
        public void crossCorrelation()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            int N = InputSignal1.Samples.Count;
            int isshifted;
            double x = 0, y = 0, normlize, corr;
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                x += Math.Pow(InputSignal1.Samples[i], 2);
                y += Math.Pow(InputSignal2.Samples[i], 2);
            }
            normlize = Math.Sqrt(x * y) /N;
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                corr = 0;
                isshifted = -1;
                for (int j = 0; j < InputSignal1.Samples.Count; j++)
                {
                    if (j + i <= InputSignal1.Samples.Count - 1)
                        corr += InputSignal1.Samples[j] * InputSignal2.Samples[j + i];
                    else if (InputSignal1.Periodic == true)
                    {
                        isshifted++;
                        corr += InputSignal1.Samples[j] * InputSignal2.Samples[isshifted];
                    }
                    else if (InputSignal1.Periodic == false)
                        corr += InputSignal1.Samples[j] * 0;
                }
                float nonNormalizedResult = (float)(corr / N);
                OutputNonNormalizedCorrelation.Add((float)nonNormalizedResult );
                OutputNormalizedCorrelation.Add((float)(nonNormalizedResult/normlize));
            }
        }

    }
}