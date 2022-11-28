using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            //1-calc correlation
            DirectCorrelation delay = new DirectCorrelation();
            delay.InputSignal1 = InputSignal1;
            delay.InputSignal2 = InputSignal2;
            delay.Run();

            //2-find max absulyte value 
            float maxInd = 0;
            for (int i = 0; i < (delay.OutputNonNormalizedCorrelation).Count; i++)
            {
                if (maxInd < Math.Abs(delay.OutputNonNormalizedCorrelation[i]))
                {
                    //3- lag(j)
                    maxInd = i;
                }
            }

            //4-time delay = j *Ts
            OutputTimeDelay = maxInd * InputSamplingPeriod;
        }
    }
}
