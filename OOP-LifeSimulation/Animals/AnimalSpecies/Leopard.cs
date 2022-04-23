using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LifeSimulation
{
    class Leopard : CarnivorousAnimal
    {
        private readonly MoveToGoal__veryfast moveToGoalAlgo = new MoveToGoal__veryfast();

        public Leopard(int _x, int _y, Map _map, int _animalId) : base(_x, _y, _map, _animalId)
        {
            AnimalType = AnimalType.Leopard;
        }

        public override void MoveToGoal()
        {
            SetXY(path[path.Count - 1].X, path[path.Count - 1].Y);
            moveToGoalAlgo.MoveToGoal(ref path, ref CurPath);
        }
    }
}