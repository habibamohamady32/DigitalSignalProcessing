using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float sum=0;
            float avg;
            float x;
            List<float> output = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                sum+=InputSignal.Samples[i];
               
            }
            avg = (sum / InputSignal.Samples.Count);
            for (int i = 0; i < InputSignal.Samples.Count; i++) {
                x = InputSignal.Samples[i] - avg;
                output.Add(x);  
                
                       
            }
            OutputSignal =new Signal (output, false);
        }
    }
}
