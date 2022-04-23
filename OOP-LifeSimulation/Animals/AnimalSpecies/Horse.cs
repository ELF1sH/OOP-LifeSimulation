using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LifeSimulation
{
    class Horse : HerbivorousAnimal, ITamable
    {
        private readonly RandomMove__8 randomMoveAlgo = new RandomMove__8();
        private readonly MoveToGoal__veryfast moveToGoalAlgo = new MoveToGoal__veryfast();

        public bool IsTamed { get; set; } = false;
        public int OwnerId { get; set; }

        public Human Owner { get; set; }

        public Horse(int _x, int _y, Map _map, int _animalId) : base(_x, _y, _map, _animalId)
        {
            AnimalType = AnimalType.Horse;
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

        public void ComeToOwner(int x, int y)
        {
            CurPath = CurPath.None;
            path.Clear();

            SearchFor(SearchGoal.Owner, OwnerId, x, y);
        }

        public void TamedBehavior()
        {
            SetXY(Owner.Coords.X, Owner.Coords.Y);
        }

        public void RemoveOwner()
        {
            IsTamed = false;
            Owner = null;
            OwnerId = -1;
        }
    }
}
