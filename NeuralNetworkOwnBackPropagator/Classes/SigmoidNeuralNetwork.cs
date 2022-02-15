using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkOwnBackPropagator
{
    public class SigmoidNeuralNetwork
    {
        public int Layers;
        public Matrix[] W;
        public Matrix[] B;
        public Matrix[] Z;
        public Matrix[] A;
        public Matrix[] deltaW;
        public Matrix[] deltaB;
        public Matrix[] SigmoidPrims;
        public Matrix[] DeltaCosts;
        public Matrix[] descentW;
        public Matrix[] descentB;


        public SigmoidNeuralNetwork(int[] nodesPerLayer)
        {
            Layers = nodesPerLayer.Length;
            W = new Matrix[Layers];
            B = new Matrix[Layers];
            Z = new Matrix[Layers];
            A = new Matrix[Layers];
            deltaW = new Matrix[Layers];
            deltaB = new Matrix[Layers];
            SigmoidPrims = new Matrix[Layers];
            DeltaCosts = new Matrix[Layers];
            descentW = new Matrix[Layers];
            descentB = new Matrix[Layers];

            A[0] = new Matrix(nodesPerLayer[0], 1);
            
            for(int i = 1; i < Layers; i++)
            {
                W[i] = new Matrix(nodesPerLayer[i], nodesPerLayer[i-1]);
                B[i] = new Matrix(nodesPerLayer[i], 1);
                Z[i] = new Matrix(nodesPerLayer[i], 1);
                A[i] = new Matrix(nodesPerLayer[i], 1);
                deltaW[i] = new Matrix(nodesPerLayer[i], nodesPerLayer[i - 1]);
                deltaB[i] = new Matrix(nodesPerLayer[i], 1);
                SigmoidPrims[i] = new Matrix(nodesPerLayer[i], 1);
                DeltaCosts[i] = new Matrix(nodesPerLayer[i], 1);
                descentW[i] = new Matrix(nodesPerLayer[i], nodesPerLayer[i - 1]);
                descentB[i] = new Matrix(nodesPerLayer[i], 1);
            }

            Initalize();
        }

        void Initalize()
        {
            Random rnd = new Random();
            foreach (Matrix w in W)
                if(w != null) w.ElementWiseFunction(x => {return 0.001d ; });

            foreach (Matrix b in B)
                if (b != null) b.ElementWiseFunction(x => { return rnd.NextDouble()-0.5d; });
        }
        

        public int TrainingSession(Matrix[] Inputs, Matrix[] correctYs, double learningRate)
        {
            int NrCorrect = 0;
            for(int i = 1; i < Layers; i++)
            {
                descentW[i].Fill(0);
                descentB[i].Fill(0);
            }

            for(int i = 0; i<Inputs.Length; i++)
            {
                SetEntry(Inputs[i]);
                Calculate();
                if (LargestIndex(correctYs[i]) == LargestIndex(A[Layers - 1]))
                    NrCorrect++;

                for(int L = Layers -1; L > 0; L--)
                {
                    SigmoidPrims[L].StoreElemtwizeTransform(Z[L], SigmoidPrim);


                    if (L == Layers - 1)
                    {
                        DeltaCosts[L].StoreAddition(A[L], correctYs[i], 1, -1);
                        DeltaCosts[L].HadamarDivision(SigmoidPrims[L]);
                    }
                    else
                    {
                        W[L + 1].Transpose();
                        DeltaCosts[L].StoreMultiplication(W[L + 1], deltaB[L + 1]);
                        W[L + 1].Transpose();
                    }

                    deltaB[L].StoreHadamarProduct(DeltaCosts[L], SigmoidPrims[L]);

                    //piece of code since this part is none of the regular LA-operations
                    for (int m = 0; m < W[L].rows; m++)
                        for (int n = 0; n < W[L].columns; n++)
                            deltaW[L][m, n] = A[L - 1][n] * deltaB[L][m];

            
                    //Update
                    descentW[L].Add(deltaW[L]);
                    descentB[L].Add(deltaB[L]);
                }
            }

            for(int i = 1; i < Layers; i++)
            {
                W[i].Add(descentW[i], -learningRate /Inputs.Length);
                B[i].Add(descentB[i], -learningRate /Inputs.Length);
            }

            return NrCorrect;
        }

        public void SetEntry(Matrix A0)
        {
            A[0].StoreElemtwizeTransform(A0, x => { return x; });
        }

        public void Calculate()
        {
            for (int i = 1; i < Layers; i++)
                ForwardA(i);
        }

        public void ForwardA(int layer)
        {
            Z[layer].StoreMultiplication(W[layer], A[layer - 1]);
            Z[layer].Add(B[layer]);
            A[layer].StoreElemtwizeTransform(Z[layer], Sigmoid);
            //A[layer] = (W[layer] * A[layer-1]) + B[layer];
            //A[layer].ElementWiseFunction(Sigmoid);
        }

        public double Sigmoid(double z)
        {
            return 1 / (1 + Math.Exp(-z));
        }

        public double SigmoidPrim(double z)
        {
            return Math.Exp(-z) / Math.Pow(1 + Math.Exp(-z),2);
        }

        public int LargestIndex(Matrix Vector)
        {
            int maxIndex = 0;
            double maxObs = -0.1d;

            for (int i = 0; i < Vector.elements; i++)
            {
                if (Vector[i] > maxObs)
                {
                    maxIndex = i;
                    maxObs = Vector[i];
                }
            }
            return maxIndex;
        }

    }

}
