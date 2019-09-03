using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkOwnBackPropagator
{
    public static class BasicLinearAlgebra
    {
        /*
        public static Matrix<double> MatrixProd(Matrix<double> m1, Matrix<double> m2)
        {
            if (m1.columns != m2.rows)
            {
                Console.WriteLine("non-mathing lengths");
                return new Matrix<double>(1,1);
            }

            Matrix<Double> m = new Matrix<double>(m1.rows, m2.columns);
            for (int r = 0; r < m.rows; r++)
                for (int c = 0; c < m.columns; c++)
                    for (int i = 0; i < m1.columns; i++)
                        m[r, c] += m1[r, i] * m2[i, c];

            return m;
        }
        */


        public static double VectorProd(double[] v1, double[] v2)
        {
            if (v1.Length != v2.Length)
            {
                Console.WriteLine("non-mathing vector lengths");
                return -1d;
            }

            double z = 0;

            for (int i = 0; i < v1.Length; i++)
                z += v1[i] * v2[i];

            return z;
        }

    }
}
