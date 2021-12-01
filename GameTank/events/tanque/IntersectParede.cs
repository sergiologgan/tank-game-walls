using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTank.events.tanque
{
    public enum Intersect { Left, Right, Up, Down }
    public class IntersectParede
    {
        public bool IsIntersect { get; set; }

        
        public Intersect Direction { get; set; }

        public void GetParede()
        {

        }

        public Point PointIntersect()
        {
            return new Point();
        }

    }
}
