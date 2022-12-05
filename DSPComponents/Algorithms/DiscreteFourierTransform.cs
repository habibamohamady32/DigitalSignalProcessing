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
        public override void Run()
        {
            
            List<float> amb = new List<float>();
            List<float> ph_shift = new List<float>();
            List<float> frq = new List<float>();
            int itr = InputTimeDomainSignal.Samples.Count;
            for(int i = 0; i < itr; i++)
            {
                Complex harmonic = 0;
                for(int y = 0; y < itr; y++)
                {
                    Complex term ;
                    float temp = InputTimeDomainSignal.Samples[y];
                    float theta = (float)((2 * Math.PI * i * y) / itr);
                    float Real = temp*(float)Math.Cos(theta);
                    float imaginary =temp* (float)Math.Sin(theta);
                    term = new Complex(Real, -imaginary);
                    harmonic+=term;
                }
                harmonis.Add(harmonic);
            }
            foreach(var g in harmonis)
            {
                amb.Add((float)g.Magnitude);
                ph_shift.Add((float)g.Phase);
                
            }
            OutputFreqDomainSignal = new Signal(InputTimeDomainSignal.Samples, false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = amb;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = ph_shift;



        //    OutputFreqDomainSignal = new Signal(new List<float>(), false);
        //    int N = InputTimeDomainSignal.Samples.Count;
        //    List<float> Amplitude = new List<float>();
        //    List<float> Phase = new List<float>();
        //    //k constant term in samples
        //    for (int k = 0; k < N; k++)
        //    {
        //        double sum_of_complex = 0, sum_of_real = 0;
        //        //n samples
        //        for (int n = 0; n < N; n++)
        //        {
        //            double ePower = -(n * 2 * k * Math.PI) / N;
        //            sum_of_real += (Math.Cos(ePower) * InputTimeDomainSignal.Samples[n]);
        //            sum_of_complex += (Math.Sin(ePower) * InputTimeDomainSignal.Samples[n]);
        //        }

        //        //amplitude 
        //        Amplitude.Add((float)Math.Sqrt((Math.Pow(sum_of_real, 2) + Math.Pow(sum_of_complex, 2))));

        //        //phase 
        //        Phase.Add((float)Math.Atan2(sum_of_complex, sum_of_real));

        //    }
        //    OutputFreqDomainSignal.FrequenciesAmplitudes = Amplitude;
        //    OutputFreqDomainSignal.FrequenciesPhaseShifts = Phase;

        }
    }
}
