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
        public Matrix[] A;

        public SigmoidNeuralNetwork(int Layers, int[] nodesPerLayer)
        {
            this.Layers = Layers;
            A = new Matrix[Layers];
            B = new Matrix[Layers];
            W = new Matrix[Layers];

            for(int i = 1; i < Layers; i++)
            {
                A[i] = new Matrix(1, nodesPerLayer[i]);
                B[i] = new Matrix(1, nodesPerLayer[i]);
                W[i] = new Matrix(nodesPerLayer[i - 1], nodesPerLayer[i]);
            }
        }

        public void SetEntry(Matrix A0)
        {
            A[0] = A0;
        }

        public void Calculate()
        {
            for (int i = 1; i < Layers; i++)
                ForwardA(i);
        }

        public void ForwardA(int layer)
        {
            A[layer] = (A[layer-1] * W[layer]) + B[layer];
            A[layer].ElementWiseFunction(Sigmoid);
        }

        public double Sigmoid(double z)
        {
            return 1 / (1 + Math.Exp(z));
        }

        public double vSigmoidPrim(double z)
        {
            return 1 / Math.Pow(1 + Math.Exp(z),2);
        }

    }

}
