using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkOwnBackPropagator
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = 10000;
            int batchSize = 10;
            RepositoryMNIST repo = new RepositoryMNIST(N);
            List<DigitalImage> images = repo.getImages();
            SigmoidNeuralNetwork NN = new SigmoidNeuralNetwork(new int[] { 28*28, 15, 10});

            Matrix[] Input = new Matrix[batchSize];
            Matrix[] CorrectAns = new Matrix[batchSize];

            for (int e = 0; e < 30; e++)
            {
                int correct = 0;
                int c = 0;

                for (int k = 0; k < N / batchSize; k++)
                {
                    for (int i = 0; i < batchSize; i++)
                    {
                        Input[c % 10] = new Matrix(28 * 28, 1);
                        for (int n = 0; n < 28 * 28; n++)
                            Input[c % batchSize][n] = images[c][n] / 1000d;

                        CorrectAns[c % batchSize] = new Matrix(10, 1);
                        CorrectAns[c % batchSize][images[c].label] = 1;
                        c++;
                    }

                    correct += NN.TrainingSession(Input, CorrectAns, 3.0d);
                }

                Console.WriteLine("Epoch " + e + ": " + String.Format("{0,5}",correct) + " /10 000");
            }
            /*
            for(int i = 0; i < 100; i++)
            {
                int correct = NN.TrainingSession(Input, CorrectAns, 3.0d);
                Console.WriteLine("Round " + i + ": " + correct);
            }
            */

            Console.ReadKey();
            
        }
    }
}
