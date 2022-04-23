using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_LifeSimulation
{
    public class Build : IWork
    {
        private readonly Human _human; 

        public Build(Human human)
        {
            _human = human;
        }  
        
        public bool Execute()
        {
            if (_human.HP + _human.SP > 150 && !_human.IsWaitingForTamedAnimal && !_human.IsWaitingForFemale && _human.CurPath == CurPath.None)
            {
                if (_human.SearchFor(SearchGoal.Materials, tool: _human.Tool))
                {
                    _human.CurPath = CurPath.Materials;
                }
            }
            else if (_human.CurPath != CurPath.None && _human.path.Count > 0)
            {
                if (!_human.IsWaitingForFemale)
                {
                    _human.MoveToGoal();
                }
                if (_human.CurPath == CurPath.Materials && _human.path.Count == 0)
                {
                    Console.WriteLine("reached a material");
                    HandleMaterial();
                }
                if (_human.CurPath == CurPath.Construction && _human.path.Count == 0)
                {
                    Console.WriteLine("reached a construction");
                    if (!TryToBuild())
                    {
                        if (_human.SearchFor(SearchGoal.Materials, resourceType: _human.CurResourceType, tool: _human.Tool))
                        {
                            _human.CurPath = CurPath.Materials;
                        }
                        else _human.CurPath = CurPath.None;
                    }
                    else
                    {
                        _human.CurPath = CurPath.None;
                    }
                }
            }
            else if (_human.CurPath == CurPath.None && !_human.IsWaitingForFemale)
            {
                _human.RandomMove();
                return false;
            }
            return true;
        }

        private void HandleMaterial()
        {
            _human.GetItemsFromResource();

            if (_human.constructionPos.X == -1)
            {
                _human.constructionPos = _human.map.GetFreeCloseCell(_human.House.Coords, new BiomStatus[] { BiomStatus.Grass, BiomStatus.Desert }, 5);
            }
            
            _human.SearchFor(searchGoal: SearchGoal.Construction, x: _human.constructionPos.X, y: _human.constructionPos.Y);
            _human.CurPath = CurPath.Construction;
        }

        private bool TryToBuild()
        {
            _human.map.Cells[_human.Coords.X, _human.Coords.Y].MaterialsOnConstruction.AddRange(_human.HumanInventory.RemoveMaterials());
            if (_human.map.Cells[_human.Coords.X, _human.Coords.Y].MaterialsOnConstruction.Count >= _human.itemsToBuild)
            {
                _human.map.Build
                (
                    _human.Coords, 
                    _human.map.Cells[_human.Coords.X, _human.Coords.Y].MaterialsOnConstruction[0].GetType(), 
                    isBarn: !_human.House.Village.DoesHaveBarn()
                );
                _human.map.Cells[_human.Coords.X, _human.Coords.Y].MaterialsOnConstruction.Clear();
                _human.constructionPos.X = -1;
                _human.constructionPos.Y = -1;
                return true;
            }
            return false;
        }
    }
}
