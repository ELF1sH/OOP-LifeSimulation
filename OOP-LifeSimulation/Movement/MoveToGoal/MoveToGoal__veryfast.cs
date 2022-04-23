using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    class MoveToGoal__veryfast
    {
        public void MoveToGoal(ref List<Point> path, ref CurPath CurPath)
        {
            path.RemoveAt(path.Count - 1);
            if (path.Count > 2) path.RemoveAt(path.Count - 1);
            if (path.Count > 4) path.RemoveAt(path.Count - 1);
            else if (path.Count == 0) CurPath = CurPath.None;
        }
    }
}
