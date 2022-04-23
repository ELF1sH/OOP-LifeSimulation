using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LifeSimulation
{
    class Bear : OmnivorousAnimal 
    {
        private readonly RandomMove__stable randomMoveAlgo = new RandomMove__stable();
        private readonly MoveToGoal__8 moveToGoalAlgo = new MoveToGoal__8();

        public Bear(int _x, int _y, Map _map, int _animalId) : base(_x, _y, _map, _animalId)
        {
            AnimalType = AnimalType.Bear;
            isHibernative = true;
        }

        protected override Point CheckNearCells(int x, int y)
        {
            return randomMoveAlgo.CheckNearCells(x, y, map);
        }

        protected override List<Point> CheckAvailablePoints(int curX, int curY, ref bool[,] visited)
        {
            return moveToGoalAlgo.CheckAvailablePoints(curX, curY, ref visited, map);
        }
    }
}
