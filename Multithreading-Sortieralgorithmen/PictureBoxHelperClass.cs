using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multithreading_Sortieralgorithmen
{
    class PictureBoxHelperClass
    {
        public delegate void UpdatePictureBox();

        public static PictureBox PictureBox { get; set;}
        public static double[] Array { get; set; }

        public static ValuesSwitchedEventArgs E { get; set; }

        public static bool BigUpdate { get; set; } = true;

        public static UpdatePictureBox DecidePictureBoxUpdate()
        {
            if (BigUpdate)
                return PictureBoxRePaint;
            else
                return PictureBoxValuesSwitched;
        }

        public static void PictureBoxValuesSwitched()
        {
            UpdateSamples(PictureBox, Array, E.Index1, E.Index2, E.Value1, E.Value2);
        }

        public static void PictureBoxRePaint()
        {
            DrawSamples(PictureBox, Array);
        }

        private static void DrawSamples(PictureBox pictureBox, double[] array)
        {
            Bitmap bm = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics g = Graphics.FromImage(bm);

            g.Clear(Color.White);

            for (int i = 0; i < array.Length; i++)
            {
                int x = (int)(((double)pictureBox.Width / array.Length) * i);

                Pen pen = new Pen(Color.Black);
                Point one = new Point(x, pictureBox.Height);
                Point two = new Point(x, (int)(pictureBox.Height - (int)((array[i] / (double)int.MaxValue) * pictureBox.Height)));
                g.DrawLine(pen, one, two);
            }

            pictureBox.Image = bm;
        }

        private static void UpdateSamples(PictureBox pictureBox, double[] array, int index1, int index2, double value1, double value2)
        {
            Bitmap bm = new Bitmap(pictureBox.Image);
            Graphics g = Graphics.FromImage(bm);

            //Clear the Bitmap at index1 and index2
            using (Pen penWhite = new Pen(Color.White))
            {
                int x = (int)(((double)pictureBox.Width / array.Length) * index1);
                g.DrawLine(penWhite, new Point(x, pictureBox.Height), new Point(x, 0));

                x = (int)(((double)pictureBox.Width / array.Length) * index2);
                g.DrawLine(penWhite, new Point(x, pictureBox.Height), new Point(x, 0));
            }

            //Draw new Lines at index1 and index2
            using (Pen penBlack = new Pen(E.Color))
            {
                int x = (int)(((double)pictureBox.Width / array.Length) * index1);
                g.DrawLine(
                       penBlack,
                       new Point(x, pictureBox.Height),
                       new Point(x, (int)(pictureBox.Height - (int)((value1 / (double)int.MaxValue) * pictureBox.Height)))
                       );

                x = (int)(((double)pictureBox.Width / array.Length) * index2);
                g.DrawLine(
                        penBlack,
                        new Point(x, pictureBox.Height),
                        new Point(x, (int)(pictureBox.Height - (int)((value2 / (double)int.MaxValue) * pictureBox.Height)))
                        );
            }

            pictureBox.Image = bm;
        }



    }
}
