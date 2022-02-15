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
        public int elements;
        bool isTransposed = false;
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

        public void Transpose()
        {
            int temp = rows;
            rows = columns;
            columns = temp;
            isTransposed = !isTransposed;
        }

        #region Indexers

        public double this[int i, int j]
        {
            get
            {
                if(!isTransposed)
                    return data[i][j];
                else
                    return data[j][i];
            }
            set
            {
                if (!isTransposed)
                    data[i][j] = value;
                else
                    data[j][i] = value;
            }
        }

        public double this[int i]
        {
            get
            {
                if (!isTransposed)
                    return data[i / columns][i % columns];
                else
                    return data[i / rows][i % rows];
            }
            set
            {
                if (!isTransposed)
                    data[i / columns][i % columns] = value;
                else
                    data[i / rows][i % rows] = value;
            }
        }

        #endregion

        public Matrix Clone()
        {
            Matrix m = new Matrix(rows, columns);
            for (int i = 0; i < elements; i++)
                m[i] = this[i];

            return m;
        }

        public override string ToString()
        {
            string retString = "";
            
            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                    //retString += data[i][j] + " , ";
                    retString += String.Format("{0:0.00}",data[i][j]) + " , ";
                retString += "\n";
            }

            return retString;
        }


        #region selfmodifications
        public void Fill(double x)
        {
            for (int i = 0; i < elements; i++)
                this[i] = x;
        }

        public void Add(Matrix A, double k = 1)
        {
            if (A.rows != rows || A.columns != columns)
                throw new System.InvalidOperationException("Matrix addition can only by by matrises with same dimension");

            for (int i = 0; i < elements; i++)
                this[i] += k*A[i];
        }

        public void HadamarDivision(Matrix A)
        {
            if (A.rows != rows || A.columns != columns)
                throw new System.InvalidOperationException("Matrix dimensions not compatible for hadamar division");

            for (int i = 0; i < elements; i++)
                this[i] /= A[i];
        }

        public void HadamarProduct(Matrix A)
        {
            if (A.rows != rows || A.columns != columns)
                throw new System.InvalidOperationException("Matrix dimensions not compatible for hadamar product");

            for (int i = 0; i < elements; i++)
                this[i] *= A[i];
        }

        public void StoreMultiplication(Matrix A, Matrix B)
        {
            if (A.IsVector && !B.IsVector && A.columns != B.rows)
                throw new System.InvalidOperationException("Matrix dimensions not compatible for product");
            if(A.rows != rows || B.columns != columns)
                throw new System.InvalidOperationException("Product doesnt have same dimensions as intented carrier");

            Fill(0d);

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < columns; c++)
                    for (int i = 0; i < A.columns; i++)
                        this[r, c] += A[r, i] * B[i, c];
        }

        public void StoreAddition(Matrix A, Matrix B,double k1 = 1, double k2 = 1)
        {
            if (A.rows != B.rows || A.columns != B.columns)
                throw new System.InvalidOperationException("Matrix dimensions not compatible for sum");
            if (A.rows != rows || A.columns != columns)
                throw new System.InvalidOperationException("Product doesnt have same dimensions as intented carrier");

            Fill(0d);

            for (int i = 0; i < elements; i++)
                this[i] = k1*A[i] + k2*B[i];
        }

        public void StoreHadamarProduct(Matrix A, Matrix B)
        {
            if (A.rows != B.rows || A.columns != B.columns)
                throw new System.InvalidOperationException("Matrix dimensions not compatible for hadamar prod");
            if (A.rows != rows || A.columns != columns)
                throw new System.InvalidOperationException("Product doesnt have same dimensions as intented carrier");

            for (int i = 0; i < elements; i++)
                this[i] = A[i] * B[i];
        }

        public void StoreHadamarDivision(Matrix A, Matrix B)
        {
            if (A.rows != B.rows || A.columns != B.columns)
                throw new System.InvalidOperationException("Matrix dimensions not compatible for hadamar prod");
            if (A.rows != rows || A.columns != columns)
                throw new System.InvalidOperationException("Product doesnt have same dimensions as intented carrier");

            for (int i = 0; i < elements; i++)
                this[i] = A[i] / B[i];
        }

        public void StoreElemtwizeTransform( Matrix A, Func<Double,Double> function)
        {
            if (A.rows != rows || A.columns != columns)
                throw new System.InvalidOperationException("Matrix addition can only by by matrises with same dimension");

            for (int i = 0; i < elements; i++)
                this[i] = function(A[i]);
        }



        #endregion

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

        //Hadamar operator. Yeah we use % but dont have any other easily available;
        public static Matrix operator %(Matrix A, Matrix B)
        {
            if((A.rows != B.rows && A.columns != B.columns) || (A.IsVector && B.IsVector && (A.elements != B.elements)))
                throw new System.InvalidOperationException("Matrix dimensions not compatible for hadamar-product");

            Matrix m = new Matrix(A.rows, A.columns);
            for (int i = 0; i < m.elements; i++)
                m[i] = A[i] * B[i];

            return m;
        }

        #endregion
    }
}
