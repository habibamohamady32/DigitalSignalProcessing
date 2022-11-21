using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }
        Folder folder = new Folder();
        public override void Run()
        {
            OutputShiftedSignal = new Signal(new List<float>(), false);
            for (int i = 0; i < InputSignal.SamplesIndices.Count; i++)
            {
                if (!InputSignal.Periodic)
                    InputSignal.SamplesIndices[i] -= ShiftingValue;
                else
                {
                    InputSignal.SamplesIndices[i] += ShiftingValue;
                    OutputShiftedSignal.Periodic = true;
                }
                OutputShiftedSignal.Samples.Add(InputSignal.Samples[i]);
                OutputShiftedSignal.SamplesIndices.Add(InputSignal.SamplesIndices[i]);
            }
        }
    }
}
