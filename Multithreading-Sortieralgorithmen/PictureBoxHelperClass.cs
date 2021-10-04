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
        static Color[] colors = new Color[4] { Color.Purple, Color.Orange, Color.SaddleBrown, Color.CornflowerBlue };
        public static int[] Pivots { get; set; } = new int[0]; 
        public static PictureBox PictureBox { get; set;}
        public static double[] Array { get; set; }
        public static ValuesSwitchedEventArgs E { get; set; }
        public static bool BigUpdate { get; set; } = true;
        public static float PenWidth { get; private set; }

        public static UpdatePictureBox DecidePictureBoxUpdate()
        {
            PenWidth = (int)((float)PictureBox.Width / (float)Array.Length + 1);

            if (BigUpdate)
                return PictureBoxRePaint;

            return PictureBoxValuesSwitched;
        }

        public static void PictureBoxValuesSwitched()
        {
            if (E != null)
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

                Pen pen = new Pen(PivotToColor(Pivots, i), PenWidth);
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
            using (Pen penWhite = new Pen(Color.White, PenWidth))
            {
                int x = (int)(((double)pictureBox.Width / array.Length) * index1);
                g.DrawLine(penWhite, new Point(x, pictureBox.Height), new Point(x, 0));

                x = (int)(((double)pictureBox.Width / array.Length) * index2);
                g.DrawLine(penWhite, new Point(x, pictureBox.Height), new Point(x, 0));
            }

            //Draw new Lines at index1 and index2
            using (Pen penBlack = new Pen(PivotToColor(Pivots, index1), PenWidth))
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

        private static Color PivotToColor(int[] pivots, int pivot)
        {
            for (int i = 1; i < pivots.Length; i++)
                if (pivots[i - 1] <= pivot && pivot <= pivots[i])
                    return colors[i - 1];

            return Color.Black;
        }
    }
}
