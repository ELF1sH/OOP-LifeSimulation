using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OOP_LifeSimulation
{
    public class StoneRock : Resource, IResource<Stone, Drill>
    {
        public Storage<Stone> Storage { get; } = new Storage<Stone>();

        public StoneRock(Point coords) : base(coords)
        {
            var outputList = new List<Stone>();
            for (int i = 0; i < new Random().Next(7, 12); i++)
            {
                outputList.Add(new Stone());
            }
            Storage.AddItems(outputList);

            ToolType = typeof(Drill);
        }
    }
}