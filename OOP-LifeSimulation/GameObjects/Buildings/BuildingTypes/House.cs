using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    public class House : Building
    {
        public Storage<IStoragable> Storage { get; } = new Storage<IStoragable>();
        public Type ItemsType { get; } = typeof(IStoragable);
        private List<Human> Owners = new List<Human>();

        public House(Point coords) : base(coords)
        {
            BuildingType = typeof(House);
        }

        public void AddOwner(Human owner)
        {
            Owners.Add(owner);
        }

        public void AddFood(List<Unit> inputList)
        {
            foreach (var food in inputList)
            {
                Storage.Items.Add(food);
            }
        }
    }
}
