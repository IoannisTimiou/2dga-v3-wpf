using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Genetic;

namespace _2dga_v3_wpf
{
    public class _2DFitness : IFitnessFunction
    {
        public _2DFitness() { }
        public double Evaluate(IChromosome chromosome)
        {
            double score = 1;
            _2DChromosome c = (_2DChromosome)chromosome;
            c.totaloverlap = 0; 
            c.overlaps = 0;


            for (int i = 0; i < c.positions.Count; i++)
            {
                int j = 1;
                while (i + j < c.positions.Count)
                {
                    if (c.positions[i].contnum == c.positions[i + j].contnum)
                    {
                        if (OverlapCheck(c.items[i], c.items[i + j], c.positions[i], c.positions[i + j]))
                        {
                            c.overlaps++;
                            c.totaloverlap = c.totaloverlap + OverlapSize(c.items[i], c.items[i + j], c.positions[i], c.positions[i + j]);
                        }
                    }
                    j++;
                }
            }

            if (c.overlaps > 0)
            {
                double penalty = 1 / (Convert.ToDouble(c.totaloverlap)/500 + Convert.ToDouble(c.overlaps) * 50 + Convert.ToDouble(c.outofbounds) * 1000);

                score = 1 / (1 + Math.Pow(Math.E, (-penalty + 0.5)));
            }
            return score;
        }
        public bool OverlapCheck(Package p1, Package p2, Coordinates c1, Coordinates c2)
        {
            if (c1.topleftx >= c2.topleftx + p2.x || c2.topleftx >= c1.topleftx + p1.x)
            {
                return false;
            }
            if (c1.toplefty >= c2.toplefty + p2.y || c2.toplefty >= c1.toplefty + p1.y)
            {
                return false;
            }
            return true;
        }

        public int OverlapSize(Package p1, Package p2, Coordinates c1, Coordinates c2)
        {
            return ((Math.Min(c1.topleftx + p1.x, c2.topleftx + p2.x) - Math.Max(c1.topleftx, c2.topleftx)) 
                          * (Math.Min(c1.toplefty + p1.y, c2.toplefty + p2.y) - Math.Max(c1.toplefty, c2.toplefty)));
        }
    }
}
