using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    public abstract class GameObject
    {
        public Point Coords { get; }

        public GameObject(Point coords)
        {
            Coords = coords;
        }
    }
}
