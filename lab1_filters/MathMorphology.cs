using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace lab1_filters
{
  static  class MathMorphology
    {
        static public Bitmap Erosion(Bitmap sourceImage, bool[,] matrix)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            int mh = matrix.GetLength(0);
            int mw = matrix.GetLength(1);

            int minR;
            int minG;
            int minB;

            Color sourceColor;

            for (int x = mw / 2; x < sourceImage.Width - mw / 2; x++)
                for (int y = mh / 2; y < sourceImage.Height - mh / 2; y++)
                {
                    sourceColor = sourceImage.GetPixel(x - mh / 2, y - mh / 2);
                    minR = sourceColor.R;
                    minG = sourceColor.G;
                    minB = sourceColor.B;

                    for (int i = -mw / 2; i <= mw / 2; i++)
                        for (int j = -mh / 2; j <= mh / 2; j++)
                        {
                            if (matrix[i + mh / 2, j + mw / 2])
                            {
                                if (sourceImage.GetPixel(x + i, y + j).R < minR)
                                    minR = sourceImage.GetPixel(x + i, y + j).R;
                                if (sourceImage.GetPixel(x + i, y + j).G < minG)
                                    minG = sourceImage.GetPixel(x + i, y + j).R;
                                if (sourceImage.GetPixel(x + i, y + j).B < minB)
                                    minB = sourceImage.GetPixel(x + i, y + j).B;
                            }
                        }
                    resultImage.SetPixel(x - mh / 2, y - mw / 2, Color.FromArgb(minR, minG, minB));
                }
            return resultImage;

        }

        static public Bitmap Dilation(Bitmap sourceImage, bool[,] matrix)

        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            int mh = matrix.GetLength(0);
            int mw = matrix.GetLength(1);

            int maxR;
            int maxG;
            int maxB;

            Color sourceColor;

            for (int x = mw / 2; x < sourceImage.Width - mw / 2; x++)
                for (int y = mh / 2; y < sourceImage.Height - mh / 2; y++)
                {
                    sourceColor = sourceImage.GetPixel(x - mh / 2, y - mh / 2);
                    maxR = sourceColor.R;
                    maxG = sourceColor.G;
                    maxB = sourceColor.B;

                    for (int i = -mw / 2; i <= mw / 2; i++)
                        for (int j = -mh / 2; j <= mh / 2; j++)
                        {
                            if (matrix[i + mh / 2, j + mw / 2])
                            {
                                if (sourceImage.GetPixel(x + i, y + j).R > maxR)
                                    maxR = sourceImage.GetPixel(x + i, y + j).R;
                                if (sourceImage.GetPixel(x + i, y + j).G > maxG)
                                    maxG = sourceImage.GetPixel(x + i, y + j).R;
                                if (sourceImage.GetPixel(x + i, y + j).B > maxB)
                                    maxB = sourceImage.GetPixel(x + i, y + j).B;
                            }
                        }
                    resultImage.SetPixel(x - mh / 2, y - mw / 2, Color.FromArgb(maxR, maxG, maxB));
                }
            return resultImage;

        }

        static public Bitmap Gradient(Bitmap sourceImage, bool[,] matrix)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Bitmap image1 = Dilation(sourceImage, matrix);
            Bitmap image2 = Erosion(sourceImage, matrix);

            int R;
            int G;
            int B;

            for (int i = 0; i < sourceImage.Width; i++)
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    R = image1.GetPixel(i, j).R - image2.GetPixel(i, j).R;
                    G = image1.GetPixel(i, j).G - image2.GetPixel(i, j).G;
                    B = image1.GetPixel(i, j).B - image2.GetPixel(i, j).B;
                    resultImage.SetPixel(i, j, Color.FromArgb(Clamp(R, 0, 255), Clamp(G, 0, 255), Clamp(B, 0, 255)));
                }
            return resultImage;
        }

        static public Bitmap Opening(Bitmap sourseImage, bool[,] matrix)
        {

            Bitmap resultImage = Erosion(sourseImage, matrix);
            return resultImage = Dilation(sourseImage, matrix);
        }

        static public Bitmap Closing(Bitmap sourseImage, bool[,] matrix)
        {
            Bitmap resultImage = Dilation(sourseImage, matrix);
            return resultImage = Erosion(sourseImage, matrix);
        }


        static private int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        

    }

}
