using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    public abstract class Unit : IStoragable
    {
        public Point Coords { get; protected set; }
        public Point PrevCoords { get; private set; }

        protected Unit(int _x, int _y)
        {
            Coords = new Point(_x, _y);
            PrevCoords = new Point(_x, _y);
        }

        public void SetXY(int _x, int _y)
        {
            PrevCoords = new Point(Coords.X, Coords.Y);
            Coords = new Point(_x, _y);
        }
    }
}
