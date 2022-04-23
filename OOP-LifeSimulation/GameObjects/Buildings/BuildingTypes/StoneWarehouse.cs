using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    public class StoneWarehouse : Building
    {
        public Storage<Stone> Storage { get; } = new Storage<Stone>();
        public Type ItemsType { get; } = typeof(Stone);
        public StoneWarehouse(Point coords) : base(coords)
        {
            BuildingType = typeof(StoneWarehouse);
        }
    }
}
