using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    public abstract class Animal : Unit
    {
        public Map map { get; }

        public AnimalType AnimalType { get; protected set; }
        public FeedType FeedType { get; protected set; }
        public int AnimalId { get; }

        public Gender Gender { get; }
        public int HP { get; protected set; }
        public int SP { get; protected set; }
        
        public CurPath CurPath = CurPath.None;

        protected bool isLookingForFemale = false;
        protected int timeNoReproduce = 15;

        public List<Point> path = new List<Point>();
        protected int initialPathLength = -1;
        public bool HasBeenEaten;
        protected Animal targetAnimal = null;

        protected int age;
        private readonly int DEATH_AGE = 14000;

        protected bool isHibernative = false;
        
        public Animal(int _x, int _y, Map _map, int _animalId) : base(_x, _y)
        {
            map = _map;
            AnimalId = _animalId;
            HP = 100;
            SP = new Random().Next(60, 100);

            if (new Random().Next(0, 2) == 0) Gender = Gender.Male;
            else Gender = Gender.Female;

            age = new Random().Next(0, 100);
        }

        public virtual AnimalResult Update()
        {
            if (HibernateHandler()) return AnimalResult.Nothing;

            AnimalResult animalResult;
            animalResult = UpdateSPHP();

            if (this is ITamable && (this as ITamable).IsTamed)
            {
                (this as ITamable).TamedBehavior();
            }
            else
            {
                if (CurPath == CurPath.None || path.Count == 0)
                {
                    if (HP == 100 && SP >= 98 && Gender == Gender.Male && timeNoReproduce == 0)
                    {
                        SearchFor(SearchGoal.Female);
                        if (path.Count >= 24)
                        {
                            path.Clear();
                            CurPath = CurPath.None;
                            isLookingForFemale = false;
                        }
                    }
                    RandomMove();
                }
                else
                {
                    MoveToGoal();
                    if (path.Count == initialPathLength / 2 && path.Count > 5)
                    {
                        if (CurPath == CurPath.Food) SearchFor(SearchGoal.Food);
                        else if (CurPath == CurPath.Female) SearchFor(SearchGoal.Female);
                    }
                }
            }

            if (timeNoReproduce > 0) timeNoReproduce--;
            age++;
            if (age == DEATH_AGE) return AnimalResult.Dead;
            return animalResult;
        }

        private bool HibernateHandler()
        {
            if (isHibernative && map.CurSeason == Season.Winter)
            {
                if (HP < 100) HP += 2;
                if (HP > 100) HP = 100;
                return true;
            }
            return false;
        }

        public virtual Animal TryToReproduce()
        {
            if (this is ITamable && (this as ITamable).IsTamed) return null;

            if (Gender == Gender.Male && isLookingForFemale)
            {
                if (map.DoesHaveFemale(Coords, AnimalType))
                {
                    timeNoReproduce = 20;
                    isLookingForFemale = false;

                    var newObj = Activator.CreateInstance(GetType(), Coords.X, Coords.Y, map, new Random().Next(2000, 100000));
                    return newObj as Animal;
                }
                else if (CurPath == CurPath.None && SP > 0)
                {
                    SearchFor(SearchGoal.Female);
                }
                else if (CurPath != CurPath.Female && SP == 0)
                {
                    isLookingForFemale = false;
                }
            }
            return null;
        }

        public void RandomMove()
        {
            if (path.Count == 0)
            {
                CurPath = CurPath.None;
            }
            var newPoint = CheckNearCells(Coords.X, Coords.Y);
            SetXY(newPoint.X, newPoint.Y);
        }

        protected virtual Point CheckNearCells(int x, int y)
        {
            int direction = new Random().Next(0, 4);
            if (direction == 0 && map.IsAvailable(x - 1, y))
            {
                return new Point(x - 1, y);
            }
            else if (direction == 1 && map.IsAvailable(x, y - 1))
            {
                return new Point(x, y - 1);
            }
            else if (direction == 2 && map.IsAvailable(x + 1, y))
            {
                return new Point(x + 1, y);
            }
            else if (direction == 3 && map.IsAvailable(x, y + 1))
            {
                return new Point(x, y + 1);
            }
            return new Point(x, y);
        }

        public virtual void MoveToGoal()
        {
            SetXY(path[path.Count - 1].X, path[path.Count - 1].Y);
            path.RemoveAt(path.Count - 1);
            if (path.Count == 0)
            {
                if (CurPath == CurPath.Food || CurPath == CurPath.Female || CurPath == CurPath.Owner)
                {
                    CurPath = CurPath.None;
                }
            }
        }

        public virtual AnimalResult TryToEat()
        {
            if (this is ITamable && (this as ITamable).IsTamed) return AnimalResult.Nothing;
            if (SP < 50 && !isLookingForFemale)
            {
                if (CurPath != CurPath.Food) SearchFor(SearchGoal.Food);
                var food = map.DoesHaveFood(Coords, FeedType, AnimalType);

                var animalResult = CheckFood(food);
                switch (animalResult)
                {
                    case AnimalResult.EatedPoisonousPlant: UpdateHP__PoisonousPlant(); break;
                    case AnimalResult.EatedPlant: UpdateHP__HealthyFood(); break;
                    case AnimalResult.EatedAnimal: UpdateHP__HealthyFood(); break;
                }
                return animalResult;
            }
            
            return AnimalResult.Nothing;
        } 

        protected virtual AnimalResult CheckFood(Unit food)
        {
            if (food == null && targetAnimal != null)
            {
                if (Math.Abs(Coords.X - targetAnimal.Coords.X) + Math.Abs(Coords.Y - targetAnimal.Coords.Y) == 1)
                {
                    SearchFor(SearchGoal.Food);
                    targetAnimal = null;
                    return AnimalResult.EatedAnimal;
                }
                return AnimalResult.Nothing;
            }

            AnimalResult animalResult = AnimalResult.Nothing;
            if (food is Plant || food is Fruit)
            {
                if (map.IsFoodPoisonous(Coords))
                {
                    animalResult = AnimalResult.EatedPoisonousPlant;
                }
                else
                {
                    animalResult = AnimalResult.EatedPlant;
                }
            }
            else if (food is Animal)
            {
                animalResult = AnimalResult.EatedAnimal;
                targetAnimal = null;
            }

            // Detaching eaten units from map
            if (food is Plant) map.DetachPlantFromCell(Coords.X, Coords.Y);
            else if (food is Fruit) map.DetachFruitFromCell(Coords.X, Coords.Y);
            else if (food is Animal)
            {
                map.DetachAnimalFromCell(Coords.X, Coords.Y, (food as Animal).AnimalType);
                map.DetachAnimalFromCell(PrevCoords.X, PrevCoords.Y, (food as Animal).AnimalType);
            }

            return animalResult;
        }

        public void UpdateHP__PoisonousPlant()
        {
            if (HP <= 5) HP = 0;
            else HP -= 5;
        }
        public void UpdateHP__HealthyFood()
        {
            HP = 100;
            SP = 100;
        }

        protected virtual AnimalResult UpdateSPHP()
        {
            if (map.CurSeason == Season.Summer)
            {
                if (SP >= 1) SP -= 1;
                else if (SP == 0)
                {
                    if (HP > 1) HP -= 1;
                    else HP = 0;
                }
            }
            else if (map.CurSeason == Season.Winter)
            {
                if (SP >= 2) SP -= 2;
                else if (SP > 0 && SP < 2) SP = 0;
                else if (SP == 0)
                {
                    if (HP > 1) HP -= 1;
                    else HP = 0;
                }
            }
            
            if (HP == 0)
            {
                return AnimalResult.Dead;
            }
            if (SP <= 50) return AnimalResult.SearchingForFood;
            return AnimalResult.Nothing;
        }

        public bool SearchFor(SearchGoal searchGoal, int humanId = -1, int x = -1, int y = -1, Type resourceType = null, Tool tool = null)
        {
            var start = Coords;
            var goal = new Point();

            if (searchGoal == SearchGoal.Food)
            {
                CurPath = CurPath.Food;
                goal = FindClosest(start, searchGoal, false);
            }
            else if (searchGoal == SearchGoal.Female)
            {
                goal = FindClosest(start, searchGoal, true, humanId);
                if (goal.X == -1)
                {
                    CurPath = CurPath.None;
                    isLookingForFemale = false;
                    return false;
                }
                else
                {
                    CurPath = CurPath.Female;
                    isLookingForFemale = true;
                }
            }
            else if (searchGoal == SearchGoal.Owner)
            {
                if (x == -1) goal = FindClosest(start, searchGoal, true, humanId);
                else goal = new Point(x, y);
                CurPath = CurPath.Owner;
            }
            else if (searchGoal == SearchGoal.Materials)
            {
                if (x == -1)
                {
                    goal = FindClosest(start: start, searchGoal: searchGoal, isLimited: false, resourceType: resourceType, tool: tool);
                    if (goal.X == -1)
                    {
                        return false;
                    }
                }
                else goal = new Point(x, y);
            }
            else if (searchGoal == SearchGoal.Construction)
            {
                if (x == -1) return false;
                goal = new Point(x, y);
            }
            else if (searchGoal == SearchGoal.ConcretePos)
            {
                if (x == -1) return false;
                goal = new Point(x, y);
            }
            else if (searchGoal == SearchGoal.HerbFood)
            {
                CurPath = CurPath.HerbFood;
                goal = FindClosest(start, searchGoal, false);
            }

            bool[,] visited = new bool[map.FieldSize, map.FieldSize];
            Point[,] parent = new Point[map.FieldSize, map.FieldSize];
            for (int i = 0; i < map.FieldSize; i++)
            {
                for (int j = 0; j < map.FieldSize; j++)
                {
                    visited[i, j] = false;
                }
            }

            var openList = new List<OpenListItem>();
            openList.Add(new OpenListItem(start, 0, 0));
            while (openList.Count > 0)
            {
                OpenListItem curOpenListItem = openList[0];
                openList.RemoveAt(0);
                int curX = curOpenListItem.point.X;
                int curY = curOpenListItem.point.Y;

                if (curX == goal.X && curY == goal.Y) break;

                visited[curX, curY] = true;
                var pointsToAnalyze = CheckAvailablePoints(curX, curY, ref visited);
                foreach (Point p in pointsToAnalyze)
                {
                    analyzePoint(p.X, p.Y, curOpenListItem);
                }

                // sort list in the end by F value
                openList.Sort(delegate (OpenListItem item1, OpenListItem item2) {
                    return item1.F.CompareTo(item2.F);
                });
            }

            void analyzePoint(int x, int y, OpenListItem curOpenListItem)
            {
                if (!openList.Exists(a => (a.point.X == x && a.point.Y == y)))
                {
                    addItemIfNotExists(x, y, curOpenListItem);
                }
                else
                {
                    compareGs(x, y, curOpenListItem);
                }
            }

            void addItemIfNotExists(int x, int y, OpenListItem curOpenListItem)
            {
                parent[x, y] = curOpenListItem.point;
                int G = curOpenListItem.G + 1;
                int H = Math.Abs(goal.X - x) + Math.Abs(goal.Y - y);
                openList.Add(new OpenListItem(new Point(x, y), G + H, G));
            }

            void compareGs(int x, int y, OpenListItem curOpenListItem)
            {
                var existedOpenListItem = new OpenListItem(new Point(-1, -1), 0, 0);

                openList.ForEach(delegate (OpenListItem oli) {
                    if (oli.point.X == x && oli.point.Y == y) existedOpenListItem = oli;
                });

                if (existedOpenListItem.G > curOpenListItem.G + 1)
                {
                    existedOpenListItem.G = curOpenListItem.G + 1;
                    parent[existedOpenListItem.point.X, existedOpenListItem.point.Y] = curOpenListItem.point;
                }
            }

            path = new List<Point>();
            Point curPoint = goal;
            while (curPoint.X != start.X || curPoint.Y != start.Y)
            {
                path.Add(curPoint);
                if (path.Count > 100)
                {
                    path.Clear();
                    CurPath = CurPath.None;
                }
                if (curPoint.X != -1 && curPoint.Y != -1)
                {
                    curPoint = parent[curPoint.X, curPoint.Y];
                }
            }
            initialPathLength = path.Count;

            return true;
        }

        protected virtual List<Point> CheckAvailablePoints(int curX, int curY, ref bool[,] visited)
        {
            var outputPoints = new List<Point>();
            if (map.IsAvailable(curX + 1, curY) && !visited[curX + 1, curY])
            {
                outputPoints.Add(new Point(curX + 1, curY));
            }
            if (map.IsAvailable(curX - 1, curY) && !visited[curX - 1, curY])
            {
                outputPoints.Add(new Point(curX - 1, curY));
            }
            if (map.IsAvailable(curX, curY + 1) && !visited[curX, curY + 1])
            {
                outputPoints.Add(new Point(curX, curY + 1));
            }
            if (map.IsAvailable(curX, curY - 1) && !visited[curX, curY - 1])
            {
                outputPoints.Add(new Point(curX, curY - 1));
            }
            return outputPoints;
        }

        protected Point FindClosest(Point start, SearchGoal searchGoal, bool isLimited, int humanId = -1, Type resourceType = null, Type materialType = null, Tool tool = null)
        {
            var q = new Queue<Point>();
            int fieldSize = map.FieldSize;
            bool[,] visited = new bool[fieldSize, fieldSize];
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    visited[i, j] = false;
                }
            }
            int counter = 0;
            q.Enqueue(start);
            while (q.Count > 0)
            {
                Point curCell = q.Dequeue();

                counter++;
                if (isLimited && counter == 600 && searchGoal != SearchGoal.ClosestBuilding) return new Point(-1, -1);
                if (searchGoal == SearchGoal.ClosestBuilding && counter == 5000) return new Point(-1, -1);

                if (visited[curCell.X, curCell.Y]) continue;
                if (searchGoal == SearchGoal.Food)
                {
                    Unit goal = null;
                    if (this is Human)
                    {
                        goal = map.DoesHaveFood(curCell, FeedType, AnimalType, (this as Human).TamableAnimalsId);
                    }
                    else
                    {
                        goal = map.DoesHaveFood(curCell, FeedType, AnimalType);
                    }
                    if (goal != null)
                    {
                        if (goal is Animal)
                        {
                            targetAnimal = goal as Animal;
                        }
                        return curCell;
                    }
                }
                else if (searchGoal == SearchGoal.Female)
                {
                    if (map.DoesHaveFemale(curCell, AnimalType, humanId))
                    {
                        return curCell;
                    }
                }
                else if (searchGoal == SearchGoal.TamableAnimal)
                {
                    if (map.DoesHaveTamableAnimal(curCell)) return curCell;
                }
                else if (searchGoal == SearchGoal.Owner)
                {
                    if (map.DoesHaveHuman(curCell, humanId)) return curCell;
                }
                else if (searchGoal == SearchGoal.Materials)
                {
                    if (map.DoesHaveResource(curCell, resourceType, tool)) return curCell;
                }
                else if (searchGoal == SearchGoal.Warehouse)
                {
                    if (map.DoesHaveWarehouse(curCell, materialType)) return curCell;
                }
                else if (searchGoal == SearchGoal.ClosestBuilding)
                {
                    if (map.DoesHaveBuilding(curCell)) return curCell;
                }
                else if (searchGoal == SearchGoal.HerbFood)
                {
                    if (map.DoesHaveHerbFood(curCell)) return curCell;
                }

                visited[curCell.X, curCell.Y] = true;
                if (map.IsAvailable(curCell.X - 1, curCell.Y) && curCell.X - 1 >= 0 && !visited[curCell.X - 1, curCell.Y])
                {
                    q.Enqueue(new Point(curCell.X - 1, curCell.Y));
                }
                if (map.IsAvailable(curCell.X + 1, curCell.Y) && curCell.X + 1 < map.FieldSize && !visited[curCell.X + 1, curCell.Y])
                {
                    q.Enqueue(new Point(curCell.X + 1, curCell.Y));
                }
                if (map.IsAvailable(curCell.X, curCell.Y - 1) && curCell.Y - 1 >= 0 && !visited[curCell.X, curCell.Y - 1])
                {
                    q.Enqueue(new Point(curCell.X, curCell.Y - 1));
                }
                if (map.IsAvailable(curCell.X, curCell.Y + 1) && curCell.Y + 1 < map.FieldSize && !visited[curCell.X, curCell.Y + 1])
                {
                    q.Enqueue(new Point(curCell.X, curCell.Y + 1));
                }
            }
            return new Point(-1, -1);
        }
    }

    public struct OpenListItem 
    {
        public Point point;
        public int F;
        public int G;
        public OpenListItem(Point _p, int _f, int _g)
        {
            point = _p;
            F = _f;
            G = _g;
        }
    }
}