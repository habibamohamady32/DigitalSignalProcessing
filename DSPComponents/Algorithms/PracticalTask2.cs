using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms

{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////
        public override void Run()
        {
            Signal signal = new Signal(new List<float>(), new List<int>(), false);
            Signal InputSignal = LoadSignal(SignalPath);

            //FIR        
            FIR firObj = new FIR();
            firObj.InputTimeDomainSignal = InputSignal;
            firObj.InputFilterType = FILTER_TYPES.BAND_PASS;
            firObj.InputFS = Fs;
            firObj.InputF1 = miniF;
            firObj.InputF2 = maxF;
            firObj.InputStopBandAttenuation = 50;
            firObj.InputTransitionBand = 500;
            firObj.Run();
            signal = firObj.OutputYn;
            using (StreamWriter w = new StreamWriter("D:/filessss/firpractice2.ds"))
            {
                w.WriteLine("0");
                w.WriteLine("0");
                w.WriteLine(firObj.OutputYn.Samples.Count().ToString());
                for (int i = 0; i < firObj.OutputYn.Samples.Count(); i++)
                {

                    w.WriteLine(firObj.OutputYn.SamplesIndices[i].ToString() + " " + firObj.OutputYn.Samples[i].ToString());

                }
            }
            //SAMPLING
            if (newFs >= 2 * maxF)
            {
                Sampling samplingObj = new Sampling();
                samplingObj.InputSignal = firObj.OutputYn;
                samplingObj.M = M;
                samplingObj.L = L;
                samplingObj.Run();
                //signal = samplingObj.OutputSignal;
                //File.WriteAllLines("../../../Signal Files/Sampling.ds", signal.Samples.Select(element => element.ToString()));
                using (StreamWriter w = new StreamWriter("D:/filessss/LMSamplesPractice2.ds"))
                {
                    w.WriteLine("0");
                    w.WriteLine("0");
                    w.WriteLine(samplingObj.OutputSignal.Samples.Count().ToString());
                    for (int i = 0; i < samplingObj.OutputSignal.Samples.Count(); i++)
                    {

                        w.WriteLine(samplingObj.OutputSignal.SamplesIndices[i].ToString() + " " + samplingObj.OutputSignal.Samples[i].ToString());

                    }
                }
                signal = samplingObj.OutputSignal;

            }
            else
                Console.WriteLine("newFs is not valid !..");

            //REMOVE DC_COMPONENT
            DC_Component dcObj = new DC_Component();
            dcObj.InputSignal = signal;
            dcObj.Run();
            using (StreamWriter w = new StreamWriter("D:/filessss/DCcomponentpractical2.ds"))
            {
                w.WriteLine("0");
                w.WriteLine("0");
                w.WriteLine(dcObj.OutputSignal.Samples.Count().ToString());
                for (int i = 0; i < dcObj.OutputSignal.Samples.Count(); i++)
                {

                    w.WriteLine(dcObj.OutputSignal.SamplesIndices[i].ToString() + " " + dcObj.OutputSignal.Samples[i].ToString());

                }
            }
            //NORMALIZE
            Normalizer normalizerObj = new Normalizer();
            normalizerObj.InputSignal = dcObj.OutputSignal;
            normalizerObj.InputMinRange = -1;
            normalizerObj.InputMaxRange = 1;
            normalizerObj.Run();
            using (StreamWriter w = new StreamWriter("D:/filessss/normalizerpractical2.ds"))
            {
                w.WriteLine("0");
                w.WriteLine("0");
                w.WriteLine(normalizerObj.OutputNormalizedSignal.Samples.Count().ToString());
                for (int i = 0; i < normalizerObj.OutputNormalizedSignal.Samples.Count(); i++)
                {

                    w.WriteLine(normalizerObj.OutputNormalizedSignal.SamplesIndices[i].ToString() + " " + normalizerObj.OutputNormalizedSignal.Samples[i].ToString());

                }
            }
            //DFT
            DiscreteFourierTransform dftObj = new DiscreteFourierTransform();
            dftObj.InputTimeDomainSignal = normalizerObj.OutputNormalizedSignal;
            dftObj.InputSamplingFrequency = Fs;
            dftObj.Run();
            for (int i = 0; i < dftObj.OutputFreqDomainSignal.Frequencies.Count; i++)
                dftObj.OutputFreqDomainSignal.Frequencies[i] = (float)Math.Round((double)dftObj.OutputFreqDomainSignal.Frequencies[i], 1);
            OutputFreqDomainSignal = dftObj.OutputFreqDomainSignal;
            using (StreamWriter w = new StreamWriter("D:/filessss/DFTpractical2.ds"))
            {
                w.WriteLine("1");
                w.WriteLine("0");
                w.WriteLine(dftObj.OutputFreqDomainSignal.Frequencies.Count().ToString());
                for (int i = 0; i < dftObj.OutputFreqDomainSignal.Frequencies.Count(); i++)
                {

                    w.WriteLine(dftObj.OutputFreqDomainSignal.Frequencies[i].ToString() + " " + dftObj.OutputFreqDomainSignal.FrequenciesAmplitudes[i].ToString() + " " + dftObj.OutputFreqDomainSignal.FrequenciesPhaseShifts[i].ToString());

                }
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////



        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}