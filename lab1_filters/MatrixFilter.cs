using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace lab1_filters
{
    class MatrixFilter : Filters
    {
        protected float[,] kernel = null;
        protected MatrixFilter() { }

        public MatrixFilter(float[,] kernel)
        {
            this.kernel = kernel;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            float resultR = 0;
            float resultG = 0;
            float resultB = 0;
            for (int l = -radiusY; l <= radiusY; l++)
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    resultR += neighborColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += neighborColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += neighborColor.B * kernel[k + radiusX, l + radiusY];
                }
            return Color.FromArgb(
                Clamp((int)resultR, 0, 255),
                Clamp((int)resultG, 0, 255),
                Clamp((int)resultB, 0, 255));
        }
    }

    class BlurFilter : MatrixFilter
    {
        public BlurFilter()
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                {
                    kernel[i, j] = 1.0f / (float)(sizeX * sizeY);
                }
        }
    }

    class SharpnessFilter : MatrixFilter
    {
        public void createSharpnessKernel()
        {
            kernel = new float[,] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
        }

        public SharpnessFilter()
        {
            createSharpnessKernel();
        }
    }


    class GaussianFilter : MatrixFilter
    {
        public void createGaussianKernel(int radius, float sigma)
        {
            int size = 2 * radius + 1; //размер ядра
            kernel = new float[size, size]; //ядро фильтра
            float norm = 0; //коэффициент нормировки
            for (int i = -radius; i <= radius; i++) //рассчет ядра фильтра
                for (int j = -radius; j <= radius; j++)
                {
                    kernel[i + radius, j + radius] = (float)(Math.Exp(-(i * i + j * j) / (sigma * sigma)));
                    norm += kernel[i + radius, j + radius];
                }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    kernel[i, j] /= norm;
        }

        public GaussianFilter()
        {
            createGaussianKernel(3, 2);
        }
    }

    class SobelFilter : MatrixFilter
    {
        float[,] kernelX = new float[,] { { -1, 0, 1 },
                                          { -2, 0, 2 },
                                          { 1, 0, -1 } };

        float[,] kernelY = new float[,] { { -1, -2, -1 },
                                          { 0, 0, 0 },
                                          { 1, 2, 1 } };


        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            int radiusX = kernelX.GetLength(0) / 2;
            int radiusY = kernelX.GetLength(1) / 2;
            float resultRx = 0;
            float resultGx = 0;
            float resultBx = 0;
            float resultRy = 0;
            float resultGy = 0;
            float resultBy = 0;
            for (int l = -radiusX; l <= radiusX; l++)
            {
                for (int k = -radiusY; k <= radiusY; k++)
                {
                    int idX = Clamp(x + k, 0, sourseImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourseImage.Height - 1);
                    Color neighbotColor = sourseImage.GetPixel(idX, idY);
                    resultRx += neighbotColor.R * kernelX[k + radiusX, l + radiusY];
                    resultGx += neighbotColor.G * kernelX[k + radiusX, l + radiusY];
                    resultBx += neighbotColor.B * kernelX[k + radiusX, l + radiusY];
                    resultRy += neighbotColor.R * kernelY[k + radiusX, l + radiusY];
                    resultGy += neighbotColor.G * kernelY[k + radiusX, l + radiusY];
                    resultBy += neighbotColor.B * kernelY[k + radiusX, l + radiusY];
                }
            }
            return Color.FromArgb(Clamp((int)Math.Sqrt(resultRx * resultRx + resultRy * resultRy), 0, 255),
                                  Clamp((int)Math.Sqrt(resultRx * resultRx + resultRy * resultRy), 0, 255),
                                  Clamp((int)Math.Sqrt(resultBx * resultBx + resultBy * resultBy), 0, 255));
        }

    }


    class EmbossingFilter : MatrixFilter
    {
        public EmbossingFilter()
        {
            kernel = new float[3, 3] { { 0, 1, 0 }, { 1, 0, -1 }, { 0, -1, 0 } };

            int norm = 2;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)

                    kernel[i, j] /= norm;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int c = 25;

            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            double resultR = 0;
            double resultG = 0;
            double resultB = 0;
            for (int l = -radiusY; l <= radiusY; l++)
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    resultR += neighborColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += neighborColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += neighborColor.B * kernel[k + radiusX, l + radiusY];
                }
            return Color.FromArgb(
                Clamp((int)resultR + c, 0, 255),
                Clamp((int)resultG + c, 0, 255),
                Clamp((int)resultB + c, 0, 255));

        }
    }


    class MotionBlur : MatrixFilter
    {
        public void createMotionBlurKernel(int radius)
        {
            int size = 2 * radius + 1;
            kernel = new float[size, size];
            float norm = (1f / size);
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    if (i == j)
                        kernel[i + radius, j + radius] = norm;
                    else
                        kernel[i + radius, j + radius] = 0;

                }
            }

        }
        public MotionBlur(int radius)
        {
            createMotionBlurKernel(radius);
        }
    }


    class SharrFilter :MatrixFilter
    {
        float[,] kernelX = new float[,] { { 3, 0, -3 },
                                  { 10, 0, -10 },
                                  { 3, 0, -3 } };

        float[,] kernelY = new float[,] { { 3, 10, 3 },
                                 { 0, 0, 0 },
                                 { -3, -10, -3 } };


        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            int radiusX = kernelX.GetLength(0) / 2;
            int radiusY = kernelX.GetLength(1) / 2;
            float resultRx = 0;
            float resultGx = 0;
            float resultBx = 0;
            float resultRy = 0;
            float resultGy = 0;
            float resultBy = 0;
            for (int l = -radiusY; l <= radiusY; l++)
            {
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, sourseImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourseImage.Height - 1);
                    Color neighbotColor = sourseImage.GetPixel(idX, idY);
                    resultRx += neighbotColor.R * kernelX[k + radiusX, l + radiusY];
                    resultGx += neighbotColor.G * kernelX[k + radiusX, l + radiusY];
                    resultBx += neighbotColor.B * kernelX[k + radiusX, l + radiusY];
                    resultRy += neighbotColor.R * kernelY[k + radiusX, l + radiusY];
                    resultGy += neighbotColor.G * kernelY[k + radiusX, l + radiusY];
                    resultBy += neighbotColor.B * kernelY[k + radiusX, l + radiusY];
                }
            }
            return Color.FromArgb(Clamp((int)Math.Sqrt(resultRx * resultRx + resultRy * resultRy), 0, 255),
                                  Clamp((int)Math.Sqrt(resultRx * resultRx + resultRy * resultRy), 0, 255),
                                  Clamp((int)Math.Sqrt(resultBx * resultBx + resultBy * resultBy), 0, 255));
        }
    }
}
