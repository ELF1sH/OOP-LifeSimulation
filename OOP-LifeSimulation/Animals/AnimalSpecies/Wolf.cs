using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LifeSimulation
{
    class Wolf : CarnivorousAnimal
    {
        private readonly RandomMove__stable randomMoveAlgo = new RandomMove__stable();
        private readonly MoveToGoal__fast moveToGoalAlgo = new MoveToGoal__fast();

        public Wolf(int _x, int _y, Map _map, int _animalId) : base(_x, _y, _map, _animalId)
        {
            AnimalType = AnimalType.Wolf;
        }

        protected override Point CheckNearCells(int x, int y)
        {
            return randomMoveAlgo.CheckNearCells(x, y, map);
        }

        public override void MoveToGoal()
        {
            SetXY(path[path.Count - 1].X, path[path.Count - 1].Y);
            moveToGoalAlgo.MoveToGoal(ref path, ref CurPath);
        }
    }
}
