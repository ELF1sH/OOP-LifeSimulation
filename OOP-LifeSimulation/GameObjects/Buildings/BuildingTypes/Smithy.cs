using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    public class Smithy : Building
    {
        public Storage<Iron> Storage { get; } = new Storage<Iron>();
        public Type ItemsType { get; } = typeof(Iron);
        public Smithy(Point coords) : base(coords)
        {
            BuildingType = typeof(Smithy);
        }
    }
}
