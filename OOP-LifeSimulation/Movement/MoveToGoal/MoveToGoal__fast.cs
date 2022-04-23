using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LifeSimulation
{
    class MoveToGoal__fast
    {
        public void MoveToGoal(ref List<Point> path, ref CurPath CurPath)
        {
            path.RemoveAt(path.Count - 1);
            if (path.Count > 2) path.RemoveAt(path.Count - 1);
            else if (path.Count == 0) CurPath = CurPath.None;
        }
    }
}
