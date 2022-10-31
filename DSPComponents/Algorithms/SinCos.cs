using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class SinCos: Algorithm
    {
        public string type { get; set; }
        public float A { get; set; }
        public float PhaseShift { get; set; }
        public float AnalogFrequency { get; set; }
        public float SamplingFrequency { get; set; }
        public List<float> samples { get; set; }
        public override void Run()
        {   

            float sincos ;
            List<float> Sampels = new List<float>();
            
            if (type =="sin")
            {
                for (int i = 0; i < SamplingFrequency; i++) {
                    sincos = (float)(A * (Math.Sin(2 * Math.PI * (AnalogFrequency / SamplingFrequency)*i+ PhaseShift)));
                    Sampels.Add(sincos);    
                }
                samples = new List<float>(Sampels);
            }
            else if (type == "cos")
            {
                for (int i = 0; i < SamplingFrequency; i++)
                {
                    sincos = (float)(A * (Math.Cos(2 * Math.PI * (AnalogFrequency / SamplingFrequency) * i + PhaseShift)));
                    Sampels.Add(sincos);
                }
                samples = new List<float>(Sampels);

            }
        }
    }
}
