using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    public abstract class Building : GameObject
    {
        public Type BuildingType { get; protected set; }
        public Village Village { get; private set; } = null;

        public Building(Point coords) : base(coords)
        {
            
        }

        public void AddToVillage(Village village)
        {
            Village = village;
        }
    }
}
