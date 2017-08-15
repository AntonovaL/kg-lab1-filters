using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;


    namespace lab1_filters
    {
 class ColorCorrection: Filters
    {
        /*public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }*/

        double MeRs = 0; double DRs = 0;   //мат ожидание и дисперсия (1)-для исходного изображения (2)-для источника цвета
        double MeRt = 0; double DRt = 0;   //в lab
        double MeGs = 0; double DGs = 0;
        double MeGt = 0; double DGt = 0;
        double MeBs = 0; double DBs = 0;
        double MeBt = 0; double DBt = 0;

       public ColorCorrection(Bitmap Source, Bitmap Target, BackgroundWorker worker)
        {
            Bitmap result = processImage2(Source, Target, worker);
        }

            int ns, nt;  //размеры изображений
            void CalculateMe(Bitmap source, Bitmap target )
            {
                ns = source.Width * source.Height;
                nt = target.Width * target.Height;
            Color tC, sC; 
                for (int i = 0; i < source.Height; i++)
                {
                    for (int j = 0; j < source.Width; j++)
                    {
                    sC = source.GetPixel(j, i);
                    MeRs += sC.R;
                    MeGs += sC.G;
                    MeBs += sC.B;
                    }

                }
                for (int i = 0; i < target.Height; i++)
                {
                    for (int j = 0; j < target.Width; j++)
                    {
                    tC = target.GetPixel(j, i);
                    MeRt += tC.R;
                    MeGt += tC.G;
                    MeBt += tC.B;
                    }
                }

            MeRs = MeRs / ns;
            MeRt = MeRt / nt;
            MeGs = MeGs / ns;
            MeGt = MeGt / nt;
            MeBs = MeBs / ns;
                MeBt = MeBt / nt;

            }

            void CalculateD(Bitmap source, Bitmap target)
            {
            Color tC, sC;

                for (int i = 0; i < source.Width; i++)
                {
                    for (int j = 0; j < source.Height; j++)
                    {
                      sC = source.GetPixel(i, j);
                        DRs += (sC.R - MeRs) * (sC.R - MeRs);
                        DGs += (sC.G - MeGs) * (sC.G - MeGs);
                        DBs += (sC.B - MeBs) * (sC.B - MeBs);
                    }

                }
                for (int i = 0; i < target.Width; i++)
                {
                    for (int j = 0; j < target.Height; j++)
                    {
                    tC = target.GetPixel(i, j);
                        DRt += (tC.R - MeRt) * (tC.R - MeRt);
                        DGt += (tC.G - MeGt) * (tC.G - MeGt);
                        DBt += (tC.B - MeBt) * (tC.B - MeBt);
                     
                    }
                }
                DRs = Math.Sqrt(DRs / ns);
                DRt = Math.Sqrt(DRt / nt);
                DGs = Math.Sqrt(DGs / ns);
                DGt = Math.Sqrt(DGt / nt);
            DBs = Math.Sqrt(DBs / ns);
            DBt = Math.Sqrt(DBt / nt);

        }

    

       



            protected override  Color calculateNewPixelColor(Bitmap Target, int x, int y)
            {

                double R;
                double G;
                double B;
               R = Target.GetPixel(x, y).R;
                G = Target.GetPixel(x, y).G;
                B = Target.GetPixel(x, y).B;
                 R = MeRs + (R - MeRt) * DRs / DRt;
                G= MeGs + (G - MeGt) * DGs / DGt;
                B= MeBs + (B - MeBt) * DBs / DBt;


            return Color.FromArgb(Clamp((int)R, 0,255), Clamp((int)G, 0,255), Clamp((int)B, 0,255));
            }

            public Bitmap processImage2(Bitmap Target, Bitmap Source,  BackgroundWorker worker)
            {
                Bitmap resultImage = new Bitmap(Target.Width, Target.Height);
                CalculateMe(Source, Target);
                CalculateD(Source, Target);
                for (int i = 0; i < Target.Width; i++)
                {
                    worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                    if (worker.CancellationPending)
                        return null;
                    for (int j = 0; j < Target.Height; j++)
                    {
                        resultImage.SetPixel(i, j, calculateNewPixelColor(Target, i, j));
                    }
                }
                return resultImage;

            }
        }
    }
