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
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            List<float> copy1 = new List<float>();
            List<float> copy2 = new List<float>();
            int N = InputSignal1.Samples.Count;

            for (int i = 0; i < N; i++)
                copy1.Add(InputSignal1.Samples[i]);
            int ishift = 0;
            //auto-correlation
            if (InputSignal2 == null)
            {
                double normal1 = 0, one = 0, two = 0;
                for (int i = 0; i < N; i++)
                {
                    // signal_samples_summation += signal1_samples[i] * signal1_samples[i];
                    // signal_samples_copy_summation += signal1_samples_copy[i] * signal1_samples_copy[i];
                    one += Math.Pow(copy1[i], 2);
                    two += Math.Pow(copy1[i], 2);

                }
                normal1 = Math.Sqrt(one * two) / N;

                if (InputSignal1.Periodic == true)
                {
                    for (int i = 0; i < N; i++)
                    {
                        double s = 0;
                        ishift = i;
                        for (int j = 0; j < N; j++)
                        {
                            s += copy1[ishift++] * copy1[j];
                            ishift %= N;
                        }
                        float tmp = (float)s / N;
                        OutputNonNormalizedCorrelation.Add(tmp);
                        OutputNormalizedCorrelation.Add((float)(tmp / normal1));
                    }
                }
                else
                {
                    for (int i = 0; i < N; i++)
                    {
                        double s = 0;
                        if (i != 0)
                        {
                            for (int j = 0; j < N - 1; j++)
                            {
                                copy1[j] = copy1[j + 1];
                                s += copy1[j] * InputSignal1.Samples[j];
                            }
                            copy1[N - 1] = 0;
                        }
                        else
                        {
                            for (int j = 0; j < N; j++)
                            {
                                s += InputSignal1.Samples[j] * InputSignal1.Samples[j];
                            }
                        }
                        float tmp = (float)s / N;
                        OutputNonNormalizedCorrelation.Add(tmp);
                        OutputNormalizedCorrelation.Add((float)(tmp / normal1));
                    }
                }
            }

            else// cross-correlation
            {
                OutputNonNormalizedCorrelation = new List<float>();
                OutputNormalizedCorrelation = new List<float>();
                int steps;
                double x = 0, y = 0, norm, corr;
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    x += Math.Pow(InputSignal1.Samples[i], 2);
                    y += Math.Pow(InputSignal2.Samples[i], 2);
                }
                norm = Math.Sqrt(x * y) / InputSignal1.Samples.Count;
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    corr = 0;
                    steps = -1;
                    for (int j = 0; j < InputSignal1.Samples.Count; j++)
                    {
                        if (j + i <= InputSignal1.Samples.Count - 1)
                            corr += InputSignal1.Samples[j] * InputSignal2.Samples[j + i];
                        else if (InputSignal1.Periodic == true)
                        {
                            steps++;
                            corr += InputSignal1.Samples[j] * InputSignal2.Samples[steps];
                        }
                        else if (InputSignal1.Periodic == false)
                            corr += InputSignal1.Samples[j] * 0;
                    }
                    OutputNonNormalizedCorrelation.Add((float)corr / InputSignal1.Samples.Count);
                    OutputNormalizedCorrelation.Add((float)((corr / InputSignal1.Samples.Count) / norm));
                }
            }
            
        }
    }
}