using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_LifeSimulation
{
    class Tree : Plant, IResource<Wood, Axe>
    {
        public Storage<Wood> Storage { get; } = new Storage<Wood>();

        public Type ToolType { get; } = typeof(Axe);

        public Tree(int _x, int _y, PlantStatus _plantStatus, Map _map) : base(_x, _y, _plantStatus, _map)
        {
            PlantType = PlantType.Tree;

            isWintery = true;

            var outputList = new List<Wood>();
            for (int i = 0; i < new Random().Next(7, 12); i++)
            {
                outputList.Add(new Wood());
            }
            Storage.AddItems(outputList);
        }
    }
}
