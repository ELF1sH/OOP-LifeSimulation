using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OOP_LifeSimulation
{
    public class GoldMine : Resource, IResource<Gold, Pickaxe>
    {
        public Storage<Gold> Storage { get; } = new Storage<Gold>();

        public GoldMine(Point coords) : base(coords)
        {
            var outputList = new List<Gold>();
            for (int i = 0; i < new Random().Next(7, 12); i++)
            {
                outputList.Add(new Gold());
            }
            Storage.AddItems(outputList);

            ToolType = typeof(Pickaxe);
        }
    }
}