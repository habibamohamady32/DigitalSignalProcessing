﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            OutputFoldedSignal = new Signal(new List<float>(), false);
            for (int i = InputSignal.Samples.Count - 1; i >= 0; i--)
            {
                OutputFoldedSignal.Samples.Add(InputSignal.Samples[i]);
                OutputFoldedSignal.SamplesIndices.Add(-InputSignal.SamplesIndices[i]);
            }
            OutputFoldedSignal.Periodic = !InputSignal.Periodic;
        }
    }
}
