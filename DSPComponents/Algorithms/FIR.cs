using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {

        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        List<float> getWindowfun(int type_no, int N)
        {
            List<float> wn;
            if (type_no == 1)
            {
                wn = rectangular(N);

            }
            else if (type_no == 2)
            {
                wn = himming(N);
            }
            else if (type_no == 3)
            {
                wn = hamming(N);
            }
            else
            {
                wn = Blackman(N);
            }
            return wn;
        }

        float high_pass(float n, float fc)
        {

            if (n == 0)
            {

                return (float)(1 - (2 * fc));
            }
            else
            {
                return (float)((-2 * fc * Math.Sin(n * 2 * fc * Math.PI)) / (n * 2 * fc * Math.PI));

            }
        }
        float low_pass(int n, float fc)
        {


            if (n == 0)
            {

                return 2 * fc;
            }
            else
            {

                return (float)((2 * fc * (float)Math.Sin(n * 2 * fc * Math.PI)) / (n * 2 * fc * Math.PI));

            }
        }
        float band_pass(int n, float f1, float f2)
        {
            if (n == 0)
            {
                return (float)2.0 * (f2 - f1);
            }
            else
            {
                return (float)(((2.0 * f2 * Math.Sin(n * 2.0 * f2 * Math.PI)) / (n * 2.0 * f2 * Math.PI)) - ((2.0 * f1 * Math.Sin(n * 2.0 * f1 * Math.PI)) / (n * 2.0 * f1 * Math.PI)));

            }
        }

        float band_stop(int n, float f1, float f2)
        {
            if (n == 0)
            {
                return (float)(1.0 - (2.0 * (f2 - f1)));
            }
            else
            {
                return (float)(((2.0 * f1 * Math.Sin(n * 2.0 * f1 * Math.PI)) / (n * 2.0 * f1 * Math.PI)) - ((2.0 * f2 * Math.Sin(n * 2.0 * f2 * Math.PI)) / (n * 2.0 * f2 * Math.PI)));

            }

        }

        List<float> Blackman(int N)
        {
            List<float> result = new List<float>();
            for (int n = 0; n <= N / 2; n++)
            {
                result.Add((float)(0.42 + 0.5 * Math.Cos((2 * Math.PI * n) / (N - 1)) + (0.08 * (Math.Cos((4 * Math.PI * n) / (N - 1))))));

            }
            return result;
        }

        List<float> hamming(int N)
        {

            List<float> result = new List<float>();
            for (int n = 0; n <= N / 2; n++)
            {


                result.Add((float)(0.54 + 0.46 * Math.Cos((2 * Math.PI * n) / N)));
            }
            return result;
        }

        List<float> himming(int N)
        {
            List<float> result = new List<float>();
            for (int n = 0; n <= N / 2; n++)
            {
                result.Add((float)(0.5 + 0.5 * Math.Cos((2 * Math.PI * n) / N)));
            }
            return result;
        }

        List<float> rectangular(int N)
        {
            List<float> result = new List<float>();
            for (int n = 0; n <= N / 2; n++)
            {
                result.Add((float)1);
            }
            return result;
        }

        double GetN(double df, float InputStopBandAttenuation, ref int type_no)
        {



            if (InputStopBandAttenuation <= 21)
            {
                type_no = 1;
                return Math.Ceiling((0.9 / df)) % 2 == 0 ? Math.Ceiling((0.9 / df)) + 1 : Math.Ceiling((0.9 / df));
            }
            else if (InputStopBandAttenuation <= 44)
            {
                type_no = 2;
                return Math.Ceiling(3.1 / df) % 2 == 0 ? Math.Ceiling(3.1 / df) + 1 : Math.Ceiling(3.1 / df);

            }
            else if (InputStopBandAttenuation <= 53)
            {
                type_no = 3;
                return Math.Ceiling(3.3 / df) % 2 == 0 ? Math.Ceiling(3.3 / df) + 1 : Math.Ceiling(3.3 / df);

            }
            else
            {
                type_no = 4;
                return Math.Ceiling(5.5 / df) % 2 == 0 ? Math.Ceiling(5.5 / df) + 1 : Math.Ceiling(5.5 / df);

            }


        }
   

        public override void Run()
        {
            OutputHn = new Signal(new List<float>(), new List<int>(), InputTimeDomainSignal.Periodic, new List<float>(), new List<float>(), new List<float>());
            float df = InputTransitionBand / InputFS;
            int type_no = 0;

            int N = (int)GetN(df, InputStopBandAttenuation, ref type_no);
            List<float> wn = getWindowfun(type_no, N);

            int size = N / 2;
            DirectConvolution dc = new DirectConvolution();


            if (InputFilterType == FILTER_TYPES.HIGH)
            {
                InputCutOffFrequency = (float)(InputCutOffFrequency - InputTransitionBand / 2.0);
                InputCutOffFrequency /= InputFS;

                int x = size * -1;
                for (int i = size; i >= 0; i--)
                {
                    float hn = (high_pass((float)i, (float)InputCutOffFrequency) * wn[i]);
                    OutputHn.Samples.Add(hn);
                    OutputHn.SamplesIndices.Add(x);
                    x++;

                }
                int j = size - 1;
                while (j != -1)
                {
                    OutputHn.Samples.Add(OutputHn.Samples[j]);
                    OutputHn.SamplesIndices.Add(x);
                    j--;
                    x++;
                }





            }
            else if (InputFilterType == FILTER_TYPES.LOW)
            {

                InputCutOffFrequency = (float)(InputCutOffFrequency + InputTransitionBand / 2.0);
                InputCutOffFrequency /= InputFS;

                int x = size * -1;
                for (int i = size; i >= 0; i--)
                {
                    float hn = (low_pass(i, (float)InputCutOffFrequency) * wn[i]);
                    OutputHn.Samples.Add(hn);
                    OutputHn.SamplesIndices.Add(x);
                    x++;

                }
                int j = size - 1;
                while (j != -1)
                {
                    OutputHn.Samples.Add(OutputHn.Samples[j]);
                    OutputHn.SamplesIndices.Add(x);
                    j--;
                    x++;
                }

            }
            else if (InputFilterType == FILTER_TYPES.BAND_STOP)
            {
                InputF1 = (float)(InputF1 + InputTransitionBand / 2.0);
                InputF2 = (float)(InputF2 - InputTransitionBand / 2.0);
                InputF1 /= InputFS;
                InputF2 /= InputFS;

                int x = size * -1;
                for (int i = size; i >= 0; i--)
                {
                    float hn = band_stop(i, (float)InputF1, (float)InputF2) * wn[i];
                    OutputHn.Samples.Add(hn);
                    OutputHn.SamplesIndices.Add(x);
                    x++;

                }
                int j = size - 1;
                while (j != -1)
                {
                    OutputHn.Samples.Add(OutputHn.Samples[j]);
                    OutputHn.SamplesIndices.Add(x);
                    j--;
                    x++;
                }
            }
            else
            {
                InputF1 = (float)(InputF1 - InputTransitionBand / 2.0);
                InputF2 = (float)(InputF2 + InputTransitionBand / 2.0);
                InputF1 /= InputFS;
                InputF2 /= InputFS;

                int x = size * -1;
                for (int i = size; i >= 0; i--)
                {
                    float hn = band_pass(i, (float)InputF1, (float)InputF2) * wn[i];
                    OutputHn.Samples.Add(hn);
                    OutputHn.SamplesIndices.Add(x);
                    x++;

                }
                int j = size - 1;
                while (j != -1)
                {
                    OutputHn.Samples.Add(OutputHn.Samples[j]);
                    OutputHn.SamplesIndices.Add(x);
                    j--;
                    x++;
                }

            }
            dc.InputSignal1 = OutputHn;
            dc.InputSignal2 = InputTimeDomainSignal;

            dc.Run();
            OutputYn = dc.OutputConvolvedSignal;

        }
    }
}
