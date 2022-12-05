using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<Complex> harmonic1;
            List<Complex> harmonic2;
            List<Complex> final_harmonic = new List<Complex>();
            int lenght = InputSignal1.Samples.Count() + InputSignal2.Samples.Count() - 1;
            for (int i = 0; i < lenght; i++)
            {
                if (i >= InputSignal1.Samples.Count())
                {
                    InputSignal1.Samples.Add(0);
                    InputSignal1.SamplesIndices.Add(i);
                }
                if (i >= InputSignal2.Samples.Count())
                {
                    InputSignal2.Samples.Add(0);
                    InputSignal2.SamplesIndices.Add(i);
                }
            }
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            dft.InputTimeDomainSignal = InputSignal1;
            dft.Run();
            harmonic1 = new List<Complex>(dft.harmonis);
            dft.harmonis.Clear();
            dft.InputTimeDomainSignal = InputSignal2;
            dft.Run();
            harmonic2 = new List<Complex>(dft.harmonis);

            for (int x = 0; x < harmonic1.Count; x++)
            {
                Complex temp = harmonic1[x] * harmonic2[x];
                final_harmonic.Add(temp);
            }
            Signal s = new Signal(new List<float>(), false);
            List<float> mag = new List<float>();
            List<float> phase_shift = new List<float>();
            foreach (var g in final_harmonic)
            {
                mag.Add((float)g.Magnitude);
                phase_shift.Add((float)g.Phase);

            }
            s.FrequenciesAmplitudes = mag;
            s.FrequenciesPhaseShifts = phase_shift;
            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            idft.InputFreqDomainSignal = s;
            idft.Run();
            OutputConvolvedSignal = new Signal(new List<float>(), false);
            OutputConvolvedSignal = idft.OutputTimeDomainSignal;
        }
    }
}
