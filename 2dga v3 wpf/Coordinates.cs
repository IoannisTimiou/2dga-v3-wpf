using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2dga_v3_wpf
{
    public class Coordinates
    {
        public int topleftx;
        public int toplefty;
        public int contnum;
        public bool rotation;

        public Coordinates(int cont, int tlx, int tly)
        {
            contnum = cont;
            topleftx = tlx;
            toplefty = tly;
        }

        public String toString()
        {
            return contnum + ", " + topleftx + ", " + toplefty;
        }
    }
}
