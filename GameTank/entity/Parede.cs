using GameTank.entity.utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTank.entity
{
    public class Parede
    {
        public Parede(int countHorizontal, int countVertical, Size size, int lenght)
        {
            int y = 0;
            int cout = 0;
            this.size = size;
            List<Block> blocks = new List<Block>();
            var _rectangles = new List<Rectangle>();

            for (int j = 0; j < lenght; j++)
            {
                for (int i = 0; i < countHorizontal * countVertical; i++)
                {
                    y = i >= countHorizontal ? y += 1 : y;
                    cout++;
                    if (i >= countHorizontal)
                    {
                        i = 0;
                        _rectangles.Add(new Rectangle() { Size = size, X = i * size.Width, Y = y * size.Height });
                    }
                    else
                    {
                        _rectangles.Add(new Rectangle() { Size = size, X = i * size.Width, Y = y * size.Height });
                    }
                    if (cout >= (countHorizontal * countVertical))
                    {
                        break;
                    }
                }
                Block block = new Block();
                block.ConjuntoDeParedes = _rectangles.ToArray();
                block.Location = new Point();
                block.ExecuteDistortion(100);
                blocks.Add(block);
            }
            this.blocks = blocks;
        }

        private Size size;
        private List<Block> blocks;
        public void AtualizarBlocos(Graphics graphics)
        {
            SolidBrush pincel = new SolidBrush(Color.Black);
            for (int i = 0; i < this.blocks.Count; i++)
            {
                graphics.FillRectangles(pincel, this.blocks[i].ConjuntoDeParedes);
            }
        }
        public void EspalharParedes(Size sizeScreen, List<Block> blocks)
        {
            Random r = new Random();
            List<Rectangle> lRect = new List<Rectangle>();
            int linhaY = 0;
            int linhaX = 0;

            for (int i = 0; i < blocks.Count; i++)
            {
                linhaY = r.Next(0, sizeScreen.Height);
                linhaX = r.Next(0, sizeScreen.Width);
                while (linhaX % this.size.Width != 0 || linhaY % this.size.Height != 0)
                {
                    linhaY = r.Next(0, sizeScreen.Height);
                    linhaX = r.Next(0, sizeScreen.Width);
                }
                blocks[i].Location = new Point(linhaX, linhaY);
                for (int j = 0; j < blocks[i].ConjuntoDeParedes.Length; j++)
                {
                    lRect.Add(blocks[i].ConjuntoDeParedes[j]);
                }
            }
            this.blocks = blocks;
            this.rectangles = lRect.ToArray();
        }
        public List<Block> GetBlocks()
        {            
            return this.blocks;
        }

        private Rectangle[] rectangles;
        public Rectangle[] GetRectangles()
        {
            return this.rectangles;
        }
    }
}
