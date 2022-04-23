using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    class RandomMove__bounded
    {
        Point startPoint = new Point();
        Point prevPoint = new Point(-1, -1);

        public Point CheckNearCells(int x, int y, Map map)
        {
            if (Math.Abs(prevPoint.X - x) + Math.Abs(prevPoint.Y - y) > 1)
            {
                startPoint = new Point(x, y);
            }
            prevPoint = new Point(x, y);

            int direction = new Random().Next(0, 4);
            if (direction == 0 && map.IsAvailable(x - 1, y) && Math.Abs(x - 1 - startPoint.X) + Math.Abs(y - startPoint.Y) < 5)
            {
                return new Point(x - 1, y);
            }
            else if (direction == 1 && map.IsAvailable(x, y - 1) && Math.Abs(x - startPoint.X) + Math.Abs(y - 1 - startPoint.Y) < 5)
            {
                return new Point(x, y - 1);
            }
            else if (direction == 2 && map.IsAvailable(x + 1, y) && Math.Abs(x + 1 - startPoint.X) + Math.Abs(y - startPoint.Y) < 5)
            {
                return new Point(x + 1, y);
            }
            else if (direction == 3 && map.IsAvailable(x, y + 1) && Math.Abs(x - startPoint.X) + Math.Abs(y + 1 - startPoint.Y) < 5)
            {
                return new Point(x, y + 1);
            }
            return new Point(x, y);
        }
    }
}
