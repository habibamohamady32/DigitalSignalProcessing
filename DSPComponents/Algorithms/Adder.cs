using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float adderout = 0;
            List<float> sampels = new List<float>();
            for (int i = 0; i < InputSignals[1].Samples.Count; i++)
            {
                adderout = InputSignals[1].Samples[i] + InputSignals[0].Samples[i];
                sampels.Add(adderout);


            }
            OutputSignal = new Signal(sampels, false);

        }
    }
}