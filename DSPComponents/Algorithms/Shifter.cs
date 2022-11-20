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
            List<int> outsignalIndex = new List<int>();
            if (folder.OutputFoldedSignal.Periodic == false)
            {
                
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    InputSignal.SamplesIndices[i]-=ShiftingValue ;
                }
            }
            if (folder.OutputFoldedSignal.Periodic == true)
            {

                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    InputSignal.SamplesIndices[i] += ShiftingValue;
                }
            }
            
            OutputShiftedSignal = new Signal(InputSignal.Samples, outsignalIndex, false);
        }
    }
}
