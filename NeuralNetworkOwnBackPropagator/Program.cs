﻿using System;
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
            RepositoryMNIST repo = new RepositoryMNIST(10);
            List<DigitalImage> images = repo.getImages();
            

            
        }
    }
}
