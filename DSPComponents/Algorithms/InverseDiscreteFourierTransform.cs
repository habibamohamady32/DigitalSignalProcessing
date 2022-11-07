using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            List<Complex> complex = new List<Complex>();
            List<Complex> result = new List<Complex>();
            int N = InputFreqDomainSignal.FrequenciesAmplitudes.Count;

            for (int n = 0; n < N; n++)
            {
                result.Add(0);
                for (int k = 0; k < N; k++)
                {
                    complex.Add(Complex.FromPolarCoordinates(InputFreqDomainSignal.FrequenciesAmplitudes[k], InputFreqDomainSignal.FrequenciesPhaseShifts[k]));
                    double epower = (2 * k * Math.PI * n) / N;
                    result[n] += complex[k] * (Math.Cos(epower) + Complex.ImaginaryOne * Math.Sin(epower));
                }
                result[n] /= N;
            }

            List<float> original_samples = new List<float>();

            for (int i = 0; i < N; i++)
            {
                original_samples.Add((float)result[i].Real);
            }
            OutputTimeDomainSignal = new Signal(original_samples, false);

        }
    }
}
