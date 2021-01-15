using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//class for the package input of the algorithm
namespace _2dga_v3_wpf
{
    public class Package
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public Package(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public String toString()
        {
            return x + "," + y;
        }
    }
}

