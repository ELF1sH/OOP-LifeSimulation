using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    class MoveToGoal__8
    {
        public List<Point> CheckAvailablePoints(int curX, int curY, ref bool[,] visited, Map map)
        {
            var outputPoints = new List<Point>();
            if (map.IsAvailable(curX + 1, curY) && !visited[curX + 1, curY])
            {
                outputPoints.Add(new Point(curX + 1, curY));
            }
            if (map.IsAvailable(curX - 1, curY) && !visited[curX - 1, curY])
            {
                outputPoints.Add(new Point(curX - 1, curY));
            }
            if (map.IsAvailable(curX, curY + 1) && !visited[curX, curY + 1])
            {
                outputPoints.Add(new Point(curX, curY + 1));
            }
            if (map.IsAvailable(curX, curY - 1) && !visited[curX, curY - 1])
            {
                outputPoints.Add(new Point(curX, curY - 1));
            }

            if (map.IsAvailable(curX + 1, curY + 1) && !visited[curX + 1, curY + 1])
            {
                outputPoints.Add(new Point(curX + 1, curY + 1));
            }
            if (map.IsAvailable(curX + 1, curY - 1) && !visited[curX + 1, curY - 1])
            {
                outputPoints.Add(new Point(curX + 1, curY - 1));
            }
            if (map.IsAvailable(curX - 1, curY + 1) && !visited[curX - 1, curY + 1])
            {
                outputPoints.Add(new Point(curX - 1, curY + 1));
            }
            if (map.IsAvailable(curX - 1, curY - 1) && !visited[curX - 1, curY - 1])
            {
                outputPoints.Add(new Point(curX - 1, curY - 1));
            }
            return outputPoints;
        }
    }
}
