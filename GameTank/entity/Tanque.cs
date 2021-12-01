using GameTank.entity.utils;
using GameTank.events;
using GameTank.events.tanque;
using GameTank.screen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameTank.entity
{
    public enum LastDirections { left, right, up, down, empty }
    public enum Direction { left, right, up, down, empty }
    public class Tanque
    {
        public Tanque(Point locationStart, Size size, Direction direction, Parede parede, Struct @struct)
        {
            this.size = size;
            this.parede = parede;
            this.rectangles = new Dictionary<string, Rectangle[]>();
            this.direction = direction;
            this.CreateTanque();
            this.@struct = @struct;
            for (int i = 0; i < this.rectangles.Count; i++)
            {
                switch (direction)
                {
                    case Direction.left:
                        {
                            this.rectangles["left"][i].X -= locationStart.X;
                            this.lastDirections = LastDirections.left;
                        }
                        break;
                    case Direction.right:
                        {
                            this.rectangles["right"][i].X += locationStart.X;
                            this.lastDirections = LastDirections.right;
                        }
                        break;
                    case Direction.up:
                        {
                            this.rectangles["up"][i].Y -= locationStart.Y;
                            this.lastDirections = LastDirections.up;
                        }
                        break;
                    case Direction.down:
                        {
                            this.rectangles["down"][i].Y += locationStart.Y;
                            this.lastDirections = LastDirections.down;
                        }
                        break;
                    default:
                        break;
                }
            }
        
            this.location = locationStart;
            this.intersectParede = new IntersectParede();
            this.handlerList = new HandlerList();
        }

        private Size size;
        private Point location;
        private Parede parede;
        private Dictionary<string, Rectangle[]> rectangles;
        private IntersectParede intersectParede;
        private HandlerList handlerList;
        private Struct @struct;
        private Direction direction = Direction.empty;      
        private LastDirections lastDirections = LastDirections.empty;

        private void CreateTanque()
        {
            this.rectangles.Add("up", new Rectangle[6] {

                new Rectangle(){ Size = this.size, X = this.size.Width*2, Y = this.size.Height },
                new Rectangle(){ Size = this.size, X = this.size.Width, Y = this.size.Height*2 },
                new Rectangle(){ Size = this.size, X = this.size.Width*2, Y = this.size.Height*2},
                new Rectangle(){ Size = this.size, X = this.size.Width*3, Y = this.size.Height*2 },
                new Rectangle(){ Size = this.size, X = this.size.Width, Y = this.size.Height*3},
                new Rectangle(){ Size = this.size, X = this.size.Width*3, Y = this.size.Height*3 }
            });
            this.rectangles.Add("down", new Rectangle[6] {

                new Rectangle(){ Size = this.size, X = this.size.Width, Y = this.size.Height },
                new Rectangle(){ Size = this.size, X = this.size.Width*3, Y = this.size.Height },
                new Rectangle(){ Size = this.size, X = this.size.Width, Y = this.size.Height*2 },
                new Rectangle(){ Size = this.size, X = this.size.Width*2, Y = this.size.Height*2},
                new Rectangle(){ Size = this.size, X = this.size.Width*3, Y = this.size.Height*2 },
                new Rectangle(){ Size = this.size, X = this.size.Width*2, Y = this.size.Height*3 },
            });

            this.rectangles.Add("left", new Rectangle[6] {

                new Rectangle(){ Size = this.size, X = this.size.Width*2, Y = this.size.Height },
                new Rectangle(){ Size = this.size, X = this.size.Width*3, Y = this.size.Height },
                new Rectangle(){ Size = this.size, X = this.size.Width, Y = this.size.Height*2 },
                new Rectangle(){ Size = this.size, X = this.size.Width*2, Y = this.size.Height*2},
                new Rectangle(){ Size = this.size, X = this.size.Width*2, Y = this.size.Height*3 },
                new Rectangle(){ Size = this.size, X = this.size.Width*3, Y = this.size.Height*3 }
            });

            this.rectangles.Add("right", new Rectangle[6] {

                new Rectangle(){ Size = this.size, X = this.size.Width, Y = this.size.Height },
                new Rectangle(){ Size = this.size, X = this.size.Width*2, Y = this.size.Height },
                new Rectangle(){ Size = this.size, X = this.size.Width*2, Y = this.size.Height*2},
                new Rectangle(){ Size = this.size, X = this.size.Width*3, Y = this.size.Height*2 },
                new Rectangle(){ Size = this.size, X = this.size.Width, Y = this.size.Height*3},
                new Rectangle(){ Size = this.size, X = this.size.Width*2, Y = this.size.Height*3 },
            });
        }
        public void RefreshTanque(Graphics graphics)
        {
            SolidBrush brush = new SolidBrush(Color.Blue);
            switch (this.direction)
            {
                case Direction.left:
                    graphics.FillRectangles(brush, this.rectangles["left"]);
                    break;
                case Direction.right:
                    graphics.FillRectangles(brush, this.rectangles["right"]);
                    break;
                case Direction.up:
                    graphics.FillRectangles(brush, this.rectangles["up"]);
                    break;
                case Direction.down:
                    graphics.FillRectangles(brush, this.rectangles["down"]);
                    break;
                default:
                    break;
            }
        }
        public void MoveTanque(int speed, Direction direction)
        {
            this.direction = direction;

            if (!this.IntersectarAll(speed) && this.Orientation(speed))
            {
                switch (direction)
                {
                    case Direction.left:
                        {
                            for (int i = 0; i < this.rectangles["left"].Length; i++)
                            {
                                this.rectangles["left"][i].X -= speed;
                                this.rectangles["right"][i].X -= speed;
                                this.rectangles["up"][i].X -= speed;
                                this.rectangles["down"][i].X -= speed;
                            }
                            this.location.X -= speed;
                        }
                        break;
                    case Direction.right:
                        {
                            for (int i = 0; i < this.rectangles["right"].Length; i++)
                            {
                                this.rectangles["right"][i].X += speed;
                                this.rectangles["left"][i].X += speed;
                                this.rectangles["up"][i].X += speed;
                                this.rectangles["down"][i].X += speed;
                            }
                            this.location.X += speed;
                        }
                        break;
                    case Direction.up:
                        {
                            for (int i = 0; i < this.rectangles["up"].Length; i++)
                            {
                                this.rectangles["up"][i].Y -= speed;
                                this.rectangles["down"][i].Y -= speed;
                                this.rectangles["left"][i].Y -= speed;
                                this.rectangles["right"][i].Y -= speed;
                            }
                            this.location.Y -= speed;
                        }
                        break;
                    case Direction.down:
                        {
                            for (int i = 0; i < this.rectangles["down"].Length; i++)
                            {
                                this.rectangles["down"][i].Y += speed;
                                this.rectangles["up"][i].Y += speed;
                                this.rectangles["left"][i].Y += speed;
                                this.rectangles["right"][i].Y += speed;
                            }
                            this.location.Y += speed;
                        }
                        break;
                    default:
                        break;
                }
            }
            switch (this.direction)
            {
                case Direction.left:
                    this.lastDirections = LastDirections.left;
                    break;
                case Direction.right:
                    this.lastDirections = LastDirections.right;
                    break;
                case Direction.up:
                    this.lastDirections = LastDirections.up;
                    break;
                case Direction.down:
                    this.lastDirections = LastDirections.down;
                    break;
                default:
                    break;
            }
        }
        private bool Orientation(int speed)
        {
            switch (this.direction)
            {
                case Direction.left:
                    {
                        return this.lastDirections.Equals(LastDirections.left);
                    }
                case Direction.right:
                    {
                        return this.lastDirections.Equals(LastDirections.right);
                    }
                case Direction.up:
                    {
                        return this.lastDirections.Equals(LastDirections.up);
                    }
                case Direction.down:
                    {
                        return this.lastDirections.Equals(LastDirections.down);
                    }
                case Direction.empty:                   
                    return false;
                default:
                    return false;
            }
        }
        private bool IntersectarAll(int speed)
        {
            bool left = this.IntersectInLeft(speed);
            bool right = this.IntersectInRight(speed);
            bool up = this.IntersectInUp(speed);
            bool down = this.IntersectInDown(speed);
            switch (this.direction)
            {
                case Direction.left:
                    {
                        switch (this.lastDirections)
                        {
                            case LastDirections.up:
                                {
                                    if (!left && up)
                                    {
                                        for (int i = 0; i < this.rectangles["right"].Length; i++)
                                        {
                                            this.rectangles["right"][i].X -= speed;
                                            this.rectangles["left"][i].X -= speed;
                                            this.rectangles["up"][i].X -= speed;
                                            this.rectangles["down"][i].X -= speed;
                                        }
                                    }
                                    else if (left && right)
                                    {
                                        this.direction = Direction.up;
                                    }
                                }
                                break;
                            case LastDirections.down:
                                {
                                    if (!left && down)
                                    {
                                        for (int i = 0; i < this.rectangles["right"].Length; i++)
                                        {
                                            this.rectangles["right"][i].X -= speed;
                                            this.rectangles["left"][i].X -= speed;
                                            this.rectangles["up"][i].X -= speed;
                                            this.rectangles["down"][i].X -= speed;
                                        }
                                    }
                                    else if (left && right)
                                    {
                                        this.direction = Direction.down;
                                    }
                                }
                                break;
                            case LastDirections.right:
                                {
                                    if (!left && right && (up || down))
                                    {
                                        for (int i = 0; i < this.rectangles["left"].Length; i++)
                                        {
                                            this.rectangles["right"][i].X -= speed;
                                            this.rectangles["left"][i].X -= speed;
                                            this.rectangles["up"][i].X -= speed;
                                            this.rectangles["down"][i].X -= speed;
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }           
                    }
                    return left;
                case Direction.right:
                    {
                        switch (this.lastDirections)
                        {
                            case LastDirections.up:
                                {
                                    if (!right && up)
                                    {
                                        for (int i = 0; i < this.rectangles["right"].Length; i++)
                                        {
                                            this.rectangles["right"][i].X += speed;
                                            this.rectangles["left"][i].X += speed;
                                            this.rectangles["up"][i].X += speed;
                                            this.rectangles["down"][i].X += speed;
                                        }
                                    }
                                    else if (left && right)
                                    {
                                        this.direction = Direction.up;
                                    }
                                }
                                break;
                            case LastDirections.down:
                                {
                                    if (!right && down)
                                    {
                                        for (int i = 0; i < this.rectangles["right"].Length; i++)
                                        {
                                            this.rectangles["right"][i].X += speed;
                                            this.rectangles["left"][i].X += speed;
                                            this.rectangles["up"][i].X += speed;
                                            this.rectangles["down"][i].X += speed;
                                        }
                                    }
                                    else if (left && right)
                                    {
                                        this.direction = Direction.down;
                                    }
                                }
                                break;
                            case LastDirections.left:
                                {
                                    if (!right && left && (up||down))
                                    {
                                        for (int i = 0; i < this.rectangles["right"].Length; i++)
                                        {
                                            this.rectangles["right"][i].X += speed;
                                            this.rectangles["left"][i].X += speed;
                                            this.rectangles["up"][i].X += speed;
                                            this.rectangles["down"][i].X += speed;
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }                        
                    }
                    return right;
                case Direction.up:
                    {
                        switch (this.lastDirections)
                        {
                            case LastDirections.left:
                                {
                                    if (!up && left && down)
                                    {
                                        for (int i = 0; i < this.rectangles["left"].Length; i++)
                                        {
                                            this.rectangles["right"][i].Y -= speed;
                                            this.rectangles["left"][i].Y -= speed;
                                            this.rectangles["up"][i].Y -= speed;
                                            this.rectangles["down"][i].Y -= speed;
                                        }
                                    }   
                                    else if (up && down)
                                    {
                                        this.direction = Direction.left;
                                    }
                                }
                                break;
                            case LastDirections.right:
                                {
                                    if (!up && right && down)
                                    {
                                        for (int i = 0; i < this.rectangles["left"].Length; i++)
                                        {
                                            this.rectangles["right"][i].Y -= speed;
                                            this.rectangles["left"][i].Y -= speed;
                                            this.rectangles["up"][i].Y -= speed;
                                            this.rectangles["down"][i].Y -= speed;
                                        }
                                    }                                    
                                    else if (up && down)
                                    {
                                        this.direction = Direction.right;
                                    }
                                }
                                break;
                            case LastDirections.down:
                                {
                                    if (!up && down && (left||right))
                                    {
                                        for (int i = 0; i < this.rectangles["left"].Length; i++)
                                        {
                                            this.rectangles["right"][i].Y -= speed;
                                            this.rectangles["left"][i].Y -= speed;
                                            this.rectangles["up"][i].Y -= speed;
                                            this.rectangles["down"][i].Y -= speed;
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    return up;
                case Direction.down:
                    {
                        switch (this.lastDirections)
                        {
                            case LastDirections.left:
                                {
                                    if (!down && left && up)
                                    {
                                        for (int i = 0; i < this.rectangles["left"].Length; i++)
                                        {
                                            this.rectangles["right"][i].Y += speed;
                                            this.rectangles["left"][i].Y += speed;
                                            this.rectangles["up"][i].Y += speed;
                                            this.rectangles["down"][i].Y += speed;
                                        }
                                    }
                                    else if (up && down)
                                    {
                                        this.direction = Direction.left;
                                    }
                                }
                                break;
                            case LastDirections.right:
                                {
                                    if (!down && right && up)
                                    {
                                        for (int i = 0; i < this.rectangles["left"].Length; i++)
                                        {
                                            this.rectangles["right"][i].Y += speed;
                                            this.rectangles["left"][i].Y += speed;
                                            this.rectangles["up"][i].Y += speed;
                                            this.rectangles["down"][i].Y += speed;
                                        }
                                    }
                                    else if (up && down)
                                    {
                                        this.direction = Direction.right;
                                    }
                                }
                                break;
                            case LastDirections.up:
                                {
                                    if (up && !down && (left||right))
                                    {
                                        for (int i = 0; i < this.rectangles["left"].Length; i++)
                                        {
                                            this.rectangles["right"][i].Y += speed;
                                            this.rectangles["left"][i].Y += speed;
                                            this.rectangles["up"][i].Y += speed;
                                            this.rectangles["down"][i].Y += speed;
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    return down;
                default:
                    break;
            }
            return false;
        }
        private bool IntersectInLeft(int speed)
        {
            Point location = new Point();
            for (int i = 0; i < this.rectangles["left"].Length; i++)
            {
                location = this.rectangles["left"][i].Location;
                this.rectangles["left"][i].X -= speed;
                for (int j = 0; j < this.parede.GetRectangles().Length; j++)
                {
                    if (this.rectangles["left"][i].IntersectsWith(this.parede.GetRectangles()[j]))
                    {
                        this.rectangles["left"][i].Location = location;
                        this.intersectParede.Direction = Intersect.Down;
                        this.handlerList.InvokeMethod(BindingFlags.Public | BindingFlags.NonPublic, new Type[1] { typeof(IntersectParede) }, new object[] { intersectParede });
                        return true;
                    }
                }
                this.rectangles["left"][i].Location = location;
            }
            return false;
        }
        private bool IntersectInRight(int speed)
        {
            Point location = new Point();
            for (int i = 0; i < this.rectangles["right"].Length; i++)
            {
                location = this.rectangles["right"][i].Location;
                this.rectangles["right"][i].X += speed;
                for (int j = 0; j < this.parede.GetRectangles().Length; j++)
                {
                    if (this.rectangles["right"][i].IntersectsWith(this.parede.GetRectangles()[j]))
                    {
                        this.rectangles["right"][i].Location = location;
                        this.intersectParede.Direction = Intersect.Right;
                        this.handlerList.InvokeMethod(BindingFlags.Public | BindingFlags.NonPublic, new Type[1] { typeof(IntersectParede) }, new object[] { intersectParede });

                        return true;
                    }
                }
                this.rectangles["right"][i].Location = location;
            }
            return false;
        }
        private bool IntersectInUp(int speed)
        {
            Point location = new Point();
            for (int i = 0; i < this.rectangles["up"].Length; i++)
            {
                location = this.rectangles["up"][i].Location;
                this.rectangles["up"][i].Y -= speed;
                for (int j = 0; j < this.parede.GetRectangles().Length; j++)
                {
                    if (this.rectangles["up"][i].IntersectsWith(this.parede.GetRectangles()[j]))
                    {
                        this.rectangles["up"][i].Location = location;
                        this.intersectParede.Direction = Intersect.Up;
                        this.handlerList.InvokeMethod(BindingFlags.Public | BindingFlags.NonPublic, new Type[1] { typeof(IntersectParede) }, new object[] { intersectParede });
                        return true;
                    }
                }
                this.rectangles["up"][i].Location = location;
            }
            return false;
        }
        private bool IntersectInDown(int speed)
        {
            Point location = new Point();
            for (int i = 0; i < this.rectangles["down"].Length; i++)
            {
                location = this.rectangles["down"][i].Location;
                this.rectangles["down"][i].Y += speed;
                for (int j = 0; j < this.parede.GetRectangles().Length; j++)
                {
                    if (this.rectangles["down"][i].IntersectsWith(this.parede.GetRectangles()[j]))
                    {
                        this.rectangles["down"][i].Location = location;
                        this.intersectParede.Direction = Intersect.Down;
                        this.handlerList.InvokeMethod(BindingFlags.Public | BindingFlags.NonPublic, new Type[1] { typeof(IntersectParede) }, new object[] { intersectParede });
                        return true;
                    }
                }
                this.rectangles["down"][i].Location = location;
            }
            return false;
        }
        private bool IntersectInStruct(int speed)
        {
            Point location = new Point();

            location = this.rectangles["struct"][i].Location;
            this.rectangles["struct"][i].Y += speed;
            for (int j = 0; j < this.parede.GetRectangles().Length; j++)
            {
                if (this.rectangles["struct"][i].IntersectsWith(this.parede.GetRectangles()[j]))
                {
                    this.rectangles["struct"][i].Location = location;
                    this.intersectParede.Direction = Intersect.Down;
                    this.handlerList.InvokeMethod(BindingFlags.Public | BindingFlags.NonPublic, new Type[1] { typeof(IntersectParede) }, new object[] { intersectParede });
                    return true;
                }
            }
            this.rectangles["struct"][i].Location = location;

            return false;
        }
        public Point GetLocation()
        {
            return this.location;
        }
        public string GetDirection()
        {
            return this.direction.ToString();
        }
        public Direction GetViewDirection()
        {
            return this.direction;
        }
        public string GetLastDirection()
        {
            return this.lastDirections.ToString();
        }
    }
}
