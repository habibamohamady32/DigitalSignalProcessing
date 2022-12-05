using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }
        List<Complex> comp1, comp2;
        List<Complex> complexElements;
        Complex sum;
        float theta;
        int counter;
        public override void Run()
        {
            if (InputSignal2 == null)
                InputSignal2 = new Signal(InputSignal1.Samples, false);
            comp1 = DFT(InputSignal1, true);
            comp2 = DFT(InputSignal2, false);
            complexElements = multiplication(comp1, comp2);
            IDFT();
        }

        public List<Complex> DFT(Signal s, bool minus)
        {
            DiscreteFourierTransform obj = new DiscreteFourierTransform(); ;
            obj.InputTimeDomainSignal = s;
            obj.Run();
            s = obj.OutputFreqDomainSignal;
            List<Complex> complices = new List<Complex>();
            float real, imagine;
            for (int i = 0; i < s.FrequenciesAmplitudes.Count; i++)
            {
                real = (float)(s.FrequenciesAmplitudes[i] * (Math.Cos(s.FrequenciesPhaseShifts[i])));
                imagine = (float)(s.FrequenciesAmplitudes[i] * (Math.Sin(s.FrequenciesPhaseShifts[i])));
                if (minus)
                    complices.Add(new Complex(real, (-1 * imagine)));
                else
                    complices.Add(new Complex(real, imagine));
            }
            return complices;
        }

        public List<Complex> multiplication(List<Complex> c1, List<Complex> c2)
        {
            List<Complex> results = new List<Complex>();
            for (int i = 0; i < c1.Count; i++)
                results.Add(Complex.Multiply(c1[i], c2[i]));
            return results;
        }

        public void IDFT()
        {
            counter = InputSignal1.Samples.Count;
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            double norm, x = 0, y = 0;
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                x += Math.Pow(InputSignal1.Samples[i], 2);
                y += Math.Pow(InputSignal2.Samples[i], 2);
            }
            norm = Math.Sqrt(x * y) / counter;
            for (int n = 0; n < counter; n++)
            {
                sum = 0;
                for (int k = 0; k < counter; k++)
                {
                    theta = (float)(2 * Math.PI * k * n) / counter;
                    sum += complexElements[k] * (Math.Cos(theta) + (Complex.ImaginaryOne * Math.Sin(theta)));
                }
                sum /= counter;
                OutputNonNormalizedCorrelation.Add((float)(sum.Real / counter));
                OutputNormalizedCorrelation.Add((float)((sum.Real / norm) / counter));
            }
        }

    }
}