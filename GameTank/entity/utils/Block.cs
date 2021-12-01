using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTank.entity.utils
{
    public class Block
    {
        private Point location;
        public Point Location
        {
            get { return this.location; }
            set
            {
                for (int i = 0; i < this.ConjuntoDeParedes.Length; i++)
                {
                    this.ConjuntoDeParedes[i].X += value.X;
                    this.ConjuntoDeParedes[i].Y += value.Y;
                }
                this.location = value;
            }
        }
        public Rectangle[] ConjuntoDeParedes { get; set; }

        public void ExecuteDistortion(int porcentagem)
        {
            Random r = new Random();
            List<int> numbers = new List<int>();
            int random = 0;
            int p = (porcentagem * this.ConjuntoDeParedes.Length)/ 100;

            for (int i = 0; i < p; i++)
            {
                p--;
                random = r.Next(0, this.ConjuntoDeParedes.Length);
                while (numbers.Contains(random))
                {
                    random = r.Next(0, this.ConjuntoDeParedes.Length);
                }
                numbers.Add(random);
                List<Rectangle> list = this.ConjuntoDeParedes.ToList();
                list.RemoveAt(random);
                this.ConjuntoDeParedes = list.ToArray();
            }
        }
    }
}
