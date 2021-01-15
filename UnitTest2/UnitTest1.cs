using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using _2dga_v3_wpf;
using AForge.Genetic;

namespace UnitTest2
{
    [TestClass]
    public class UnitTest1
    {
        static double contLength = 40;
        static double contWidth = 40;
        Random random = new Random();
        static Container container = new Container(Convert.ToInt32(contLength * 10), Convert.ToInt32(contWidth * 10));
        static List<Container> containers = new List<Container>();
        static List<Package> items = new List<Package>();
        Tuple<double, double>[] load = { new Tuple<double, double>(5.4, 6), new Tuple<double, double>(2, 7), new Tuple<double, double>(4, 1),
                                               new Tuple<double, double>(4.7, 3.2), new Tuple<double, double>(9.3, 8),  new Tuple<double, double>(7, 5.3),
                                               new Tuple<double, double>(6.4, 5), new Tuple<double, double>(8, 3), new Tuple<double, double>(5, 8),
                                               new Tuple<double, double>(9.1, 4), new Tuple<double, double>(2, 6), new Tuple<double, double>(7.4, 9.6)};
        [TestMethod]
        public void TestMethod1()
        {
            _2DFitness fitness = new _2DFitness();
            for (int i = 0; i < load.Length; i++)
            {
                Package package = new Package(Convert.ToInt32(load[i].Item1 * 10), Convert.ToInt32(load[i].Item2 * 10));
                items.Add(package);
            }
            containers.Add(container);
            _2DChromosome specimen = new _2DChromosome(containers, items);
            specimen.itemplace(items[0], new Coordinates(0, 5, 0));
            specimen.itemplace(items[1], new Coordinates(0, 5, 0));
            Assert.IsTrue(fitness.Evaluate(specimen) < 1);
            Assert.IsTrue(fitness.OverlapCheck(items[0], items[1], new Coordinates(0, 5, 0), new Coordinates(0, 5, 0)));
            Assert.AreEqual((20-0) * (60-0), fitness.OverlapSize(items[0], items [1], new Coordinates(0, 0, 0), new Coordinates(0, 0, 0)));
            Assert.IsTrue(fitness.OverlapCheck(items[0], items[1], new Coordinates(0, 1, 1), new Coordinates(0, 3, 4)));
            Assert.IsFalse(fitness.OverlapCheck(items[0], items[1], new Coordinates(0, 1, 1), new Coordinates(0, 54, 100)));
            Assert.IsTrue(fitness.OverlapCheck(items[0], items[1], new Coordinates(0, 3, 4), new Coordinates(0, 1, 1)));
            Assert.IsTrue(fitness.OverlapCheck(items[0], items[1], new Coordinates(0, 0, 0), new Coordinates(0, 53, 59)));
            Assert.IsFalse(fitness.OverlapCheck(items[0], items[1], new Coordinates(0, 0, 0), new Coordinates(0, 55, 60)));
            Assert.IsTrue(fitness.OverlapCheck(items[0], items[1], new Coordinates(0, 1, 1), new Coordinates(0, 53, 30)));
            Assert.IsTrue(fitness.OverlapCheck(items[0], items[1], new Coordinates(0, 1, 30), new Coordinates(0, 20, 0)));
            Assert.IsTrue(fitness.OverlapCheck(items[0], items[2], new Coordinates(0, 1, 1), new Coordinates(0, 30, 40)));
            Assert.IsTrue(fitness.OverlapCheck(items[7], items[10], new Coordinates(0, 1, 30), new Coordinates(0, 20, 1)));
            Assert.IsTrue(fitness.OverlapCheck(items[10], items[7], new Coordinates(0, 20, 1), new Coordinates(0, 3, 30)));
            Assert.IsTrue(fitness.OverlapCheck(items[0], items[11], new Coordinates(0, 0, 30), new Coordinates(0, 20, 0)));
            Assert.IsTrue(fitness.OverlapCheck(items[0], items[11], new Coordinates(0, 70, 30), new Coordinates(0, 0, 0)));
            Assert.IsTrue(fitness.OverlapCheck(items[0], items[11], new Coordinates(0, 50, 60), new Coordinates(0, 0, 0)));
        }
        [TestMethod]
        public void TestMethod2()
        {
            _2DFitness fitness = new _2DFitness();
            for (int i = 0; i < load.Length; i++)
            {
                Package package = new Package(Convert.ToInt32(load[i].Item1 * 10), Convert.ToInt32(load[i].Item2 * 10));
                items.Add(package);
            }
            containers.Add(container);
            Population population = new Population(5, new _2DChromosome(containers, items, random), fitness, new EliteSelection());
            population.RunEpoch();
            Assert.IsTrue(population.BestChromosome == population[0]);
            population.RunEpoch();
            Assert.IsTrue(population.BestChromosome == population[0]);
            for (int i = 0; i < 10; i++)
            {
                population.RunEpoch();
                Assert.IsTrue(population.BestChromosome == population[0]);
            }
        }
    }
}
