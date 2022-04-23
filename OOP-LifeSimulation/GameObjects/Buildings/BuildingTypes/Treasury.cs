using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    public class Treasury : Building
    {
        public Storage<Gold> Storage { get; } = new Storage<Gold>();
        public Type ItemsType { get; } = typeof(Gold);
        public Treasury(Point coords) : base(coords)
        {
            BuildingType = typeof(Treasury);
        }
    }
}
