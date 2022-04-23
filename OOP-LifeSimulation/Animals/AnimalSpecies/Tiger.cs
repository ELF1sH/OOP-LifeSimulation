using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LifeSimulation
{
    class Tiger : CarnivorousAnimal 
    {
        MoveToGoal__fast moveToGoalAlgo;

        public Tiger(int _x, int _y, Map _map, int _animalId) : base(_x, _y, _map, _animalId)
        {
            AnimalType = AnimalType.Tiger;

            moveToGoalAlgo = new MoveToGoal__fast();
        }

        public override void MoveToGoal()
        {
            SetXY(path[path.Count - 1].X, path[path.Count - 1].Y);
            moveToGoalAlgo.MoveToGoal(ref path, ref CurPath);
        }
    }
}
