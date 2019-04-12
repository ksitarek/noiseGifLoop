using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NoiseGifLoop
{
    public class Shape
    {
        private readonly PerlinNoise _perlin;
        private readonly Random _rand;
        private readonly double _seed;
        private readonly double _offset;
        private readonly double _thetaMultiplier;
        private readonly double _alphaMultiplier;
        private readonly double _strokeMultiplier;
        private readonly double _scale;
        private readonly double _displacement;
        private readonly int _width;
        private readonly int _height;
        private readonly WriteableBitmap _bitmap;

        private const double TwoPi = Math.PI * 2;

        private Color ShapeColor = Colors.White;

        public const int PerlinScale = 1;

        public Shape(WriteableBitmap bitmap, PerlinNoise perlin, int seed)
        {
            _bitmap = bitmap;
            _width = (int)_bitmap.Width;
            _height = (int)_bitmap.Height;

            _perlin = perlin;

            // initialize rand values
            _rand = new Random(seed);
            _seed = _rand.Next(100, 10000) / 10.0;
            _offset = _rand.Next((int)(TwoPi * 10000)) / 10000.0;
            _thetaMultiplier = 0.1 + 0.5 * Math.Pow(_rand.NextDouble(), 2);
            _alphaMultiplier = _rand.Next(5, 25) / 10.0;
            _strokeMultiplier = _rand.NextDouble();
            _scale = _rand.Next(50, 115) / 100.0;
            _displacement = _rand.Next(100, 1200) / 60.0;
        }

        public void Draw(double time)
        {
            var overlay = BitmapFactory.New(_width, _height);
            overlay.Clear(Color.FromArgb(0, 0, 0, 0));

            using (overlay.GetBitmapContext())
            {

                double? prevX = null;
                double? prevY = null;
                for (var i = 0f; i < TwoPi; i += 0.05f)
                {
                    var progress = 1.0 * i / TwoPi;
                    var theta = i;

                    var radius = 1.3;

                    float perlinX1 = (float)(_seed + radius * Math.Cos(TwoPi * (2 * progress - time)));
                    float perlinX2 = 2 * perlinX1;

                    float perlinY1 = (float)(radius * Math.Sin(TwoPi * (2 * progress - time)));
                    float perlinY2 = perlinY1;

                    var x = _scale * HeartX(theta);
                    x += _perlin.Noise(perlinX1, perlinY1) * PerlinScale * _displacement;

                    var y = _scale * HeartY(theta) * -1;
                    y += _perlin.Noise(perlinX2, perlinY2) * PerlinScale * _displacement;

                    // move drawing to the center of the canvas
                    x += _width / 2;
                    y += _height / 2;

                    if (prevX.HasValue && prevY.HasValue)
                    {

                        var alpha = 30 + _alphaMultiplier * 18 * Math.Sin(progress * Math.PI);

                        var color = Color.FromArgb((byte)alpha, ShapeColor.R, ShapeColor.G, ShapeColor.B);
                        overlay.DrawLineAa(
                            (int)prevX,
                            (int)prevY,
                            (int)x,
                            (int)y,
                            color);
                    }

                    prevX = x;
                    prevY = y;
                }
            }

            var rect = new Rect(0, 0, overlay.PixelWidth, overlay.PixelHeight);
            _bitmap.Blit(rect, overlay, rect, WriteableBitmapExtensions.BlendMode.Additive);
        }

        // functions to determine x and y of a perfect heart
        private double HeartX(double t) => 8 * (16 * Math.Pow(Math.Sin(t), 3));
        private double HeartY(double t) => 8 * (13 * Math.Cos(t) - 5 * Math.Cos(2 * t) - 2 * Math.Cos(3 * t) - Math.Cos(4 * t));
    }
}
