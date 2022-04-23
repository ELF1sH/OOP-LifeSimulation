using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    public class Fruit : Unit
    {                                                               
        public bool IsPoisonous { get; }
        public PlantType PlantType { get; }
        public bool IsDropped { get; private set; }
        private int age;
        private readonly int ageToDrop = 250;

        public Fruit(int _x, int _y, bool _isPoisonous, PlantType _plantType) : base(_x, _y)
        {
            IsPoisonous = _isPoisonous;
            PlantType = _plantType;
            IsDropped = false;

            age = new Random().Next(0, ageToDrop);
        }

        public bool Grow()
        {
            age++;
            if (age == ageToDrop)
            {
                IsDropped = true;
                return true;
            }
            return false;
        }

        public void UpdateCoords(Point p)
        {
            Coords = p;
        }
    }
}
