using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace lab1_filters
{ 
class MyColor : IComparable
{
    public int R;
    public int G;
    public int B;
    public double I;
    public MyColor(int R, int G, int B)
    {
        this.R = R;
        this.G = G;
        this.B = B;
        this.I = 0.36 * R + 0.53 * G + 0.11 * B;
    }
    public int CompareTo(object obj)
    {
        MyColor ob = (MyColor)obj;
        if (I > ob.I)
            return 1;
        if (I == ob.I)
            return 0;
        return -1;
    }
}


class MedianFilter : Filters
{
    private Color medianColor;
    private int size;

    public MedianFilter(int size = 3)
    {
        this.size = size;
    }
    protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
    {
        return medianColor;
    }

    override public Bitmap processImage(Bitmap sourseImage, BackgroundWorker worker)
    {
        Bitmap resultImage = new Bitmap(sourseImage.Width, sourseImage.Height);

        var arrayColor = new List<MyColor>();
        int R, G, B;
        MyColor color;

        for (int y = size / 2; y < sourseImage.Height - size / 2; y++)
        {
                worker.ReportProgress((int)((float)y/ resultImage.Height * 100));
                if (worker.CancellationPending)
                    return null;

                for (int x = size / 2; x < sourseImage.Width - size / 2; x++)
            {
                arrayColor.Clear();

                for (int j = -size / 2; j <= size / 2; j++)
                {
                    for (int i = -size / 2; i <= size / 2; i++)
                    {
                        R = sourseImage.GetPixel(x + i, y + j).R;
                        G = sourseImage.GetPixel(x + i, y + j).G;
                        B = sourseImage.GetPixel(x + i, y + j).B;
                        color = new MyColor(R, G, B);
                        arrayColor.Add(color);
                    }
                }

                arrayColor.Sort();

                R = arrayColor.ElementAt(size * size / 2).R;
                G = arrayColor.ElementAt(size * size / 2).G;
                B = arrayColor.ElementAt(size * size / 2).B;
                medianColor = Color.FromArgb(R, G, B);

                resultImage.SetPixel(x - size / 2, y - size / 2, calculateNewPixelColor(sourseImage, x - size / 2, y - size / 2));
            }
        }
        return resultImage;

    }
}

}