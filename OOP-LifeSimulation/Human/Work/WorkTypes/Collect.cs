using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_LifeSimulation
{
    public class Collect : IWork
    {
        private readonly Human _human;

        public Collect(Human human)
        {
            _human = human;
        }

        public bool Execute()
        {
            if (_human.CurPath == CurPath.None)
            {
                _human.SearchFor(SearchGoal.HerbFood);
            }
            if (_human.HP + _human.SP > 150 && _human.HumanInventory.Storage.Items.Count > 2)
            {
                var barnCoords = _human.House.Village.GetBarnCoords();
                if (barnCoords.X != -1)
                {
                    _human.SearchFor(SearchGoal.ConcretePos, x: barnCoords.X, y: barnCoords.Y);
                    _human.CurPath = CurPath.Barn;
                }
                else
                {
                    _human.SearchFor(SearchGoal.ConcretePos, x: _human.House.Coords.X, y: _human.House.Coords.Y);
                    _human.CurPath = CurPath.Home;
                }
            }
            if (_human.CurPath != CurPath.None && _human.path.Count > 0)
            {
                if (!_human.IsWaitingForFemale)
                {
                    _human.MoveToGoal();
                }
                if (_human.CurPath == CurPath.Barn && _human.path.Count == 0)
                {
                    var barnCoords = _human.House.Village.GetBarnCoords();
                    _human.map.Cells[barnCoords.X, barnCoords.Y].Building.Village.GetBarn().AddFood(
                        _human.HumanInventory.GetAllFood()
                    );
                    _human.CurPath = CurPath.None;
                }
                if (_human.CurPath == CurPath.Home && _human.path.Count == 0)
                {
                    if (!_human.CommingHomeToReproduce)
                    {
                        _human.House.AddFood(_human.HumanInventory.GetAllFood());
                    }
                    else
                    {
                        _human.IsWaitingForFemale = true;
                        _human.CurPath = CurPath.None;
                    }
                    _human.CurPath = CurPath.None;
                }
            }
            else if (_human.CurPath == CurPath.None && !_human.IsWaitingForFemale)
            {
                _human.RandomMove();
                return false;
            }
            return true;
        }
    }
}
