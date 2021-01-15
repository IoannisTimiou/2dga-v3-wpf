using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Genetic;

namespace _2dga_v3_wpf
{
    public class _2DChromosome : ChromosomeBase
    {
        public List<Container> containers;
        public List<Package> items;
        Random random;
        public List<Coordinates> positions = new List<Coordinates>();
        public int outofbounds = 0;
        public int overlaps = 0;
        public int totaloverlap = 0;

        public _2DChromosome(List<Container> cont, List<Package> pack, Random r1)
        {
            this.containers = cont;
            this.items = pack;
            this.random = r1;
            Generate();
        }

        //constructor for testing
        public _2DChromosome(List<Container> cont, List<Package> pack)
        {
            this.containers = cont;
            this.items = pack;
        }

        //item placing for testing
        public void itemplace(Package item, Coordinates place)
        {
            this.positions.Add(place);
        }

        private _2DChromosome(_2DChromosome source)
        {
            this.positions = source.positions;
            this.containers = source.containers;
            this.items = source.items;
            this.outofbounds = source.outofbounds;
            this.random = source.random;
            this.overlaps = source.overlaps; 
            this.totaloverlap = source.overlaps; 
        }

        public override void Generate()
        {
            for (int i = 0; i < items.Count; i++)
            {
                int C = random.Next(0, containers.Count);
                int x; int y;
                if (items[i].x <= containers[C].x && items[i].y <= containers[C].y)
                {
                    x = random.Next(containers[C].x - items[i].x);
                    y = random.Next(containers[C].y - items[i].y);
                }
                else
                {
                    x = 0;
                    y = 0;
                    outofbounds++;
                }
                Coordinates coordinates = new Coordinates(C, x, y);
                positions.Add(coordinates);
            }
        }

        public override IChromosome CreateNew()
        {
            return new _2DChromosome(containers, items, random);
        }

        public override IChromosome Clone()
        {
            return new _2DChromosome(this);
        }

        public override void Mutate()
        {
            int mut = random.Next(positions.Count);
            int itemx = items[mut].x;
            int itemy = items[mut].y;
            if (itemx > containers[positions[mut].contnum].x || itemy > containers[positions[mut].contnum].y)
            {
                outofbounds--;
            }
            int C;
            if (random.NextDouble() > 0.7)
            {
                C = random.Next(0, containers.Count);
            }
            else
            {
                C = positions[mut].contnum;
            }

            int x; int y;
            if (items[mut].x <= containers[C].x && items[mut].y <= containers[C].y)
            {
                x = random.Next(containers[C].x - itemx);
                y = random.Next(containers[C].y - itemy);
            }
            else
            {
                x = 0;
                y = 0;
                outofbounds++;
            }
            Coordinates coordinates = new Coordinates(C, x, y);
            positions.RemoveAt(mut);
            positions.Insert(mut, coordinates);
        }

        public override void Crossover(IChromosome pair)
        {
            _2DChromosome parent1 = (_2DChromosome)this.Clone();
            _2DChromosome parent2 = (_2DChromosome)pair.Clone();
            if (pair != null)
            {
                CreateChildWithCrossover(this, pair);
            }
        }
        
        private void CreateChildWithCrossover(IChromosome parent1, IChromosome parent2)
        {
            _2DChromosome p1 = (_2DChromosome)parent1;
            _2DChromosome p2 = (_2DChromosome)parent2;
            List<Coordinates> childpositions = new List<Coordinates>();
            int oob = 0;
            for (int i = 0; i < p1.items.Count; i++)
            {
                int con; int x; int y;
                if (random.NextDouble() > 0.5)
                {
                    con = p1.positions[i].contnum;
                    x = p1.positions[i].topleftx;
                    y = p1.positions[i].toplefty;
                }
                else
                {
                    con = p2.positions[i].contnum;
                    x = p2.positions[i].topleftx;
                    y = p2.positions[i].toplefty;
                }
                Coordinates coord = new Coordinates(con, x, y);
                childpositions.Add(coord);
                if ((items[i].x + x) > containers[coord.contnum].x || (items[i].y + y) > containers[coord.contnum].y)
                {
                    oob++;
                }
            }
            p1.outofbounds = oob;
            p1.positions = childpositions;
        }
        //currently redundant
        public override string ToString()
        {
            String s = "";
            s += "";
            s += "\nFitness: " + this.Fitness + ", objects overlapping: " + overlaps + ", total overlap space: " + totaloverlap;
            return s;
        }
    }
}
