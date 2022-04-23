using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OOP_LifeSimulation
{
    public class IronOre : Resource, IResource<Iron, Pickaxe>
    {
        public Storage<Iron> Storage { get; } = new Storage<Iron>();

        public IronOre(Point coords) : base(coords)
        {
            var outputList = new List<Iron>();
            for (int i = 0; i < new Random().Next(7, 12); i++)
            {
                outputList.Add(new Iron());
            }
            Storage.AddItems(outputList);

            ToolType = typeof(Pickaxe);
        }
    }
}
