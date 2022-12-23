using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }
        public List<Complex> harmonis = new List<Complex>();
        List<float> amb = new List<float>();
        List<float> ph_shift = new List<float>();
        List<float> frq = new List<float>();
        Complex k, harmonic;
        public override void Run()
        {
            k = new Complex();
            harmonic = new Complex();
            int itr = InputTimeDomainSignal.Samples.Count;
            float T = 1 / InputSamplingFrequency;
            for (int i = 0; i < itr; i++)
            {
                k = 0;
                harmonic = 0;
                for (int y = 0; y < itr; y++)
                {
                    k += InputTimeDomainSignal.Samples[y] *
                        Complex.Pow(Math.E, -2 * i * y * Math.PI * Complex.ImaginaryOne / itr);
                    Complex term;
                    float temp = InputTimeDomainSignal.Samples[y];
                    float theta = (float)(2 * Math.PI * i * y / itr);
                    float Real = temp * (float)Math.Cos(theta);
                    float imaginary = temp * (float)Math.Sin(theta);
                    term = new Complex(Real, -imaginary);
                    harmonic += term;
                }
                amb.Add((float)k.Magnitude);
                ph_shift.Add((float)k.Phase);
                harmonis.Add(harmonic);
                frq.Add((float)Math.Round(i * (4 * Math.PI / (itr * T)), 1));
            }
            OutputFreqDomainSignal = new Signal(new List<float>(), false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = amb;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = ph_shift;
            OutputFreqDomainSignal.Frequencies = frq;
        }
    }
}
