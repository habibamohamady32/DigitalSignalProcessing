using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; } 
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            //for sharpening signal
            //Y(n) = x(n)-x(n-1)
            FirstDerivative = new Signal(new List<float>(), false);
            //Y(n)= x(n+1)-2x(n)+x(n-1)
            SecondDerivative = new Signal(new List<float>(), false);

            for (int n = 0; n < (InputSignal.Samples.Count - 1); n++)
            {
               
                if (n == 0) //first iteration (no [n-1])
                {
                    FirstDerivative.Samples.Add(InputSignal.Samples[n] - 0);
                    SecondDerivative.Samples.Add(InputSignal.Samples[n + 1] - (2 * InputSignal.Samples[n]) + 0);
                }
                
                else if (n == (InputSignal.Samples.Count - 1)) //last iteration (no [n+1])
                {
                    FirstDerivative.Samples.Add(InputSignal.Samples[n] - InputSignal.Samples[n - 1]);
                    SecondDerivative.Samples.Add(0 - (2 * InputSignal.Samples[n]) + InputSignal.Samples[n - 1]);
                }
                else
                {
                    FirstDerivative.Samples.Add(InputSignal.Samples[n] - InputSignal.Samples[n - 1]);
                    SecondDerivative.Samples.Add(InputSignal.Samples[n + 1] - (2 * InputSignal.Samples[n]) + InputSignal.Samples[n - 1]);
                }

            }
            
        }
    }
}
