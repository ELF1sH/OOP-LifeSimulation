using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LifeSimulation
{
    class Elephant : HerbivorousAnimal
    {
        private readonly RandomMove__stable randomMoveAlgo = new RandomMove__stable();

        public Elephant(int _x, int _y, Map _map, int _animalId) : base(_x, _y, _map, _animalId)
        {
            AnimalType = AnimalType.Elephant;
        }

        protected override Point CheckNearCells(int x, int y)
        {
            return randomMoveAlgo.CheckNearCells(x, y, map);
        }
    }
}
