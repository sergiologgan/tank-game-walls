using GameTank.entity;
using GameTank.events;
using GameTank.events.tanque;
using GameTank.screen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTank
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();           

            this.parede = new Parede(4, 4, new Size(8, 8), 10);
            var paredes = this.parede.GetBlocks();
            this.parede.EspalharParedes(this.Size, paredes);
            this.tanque = new Tanque(new Point(0, 0), new Size(8, 8), Direction.down, this.parede, this._struct);
            this._struct = new Struct(this.Size, new Size(8, 8), new SolidBrush(Color.Red), Directions.All);
            this.ResizeRedraw = true;
            this.Invalidate();
        }

        private Tanque tanque;
        private Parede parede;
        private Struct _struct;
        private Keys keys;
        private float PenWidth = 8;

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.DoubleBuffered = true;
            this.tanque.RefreshTanque(e.Graphics);
            this.parede.AtualizarBlocos(e.Graphics);
            this._struct.RefreshBorder(e.Graphics, this.ClientRectangle);


            // Set up string. 
            string measureString = "Measure String";
            Font stringFont = new Font("Arial", 16);

            // Measure string.
            SizeF stringSize = new SizeF();
            stringSize = e.Graphics.MeasureString(measureString, stringFont);

            // Draw rectangle representing size of string.
            e.Graphics.DrawRectangle(new Pen(Color.Red, 1), 0.0F, 0.0F, stringSize.Width, stringSize.Height);

            // Draw string to screen.
            e.Graphics.DrawString(measureString, stringFont, Brushes.Black, new PointF(0, 0));


            var rect = new Rectangle(10, 10, 32, 32);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillEllipse(Brushes.White, rect);
            TextRenderer.DrawText(e.Graphics, "100", this.Font, rect, Color.Black,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            Pen myPen = new Pen(Color.Black, (float)0.05);
            e.Graphics.DrawLine(myPen, 0, 0, 10, 10);
            e.Graphics.DrawRectangle(myPen, 10, 10, 45, 5);
            e.Graphics.DrawLine(myPen, 55, 10, 70, 0);

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Invalidate();            
            switch (keys)
            {
                case Keys.W:
                    {
                        this.tanque.MoveTanque(8, Direction.up);
                    }
                    break;
                case Keys.S:
                    {
                        this.tanque.MoveTanque(8, Direction.down);
                    }
                    break;
                case Keys.D:
                    {
                        this.tanque.MoveTanque(8, Direction.right);
                    }
                    break;
                case Keys.A:
                    {
                        this.tanque.MoveTanque(8, Direction.left);
                    }
                    break;
                default:
                    break;
            }
            this.Text = $"{this.tanque.GetLocation().ToString()} >> {this.tanque.GetDirection()} >> {this.tanque.GetLastDirection()}";
        }      
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    this.keys = Keys.W;
                    break;
                case Keys.S:
                    this.keys = Keys.S;
                    break;
                case Keys.D:
                    this.keys = Keys.D;
                    break;
                case Keys.A:
                    this.keys = Keys.A;
                    break;
                default:
                    break;
            }
        }
    }
}
