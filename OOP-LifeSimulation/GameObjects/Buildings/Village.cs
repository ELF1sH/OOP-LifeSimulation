using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    public class Village
    {
        public List<Building> Buildings = new List<Building>();

        public bool DoesHaveBarn()
        {
            foreach (var building in Buildings)
            {
                if (building is Barn) return true;
            }
            return false;
        }

        public Point GetBarnCoords()
        {
            foreach (var building in Buildings)
            {
                if (building is Barn)
                {
                    return building.Coords;
                }
            }
            return new Point(-1, -1);
        }

        public Barn GetBarn()
        {
            foreach (var building in Buildings)
            {
                if (building is Barn)
                {
                    return building as Barn;
                }
            }
            return null;
        }
    }
}
