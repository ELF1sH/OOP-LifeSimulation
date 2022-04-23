using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OOP_LifeSimulation
{
    public class Resource: GameObject
    {
        public Type ToolType { get; protected set; }
        public Resource(Point coords) : base(coords)
        {

        }
    }
}
