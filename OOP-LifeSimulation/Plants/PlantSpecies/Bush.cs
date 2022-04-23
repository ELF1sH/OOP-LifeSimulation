using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LifeSimulation
{
    class Bush : Plant
    {
        public Bush(int _x, int _y, PlantStatus _plantStatus, Map _map) : base(_x, _y, _plantStatus, _map)
        {
            PlantType = PlantType.Bush;

            isWintery = false;
        }
    }
}
