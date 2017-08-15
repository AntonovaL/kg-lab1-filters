using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;


namespace lab1_filters
{
    abstract class Filters
    {

        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        public double Clamp(double value, double min, double max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        virtual public Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }
            return resultImage;
        }

        protected abstract Color calculateNewPixelColor(Bitmap sourceImage, int x, int y);


    }



    class BrightnessFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            const int k = 30;
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(Clamp((int)sourceColor.R + k, 0, 255),
                Clamp((int)sourceColor.G + k, 0, 255),
                Clamp((int)sourceColor.B + k, 0, 255));
            return resultColor;
        }
    }

    class Sepia : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            double intensy = 0.36 * sourceColor.R + 0.53 * sourceColor.G + 0.11 * sourceColor.B;
            double R = intensy + 2 * 25;
            double G = intensy + 0.5 * 25;
            double B = intensy - 1 * 25;
            Color resultColor = Color.FromArgb(
                Clamp((int)R, 0, 255),
                Clamp((int)G, 0, 255),
                Clamp((int)B, 0, 255));
            return resultColor;
        }
    }

    class InvertFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(255 - sourceColor.R, 255 - sourceColor.G, 255 - sourceColor.B);
            return resultColor;
        }
    }

    class GrayScaleFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {

            Color sourceColor = sourceImage.GetPixel(x, y);
            double intensy = 0.36 * sourceColor.R + 0.53 * sourceColor.G + 0.11 * sourceColor.B;
            Color resultColor = Color.FromArgb((int)intensy, (int)intensy, (int)intensy);
            return resultColor;

        }
    }

    class HorizontalWavesFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {

            double resultR;
            double resultG;
            double resultB;
            Color sourceColor = sourseImage.GetPixel(x, y);

            int newX = (int)(x + 20 * Math.Sin(2 * Math.PI * x / 30));
            if (newX >= 0 && newX < sourseImage.Width)
            {
                sourceColor = sourseImage.GetPixel(newX, y);

            }
            resultR = sourceColor.R;
            resultG = sourceColor.G;
            resultB = sourceColor.B;

            return Color.FromArgb(Clamp((int)resultR, 0, 255),
                                  Clamp((int)resultG, 0, 255),
                                   Clamp((int)resultB, 0, 255));

        }
    }
    class VerticalWavesFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {

            double resultR;
            double resultG;
            double resultB;
            Color sourceColor = sourseImage.GetPixel(x, y);

            int newX = (int)(x + 20 * Math.Sin(2 * Math.PI * y / 60));
            if (newX >= 0 && newX < sourseImage.Width)
            {
                sourceColor = sourseImage.GetPixel(newX, y);

            }
            resultR = sourceColor.R;
            resultG = sourceColor.G;
            resultB = sourceColor.B;

            return Color.FromArgb(Clamp((int)resultR, 0, 255),
                                  Clamp((int)resultG, 0, 255),
                                   Clamp((int)resultB, 0, 255));

        }
    }
    class EffectOfGlass : Filters
    {
        Random random = new Random();
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {

            int newX = (int)(x + (random.Next(10) % 2 - 0.5) * 10);
            int newY = (int)(y + (random.Next(10) % 2 - 0.5) * 10);
            double resultR, resultG, resultB;
            if (newY < 0 || newY >= sourseImage.Height || newX < 0 || newX >= sourseImage.Width)
            {
                Color sourceColor = sourseImage.GetPixel(x, y);
                resultR = sourceColor.R;
                resultG = sourceColor.G;
                resultB = sourceColor.B;

            }
            else
            {
                Color sourseColor = sourseImage.GetPixel(newX, newY);
                resultR = sourseColor.R;
                resultG = sourseColor.G;
                resultB = sourseColor.B;
            }
            Color resultColor = Color.FromArgb(Clamp((int)resultR, 0, 255),
                                               Clamp((int)resultG, 0, 255),
                                               Clamp((int)resultB, 0, 255));
            return resultColor;
        }
    }

    class TurnFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {

            int x0 = sourseImage.Width / 2,
                y0 = sourseImage.Height / 2;
            double a = Math.PI / 4;

            double resultR;
            double resultG;
            double resultB;

            int newX = (int)((x - x0) * Math.Cos(a) - (y - y0) * Math.Sin(a) + x0);
            int newY = (int)((x - x0) * Math.Sin(a) + (y - y0) * Math.Cos(a) + y0);

            if (newX < 0 || newX >= sourseImage.Width || newY < 0 || newY >= sourseImage.Height)
            {
                resultR = 0;
                resultG = 0;
                resultB = 0;
            }
            else
            {
                Color sourceColor = sourseImage.GetPixel(newX, newY);
                resultR = sourceColor.R;
                resultG = sourceColor.G;
                resultB = sourceColor.B;
            }


            return Color.FromArgb(Clamp((int)resultR, 0, 255),
                                  Clamp((int)resultG, 0, 255),
                                   Clamp((int)resultB, 0, 255));

        }
    }

    class LinearStretching : Filters
    {
        int maxR;
        int maxG;
        int maxB;
        int minR;
        int minG;
        int minB;

        private void MaxMinRGB(Bitmap sourceImage)
        {

            MyColor color;

            int R;
            int G;
            int B;

            var list = new List<MyColor>();

            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    R = sourceImage.GetPixel(i, j).R;
                    G = sourceImage.GetPixel(i, j).G;
                    B = sourceImage.GetPixel(i, j).B;
                    color = new MyColor(R, G, B);
                    list.Add(color);
                }
            }

            list.Sort();

            maxR = list.ElementAt((sourceImage.Width - 1) * (sourceImage.Height - 1)).R;
            maxG = list.ElementAt((sourceImage.Width - 1) * (sourceImage.Height - 1)).G;
            maxB = list.ElementAt((sourceImage.Width - 1) * (sourceImage.Height - 1)).B;

            minR = list.ElementAt(0).R;
            minG = list.ElementAt(0).G;
            minB = list.ElementAt(0).B;
        }


        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            int R = (sourseImage.GetPixel(x, y).R - minR) * (255 / (maxR - minR));
            int G = (sourseImage.GetPixel(x, y).G - minG) * (255 / (maxG - minG));
            int B = (sourseImage.GetPixel(x, y).B - minB) * 255 / (maxB - minB);
            return Color.FromArgb(Clamp(R, 0, 255), Clamp(G, 0, 255), Clamp(B, 0, 255));
        }
        public override Bitmap processImage(Bitmap sourseImage, BackgroundWorker worker)
        {
            MaxMinRGB(sourseImage);
            return base.processImage(sourseImage, worker);
        }
    }
    class GrayWorldFilter : Filters
    {
        float avg;
        float avgR;
        float avgG;
        float avgB;

        public GrayWorldFilter(ref Bitmap sourceImage)
        {
            avg = 0;
            avgR = 0;
            avgG = 0;
            avgB = 0;
            float n = sourceImage.Width * sourceImage.Height;
            for (int x = 0; x < sourceImage.Width; x++)
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    avgR += sourceImage.GetPixel(x, y).R / n;
                    avgG += sourceImage.GetPixel(x, y).G / n;
                    avgB += sourceImage.GetPixel(x, y).B / n;
                }
            avg = (avgR + avgG + avgB) / 3;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            return Color.FromArgb
                 (Clamp((int)(avg / avgR * sourceColor.R), 0, 255),
                 Clamp((int)(avg / avgG * sourceColor.G), 0, 255),
                 Clamp((int)(avg / avgB * sourceColor.B), 0, 255));
        }
    }
}