using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }
        FIR obj = new FIR();
        int counter;
        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), new List<int>(), false);
            counter = InputSignal.Samples.Count();
            if (M == 0 && L != 0)
            {
                Signal signal = upSampling(InputSignal);
                OutputSignal = lowPassFilter(signal);
            }
            else if (M != 0 && L == 0)
            {
                Signal signal = lowPassFilter(InputSignal);
                OutputSignal = downSampling(signal);
            }
            else if (M != 0 && L != 0)
            {
                Signal signal = upSampling(InputSignal);
                signal = lowPassFilter(signal);
                OutputSignal = downSampling(signal);
            }
            else
                Console.WriteLine("ERROR!..");
        }

        public Signal upSampling(Signal upSignal)
        {
            counter = (counter * L) - (L - 1);
            for (int i = 0; i < counter; i += L)
            {
                if (i == counter - 1)
                    break;
                for (int j = 0; j < L - 1; j++)
                    upSignal.Samples.Insert(i + 1, 0);
            }
            return upSignal;
        }

        public Signal downSampling(Signal filteredSignal)
        {
            Signal downSignal = new Signal(new List<float>(), new List<int>(), false);
            for (int i = 0; i < filteredSignal.Samples.Count; i += M)
                downSignal.Samples.Add(filteredSignal.Samples[i]);
            return downSignal;
        }

        public Signal lowPassFilter(Signal signal)
        {
            obj.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            obj.InputFS = 8000;
            obj.InputStopBandAttenuation = 50;
            obj.InputCutOffFrequency = 1500;
            obj.InputTransitionBand = 500;
            obj.InputTimeDomainSignal = signal;
            obj.Run();
            Signal filteredSignal = new Signal(new List<float>(), new List<int>(), false);
            filteredSignal = obj.OutputYn;
            return filteredSignal;
        }

    }
}