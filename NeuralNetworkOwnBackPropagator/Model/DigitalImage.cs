using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkOwnBackPropagator
{
    public class DigitalImage
    {
        public byte label;
        public byte[,] pixels;

        int m;
        int n;

        public DigitalImage(byte label, byte[,] pixels)
        {
            this.label = label;
            this.pixels = pixels;

            m = pixels.GetLength(1);
            n = pixels.GetLength(0);

        }

        public byte this[int i,int j] => pixels[i,j];
        public byte this[int i] => pixels[i%m, i/m];

        public override string ToString()
        {
            string retString = label + ":\n";
            for (int i = 0; i < m; i++) {
                retString += "\n";
                for (int j = 0; j < n; j++)
                    retString += String.Format("{0,3}",this[i, j]);
            }

            return retString;
        }

    }
}
