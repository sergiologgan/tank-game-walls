using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTank.events.tanque
{
    public class ShotIntersect
    {
        private bool isIntersectParede;

        public bool IsIntesectParede
        {
            get { return isIntersectParede; }
            set { isIntersectParede = value; }
        }


        private bool isIntersectTanque;

        public bool IsIntersectTanque
        {
            get { return isIntersectTanque; }
            set { isIntersectTanque = value; }
        }

        public void GetParede()
        {

        }

        public void GetTanque()
        {

        }

        public Point GetPoint()
        {
            return new Point();
        }

    }
}
