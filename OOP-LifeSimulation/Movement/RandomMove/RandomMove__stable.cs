using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    class RandomMove__stable
    {
        private int offsetX = 999;
        private int offsetY = 999;

        public Point CheckNearCells(int x, int y, Map map)
        {
            if (offsetX == 999)
            {
                int direction = new Random().Next(0, 4);
                switch (direction)
                {
                    case 0:
                        if (map.IsAvailable(x - 1, y))
                        {
                            offsetX = -1;
                            offsetY = 0;
                            return new Point(x - 1, y);
                        }
                        break;
                    case 1:
                        if (map.IsAvailable(x + 1, y))
                        {
                            offsetX = 1;
                            offsetY = 0;
                            return new Point(x + 1, y);
                        }
                        break;
                    case 2:
                        if (map.IsAvailable(x, y - 1))
                        {
                            offsetX = 0;
                            offsetY = -1;
                            return new Point(x, y - 1);
                        }
                        break;
                    case 3:
                        if (map.IsAvailable(x, y + 1))
                        {
                            offsetX = 0;
                            offsetY = 1;
                            return new Point(x, y + 1);
                        }
                        break;
                }
                return new Point(x, y);
            }
            else
            {
                int direction = new Random().Next(0, 8);
                switch (direction)
                {
                    case 0:
                        if (map.IsAvailable(x - 1, y))
                        {
                            offsetX = -1;
                            offsetY = 0;
                            return new Point(x - 1, y);
                        }
                        break;
                    case 1:
                        if (map.IsAvailable(x + 1, y))
                        {
                            offsetX = 1;
                            offsetY = 0;
                            return new Point(x + 1, y);
                        }
                        break;
                    case 2:
                        if (map.IsAvailable(x, y - 1))
                        {
                            offsetX = 0;
                            offsetY = -1;
                            return new Point(x, y - 1);
                        }
                        break;
                    case 3:
                        if (map.IsAvailable(x, y + 1))
                        {
                            offsetX = 0;
                            offsetY = 1;
                            return new Point(x, y + 1);
                        }
                        break;
                    case 4:
                        if (map.IsAvailable(x + offsetX, y + offsetY))
                        {
                            return new Point(x + offsetX, y + offsetY);
                        }
                        break;
                    case 5:
                        if (map.IsAvailable(x + offsetX, y + offsetY))
                        {
                            return new Point(x + offsetX, y + offsetY);
                        }
                        break;
                    case 6:
                        if (map.IsAvailable(x + offsetX, y + offsetY))
                        {
                            return new Point(x + offsetX, y + offsetY);
                        }
                        break;
                    case 7:
                        if (map.IsAvailable(x + offsetX, y + offsetY))
                        {
                            return new Point(x + offsetX, y + offsetY);
                        }
                        break;
                }
                return new Point(x, y);
            }
        }
    }
}
