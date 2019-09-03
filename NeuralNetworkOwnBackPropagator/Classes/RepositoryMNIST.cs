using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkOwnBackPropagator
{
    public class RepositoryMNIST
    {
        FileStream fsLabels;
        FileStream fsImages;

        List<DigitalImage> images = new List<DigitalImage>();

        public RepositoryMNIST(int N)
        {
            fsLabels = new FileStream(@"C:\Users\Seth\Documents\c#\NeuralNetworkOwnBackPropagator\NeuralNetworkOwnBackPropagator\data\train-labels.idx1-ubyte", FileMode.Open);
            fsImages = new FileStream(@"C:\Users\Seth\Documents\c#\NeuralNetworkOwnBackPropagator\NeuralNetworkOwnBackPropagator\data\train-images.idx3-ubyte", FileMode.Open);

            BinaryReader brLabels = new BinaryReader(fsLabels);
            BinaryReader brImages = new BinaryReader(fsImages);

            int magicLabel = brLabels.ReadInt32();
            int nrLabels = brLabels.ReadInt32();

            int magicImage = brImages.ReadInt32();
            int nrImages = brImages.ReadInt32();
            int nrImagesRows = brImages.ReadInt32();
            int nrImagesCols = brImages.ReadInt32();

            for(int i = 0; i< N; i++)
            {
                byte label = brLabels.ReadByte();
                byte[,] pixels = new byte[28,28];

                for (int n = 0; n < 28; n++)
                    for (int m = 0; m < 28; m++)
                        pixels[n, m] = brImages.ReadByte();

                images.Add(new DigitalImage(label, pixels));
            }

            fsLabels.Close();
            brLabels.Close();
            fsImages.Close();
            brImages.Close();
           
        }

        public List<DigitalImage> getImages() => images;

    }
}
