using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    public class Barn : Building
    {
        public Storage<Unit> Storage { get; } = new Storage<Unit>();
        public Type ItemsType { get; } = typeof(Unit);

        public Barn(Point coords) : base(coords)
        {
            BuildingType = typeof(Barn);
        }

        public void AddFood(List<Unit> inputList)
        {
            foreach (var food in inputList)
            {
                Storage.Items.Add(food);
            }
        }

        public Unit GetFood()
        {
            if (Storage.Items.Count > 0)
            {
                var food = Storage.Items[0];
                Storage.Items.RemoveAt(0);
                return food;
            }
            return null;
        }
    }
}
