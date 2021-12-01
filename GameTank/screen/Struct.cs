using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTank.screen
{
    public enum Directions { Up, Down, Left, Right, All}
    public class Struct
    {
        public Struct(Size sizeScreen, Size sizeRectangles, SolidBrush brush, Directions directions)
        {
            this.sizeRectangles = sizeRectangles;
            this.sizeScreen = sizeScreen;
            this.brush = brush;
            this.directions = directions;
            this.dicRectangles = new Dictionary<string, Rectangle[]>();
            this.fill = new List<Rectangle>();
        }

        private List<Rectangle> fill;
        private Size sizeScreen;
        private Size sizeRectangles;
        private SolidBrush brush;
        private Directions directions;
        private float PenWidth = 8;

        public void RefreshBorder(Graphics graphics, Rectangle clientRectangle)
        {
            using (var pen = new Pen(Color.Red, PenWidth))
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                graphics.DrawRectangle(pen, clientRectangle);
            }
        }

        private Dictionary<string, Rectangle[]> dicRectangles;

        public Rectangle[] GetRectangles(string key)
        {
            if (this.dicRectangles.ContainsKey(key))
            {
                return this.dicRectangles[key];
            }
            return null;
        }

        public Rectangle[] GetAllRectangles()
        {
            return this.dicRectangles.Values.SelectMany(x => x).ToArray();
        }
    }
}
