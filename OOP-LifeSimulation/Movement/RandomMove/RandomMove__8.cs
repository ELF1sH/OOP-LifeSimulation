using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    class RandomMove__8
    {
        public Point CheckNearCells(int x, int y, Map map)
        {
            switch (new Random().Next(0, 8))
            {
                case 0: 
                    if (map.IsAvailable(x - 1, y)) return new Point(x - 1, y);
                    break;
                case 1:
                    if (map.IsAvailable(x + 1, y)) return new Point(x + 1, y);
                    break;
                case 2:
                    if (map.IsAvailable(x, y - 1)) return new Point(x, y - 1);
                    break;
                case 3:
                    if (map.IsAvailable(x, y + 1)) return new Point(x, y + 1);
                    break;
                case 4:
                    if (map.IsAvailable(x - 1, y - 1)) return new Point(x - 1, y - 1);
                    break;
                case 5:
                    if (map.IsAvailable(x + 1, y + 1)) return new Point(x + 1, y + 1);
                    break;
                case 6:
                    if (map.IsAvailable(x - 1, y + 1)) return new Point(x - 1, y + 1);
                    break;
                case 7:
                    if (map.IsAvailable(x + 1, y - 1)) return new Point(x + 1, y - 1);
                    break;
            }
            return new Point(x, y);
        }
    }
}
