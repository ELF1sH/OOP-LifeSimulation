using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    public class WoodWarehouse : Building
    {
        public Storage<Wood> Storage { get; } = new Storage<Wood>();
        public Type ItemsType { get; } = typeof(Wood);
        public WoodWarehouse(Point coords) : base(coords)
        {
            BuildingType = typeof(WoodWarehouse);
        }
    }
}
