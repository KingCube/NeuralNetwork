using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkOwnBackPropagator
{
    public class Matrix
    {
        private double[][] data;

        public int rows;
        public int columns;
        int elements;
        public bool IsVector
        {
            get
            {
                return rows == 1 || columns == 1;
            }
        }


        public Matrix(int m, int n){
            rows = m;
            columns = n;
            elements = m * n;

            data = new double[m][];

            for (int i = 0; i < m; i++)
                data[i] = new double[n];
        }

        public void ElementWiseFunction(Func<double,double> function)
        {
            for (int i = 0; i < elements; i++)
                this[i] = function(this[i]);
        }

        #region Indexers

        public double this[int i, int j]
        {
            get
            {
                return data[i][j];
            }
            set
            {
                data[i][j] = value;
            }
        }

        public double this[int i]
        {
            get
            {
                return data[i/columns][i% columns];
            }
            set
            {
                data[i/ columns][i% columns] = value;
            }
        }

        #endregion

        public override string ToString()
        {
            string retString = "";
            
            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                    retString += String.Format("{0:0.00}",data[i][j]) + " , ";
                retString += "\n";
            }

            return retString;
        }

        #region Operators

        public static Matrix operator+ (Matrix a, Matrix b)
        {
            if(a.rows != b.rows || a.columns != b.columns)
                throw new System.InvalidOperationException("Matrix addition can only by by matrises with same dimension");

            Matrix M = new Matrix(a.rows, a.columns);

            for (int i = 0; i < a.elements; i++)
                M[i] = a[i] + b[i];
            
            return M;
        }

        public static Matrix operator- (Matrix a, Matrix b)
        {
            if (a.rows != b.rows || a.columns != b.columns)
                throw new System.InvalidOperationException("Matrix addition can only by by matrises with same dimension");

            Matrix M = new Matrix(a.rows, a.columns);

            for (int i = 0; i < a.elements; i++)
                M[i] = a[i] - b[i];

            return M;
        }



        public static Matrix operator*(Matrix A, Matrix B)
        {
            if (A.IsVector && !B.IsVector && A.columns != B.rows)
                throw new System.InvalidOperationException("Matrix dimensions not compatible for product");
            

            Matrix m = new Matrix(A.rows, B.columns);
            for (int r = 0; r < m.rows; r++)
                for (int c = 0; c < m.columns; c++)
                    for (int i = 0; i < A.columns; i++)
                        m[r, c] += A[r, i] * B[i, c];

            return m;
        }

        public static Matrix operator*(int x, Matrix A)
        {
            Matrix m = new Matrix(A.rows, A.columns);
            for (int i = 0; i < m.elements; i++)
                m[i] = x * A[i];

            return m;
        }

        public static Matrix operator *(double x, Matrix A)
        {
            Matrix m = new Matrix(A.rows, A.columns);
            for (int i = 0; i < m.elements; i++)
                m[i] = x * A[i];

            return m;
        }

        #endregion
    }
}
