using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace NoiseGifLoop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Random _rand = new Random();
        private PerlinNoise _perlin;
        private WriteableBitmap _bitmap;

        private const float TimeScale = 100;
        private const int ShapesCnt = 250;
        private Shape[] Shapes;

        public MainWindow()
        {
            InitializeComponent();
            InitPerlinNoise();
            InitializeBitmap();
            InitializeShapes();
            StartDrawing();
        }

        private void Draw(double time)
        {
            foreach (var shape in Shapes)
            {
                shape.Draw(time);
            }
        }


        private void DrawWrapper(double time)
        {
            using (_bitmap.GetBitmapContext())
            {
                // clear with black background
                _bitmap.Clear(Colors.Black);

                // call the actual draw
                Draw(time);
            }
        }

        private void InitPerlinNoise()
        {
            _perlin = new PerlinNoise(_rand.Next());
        }

        private void InitializeBitmap()
        {
            var width = (int)Image.Width;
            var height = (int)Image.Height;

            _bitmap = BitmapFactory.New(width, height);
            Image.Source = _bitmap;

        }

        private void InitializeShapes()
        {
            Shapes = new Shape[ShapesCnt];
            for (var i = 0; i < ShapesCnt; i++)
            {
                Shapes[i] = new Shape(_bitmap, _perlin, _rand.Next());
            }
        }

        private void StartDrawing()
        {
            var time = 0;

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += (s, e) =>
            {
                DrawWrapper(time / TimeScale);

                time++;
                if (time > TimeScale)
                {
                    time = 0;
                }
            };

            timer.Start();
        }
    }
}
