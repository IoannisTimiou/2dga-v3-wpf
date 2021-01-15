using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AForge.Genetic;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Numerics;

namespace _2dga_v3_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<Container> containers = new List<Container>();
        static List<Package> items = new List<Package>();



        private int populationSize = 40;
        Random random = new Random();
        private _2DFitness newfitness = new _2DFitness();
        private Population population;

        static int gen = 1;
        static int conMaxX = 0;
        static int conTotalY = 0;
        public MainWindow()
        {
            InitializeComponent();
            InitializeProblem3();

            for (int i = 0; i < containers.Count; i++)
            {
                conTotalY += containers[i].y;
                if (conMaxX < containers[i].x)
                {
                    conMaxX = containers[i].x;
                }
            }
            population = new Population(populationSize, new _2DChromosome(containers, items, random), newfitness, new EliteSelection());
            population.RandomSelectionPortion = 0.4;

            for (int i = 0; i < 5; i++)
            {
                displayContainers(i);
                displayItems((_2DChromosome)population[i], i);
            }
            string title = "Generation: " + gen + ", Fitness of first 5: " + population[0].Fitness;
            for (int i = 1; i < 5; i++)
            {
                title += ", " + population[i].Fitness;
            }
            viewPort3d.Title = title;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (PopSize != null && Convert.ToInt32(PopSize.Text) >= 5)
            {
                populationSize = Convert.ToInt32(PopSize.Text);
            }
            this.PopSize.Text = string.Empty;
            FocusInputText();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (MutationRate != null && Convert.ToDouble(MutationRate.Text) >= 0.1 && Convert.ToDouble(MutationRate.Text) <= 1)
            {
                population.MutationRate = Convert.ToDouble(MutationRate.Text);
            }
            if (CrossoverRate != null && Convert.ToDouble(CrossoverRate.Text) >= 0.1 && Convert.ToDouble(CrossoverRate.Text) <= 1)
            {
                population.CrossoverRate = Convert.ToDouble(CrossoverRate.Text);
            }
            if (RandomGeneration != null && Convert.ToDouble(RandomGeneration.Text) >= 0 && Convert.ToDouble(RandomGeneration.Text) <= 0.9)
            {
                population.RandomSelectionPortion = Convert.ToDouble(RandomGeneration.Text);
            }
            this.MutationRate.Text = string.Empty;
            this.CrossoverRate.Text = string.Empty;
            this.RandomGeneration.Text = string.Empty;
            FocusInputText();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            viewPort3d.Children.Clear();
            viewPort3d.Children.Add(new ModelVisual3D() { Content = new AmbientLight(Colors.White) });
            population = new Population(populationSize, new _2DChromosome(containers, items, random), newfitness, new EliteSelection());
            gen = 1;
            for (int i = 0; i < 5; i++)
            {
                displayContainers(i);
                displayItems((_2DChromosome)population[i], i);
            }
            string title = "Generation: " + gen + ", Fitness of first 5: " + population[0].Fitness;
            for (int i = 1; i < 5; i++)
            {
                title += ", " + population[i].Fitness;
            }
            viewPort3d.Title = title;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            viewPort3d.Children.Clear();
            viewPort3d.Children.Add(new ModelVisual3D() { Content = new AmbientLight(Colors.White) });
            gen++;
            population.RunEpoch();
            for (int i = 0; i < 5; i++)
            {
                displayContainers(i);
                displayItems((_2DChromosome)population[i], i);
            }
            string title = "Generation: " + gen + ", Fitness of 5 best: " + population[0].Fitness;
            for (int i = 1; i < 5; i++)
            {
                title += ", " + population[i].Fitness;
            }
            viewPort3d.Title = title;
        }


        private void QuickGenButton_Click(object sender, RoutedEventArgs e)
        {
            viewPort3d.Children.Clear();
            viewPort3d.Children.Add(new ModelVisual3D() { Content = new AmbientLight(Colors.White) });
            for (int i = 0; i < 10; i++)
            {
                gen++;
                population.RunEpoch();
            }
            for (int i = 0; i < 5; i++)
            {
                displayContainers(i);
                displayItems((_2DChromosome)population[i], i);
            }
            string title = "Generation: " + gen + ", Fitness of 5 best: " + population[0].Fitness;
            for (int i = 1; i < 5; i++)
            {
                title += ", " + population[i].Fitness;
            }
            viewPort3d.Title = title;
        }
        

        private void Skip100Button_Click(object sender, RoutedEventArgs e)
        {
            viewPort3d.Children.Clear();
            viewPort3d.Children.Add(new ModelVisual3D() { Content = new AmbientLight(Colors.White) });
            for (int i = 0; i < 100; i++)
            {
                gen++;
                population.RunEpoch();
            }
            for (int i = 0; i < 5; i++)
            {
                displayContainers(i);
                displayItems((_2DChromosome)population[i], i);
            }
            string title = "Generation: " + gen + ", Fitness of 5 best: " + population[0].Fitness;
            for (int i = 1; i < 5; i++)
            {
                title += ", " + population[i].Fitness;
            }
            viewPort3d.Title = title;
        }

        private void PerfectButton_Click(object sender, RoutedEventArgs e)
        {
            viewPort3d.Children.Clear();
            viewPort3d.Children.Add(new ModelVisual3D() { Content = new AmbientLight(Colors.White) });

            while (population.FitnessMax < 1 && gen < 10000)
            {
                gen++;
                population.RunEpoch();
            }
            for (int i = 0; i < 5; i++)
            {
                displayContainers(i);
                displayItems((_2DChromosome)population[i], i);
            }
            string title = "Generation: " + gen + ", Fitness of 5 best: " + population[0].Fitness;
            for (int i = 1; i < 5; i++)
            {
                title += ", " + population[i].Fitness;
            }
            viewPort3d.Title = title;
        }

        private void displayContainers(int solution)
        {
            double contotal = 0;
            for (int i = 0; i < containers.Count; i++)
            {
                RectangleVisual3D cont = new RectangleVisual3D();
                double dimx = Convert.ToDouble(containers[i].x);
                double dimy = Convert.ToDouble(containers[i].y);
                cont.Origin = new Point3D(dimx / 2 + solution * (conMaxX + 20), (contotal + 20 * i + dimy / 2), -0.01);
                cont.Length = dimx;
                cont.Width = dimy;
                cont.Material = new DiffuseMaterial(Brushes.Silver);
                cont.BackMaterial = new DiffuseMaterial(Brushes.Transparent);
                viewPort3d.Children.Add(cont);
                contotal += dimy;
            }
        }

        private void displayItems(_2DChromosome c, int solution)
        {
            for (int i = 0; i < c.positions.Count; i++)
            {
                RectangleVisual3D rectangle = new RectangleVisual3D();
                double ystart = 0;
                for (int j = 0; j < c.positions[i].contnum; j++)
                {
                    ystart += containers[j].y;
                }
                ystart += 20 * c.positions[i].contnum + c.positions[i].toplefty + (c.items[i].y) / 2;
                rectangle.Origin = new Point3D(solution * (conMaxX + 20) + c.positions[i].topleftx + (c.items[i].x) / 2, ystart, i * 0.001);
                rectangle.Length = c.items[i].x - 0.5;
                rectangle.Width = c.items[i].y - 0.5;
                rectangle.Material = MaterialHelper.CreateMaterial(Colors.Blue, 0.5);
                viewPort3d.Children.Add(rectangle);
            }
        }
        private void FocusInputText()
        {
            this.PopSize.Focus();
        }


        private void InitializeProblem1()
        {
            containers.Add(new Container(400, 400));
            Tuple<double, double>[] load = { new Tuple<double, double>(0, 0), new Tuple<double, double>(5.4, 6), new Tuple<double, double>(2, 7), new Tuple<double, double>(4, 1)};
            for (int i = 0; i < load.Length; i++)
            {
                Package package = new Package(Convert.ToInt32(load[i].Item1 * 10), Convert.ToInt32(load[i].Item2 * 10));
                items.Add(package);
            }
            viewPort3d.SetView(new Point3D(1000, 450, 3000), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0));
        }

        private void InitializeProblem2()
        {
            containers.Add(new Container(400, 400));
            Tuple<double, double>[] load = { new Tuple<double, double>(0, 0), 
                                               new Tuple<double, double>(5.4, 6), new Tuple<double, double>(2, 7), new Tuple<double, double>(4, 1),
                                               new Tuple<double, double>(4.7, 3.2), new Tuple<double, double>(9.3, 8),  new Tuple<double, double>(7, 5.3),
                                               new Tuple<double, double>(6.4, 5), new Tuple<double, double>(8, 3), new Tuple<double, double>(5, 8),
                                               new Tuple<double, double>(9.1, 4), new Tuple<double, double>(2, 6), new Tuple<double, double>(7.4, 9.6)};
            for (int i = 0; i < load.Length; i++)
            {
                Package package = new Package(Convert.ToInt32(load[i].Item1 * 10), Convert.ToInt32(load[i].Item2 * 10));
                items.Add(package);
            }
            viewPort3d.SetView(new Point3D(1000, 450, 3000), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0));
        }

        private void InitializeProblem3()
        {
            containers.Add(new Container(400, 100));
            containers.Add(new Container(200, 100));
            containers.Add(new Container(200, 100));
            Tuple<double, double>[] load = { new Tuple<double, double>(0, 0), 
                                               new Tuple<double, double>(5.4, 6), new Tuple<double, double>(2, 7), new Tuple<double, double>(4, 1),
                                               new Tuple<double, double>(4.7, 3.2), new Tuple<double, double>(9.3, 8),  new Tuple<double, double>(7, 5.3),
                                               new Tuple<double, double>(6.4, 5), new Tuple<double, double>(8, 3), new Tuple<double, double>(5, 8),
                                               new Tuple<double, double>(9.1, 4), new Tuple<double, double>(2, 6), new Tuple<double, double>(7.4, 9.6)};
            for (int i = 0; i < load.Length; i++)
            {
                Package package = new Package(Convert.ToInt32(load[i].Item1 * 10), Convert.ToInt32(load[i].Item2 * 10));
                items.Add(package);
            }
            viewPort3d.SetView(new Point3D(1000, 450, 3000), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0));
        }

        private void InitializeProblem4()
        {
            containers.Add(new Container(120, 23));
            containers.Add(new Container(60, 23));
            containers.Add(new Container(60, 23));
            Tuple<double, double>[] load = { new Tuple<double, double>(0, 0),
                                                 new Tuple<double, double>(9.3, 0.1), new Tuple<double, double>(9.7, 0.3),
                                                 new Tuple<double, double>(0.2, 0.3), new Tuple<double, double>(0.2, 0.3), new Tuple<double, double>(0.2, 0.3),
                                                 new Tuple<double, double>(0.3, 0.1), new Tuple<double, double>(0.1, 1),
                                                 new Tuple<double, double>(0.1, 0.6), new Tuple<double, double>(0.3, 0.1),
                                                 new Tuple<double, double>(0.4, 0.3), new Tuple<double, double>(0.3, 0.2),

                                                 new Tuple<double, double>(1.4, 0.2), new Tuple<double, double>(0.6, 0.3),
                                                 new Tuple<double, double>(1.4, 0.2), new Tuple<double, double>(10.2, 0.3), new Tuple<double, double>(0.6, 0.3),
                                                 new Tuple<double, double>(0.2, 0.6), new Tuple<double, double>(0.2, 0.6),
                                                 new Tuple<double, double>(0.6, 0.2), new Tuple<double, double>(10.2, 0.2),
                                                 new Tuple<double, double>(0.6, 0.3), new Tuple<double, double>(10.2, 0.3), new Tuple<double, double>(0.4, 0.1),
                                                 new Tuple<double, double>(0.4, 0.1)};
            for (int i = 0; i < load.Length; i++)
            {
                Package package = new Package(Convert.ToInt32(load[i].Item1 * 10), Convert.ToInt32(load[i].Item2 * 10));
                items.Add(package);
            }
            viewPort3d.SetView(new Point3D(320, 100, 900), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0));
        }

        private void InitializeProblem5()
        {
            containers.Add(new Container(120, 23));
            containers.Add(new Container(60, 23));
            Tuple<double, double>[] load = { new Tuple<double, double>(0, 0),
                                                 new Tuple<double, double>(9.3, 0.1), new Tuple<double, double>(9.7, 0.3),
                                                 new Tuple<double, double>(0.2, 0.3), new Tuple<double, double>(0.2, 0.3), new Tuple<double, double>(0.2, 0.3),
                                                 new Tuple<double, double>(0.3, 0.1), new Tuple<double, double>(0.1, 1),
                                                 new Tuple<double, double>(0.1, 0.6), new Tuple<double, double>(0.3, 0.1),
                                                 new Tuple<double, double>(0.4, 0.3), new Tuple<double, double>(0.3, 0.2),

                                                 new Tuple<double, double>(8.4, 0.4), new Tuple<double, double>(0.2, 0.3), new Tuple<double, double>(0.3, 0.1),
                                                 new Tuple<double, double>(0.3, 0.1), new Tuple<double, double>(0.6, 0.3),
                                                 new Tuple<double, double>(8.7, 0.3), new Tuple<double, double>(9.1, 0.1),
                                                 new Tuple<double, double>(1.2, 0.5), new Tuple<double, double>(1.2, 0.5),

                                                 new Tuple<double, double>(1.4, 0.2), new Tuple<double, double>(0.6, 0.3),
                                                 new Tuple<double, double>(1.4, 0.2), new Tuple<double, double>(10.2, 0.3), new Tuple<double, double>(0.6, 0.3),
                                                 new Tuple<double, double>(0.2, 0.6), new Tuple<double, double>(0.2, 0.6),
                                                 new Tuple<double, double>(0.6, 0.2), new Tuple<double, double>(10.2, 0.2),
                                                 new Tuple<double, double>(0.6, 0.3), new Tuple<double, double>(10.2, 0.3), new Tuple<double, double>(0.4, 0.1),
                                                 new Tuple<double, double>(0.4, 0.1)};
            for (int i = 0; i < load.Length; i++)
            {
                Package package = new Package(Convert.ToInt32(load[i].Item1 * 10), Convert.ToInt32(load[i].Item2 * 10));
                items.Add(package);
            }
            viewPort3d.SetView(new Point3D(320, 100, 900), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0));
        }
    }
}

