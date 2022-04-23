using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OOP_LifeSimulation
{
    public class Map
    {
        public Cell[,] Cells { get; private set; }
        public Drawer Drawer { get; set; }
        public int FieldSize { get; private set; }
        public List<Village> Villages { get; } = new List<Village>();

        public Season CurSeason = Season.Summer;

        public Map(int _fieldSize)
        {
            FieldSize = _fieldSize;
            Cells = new Cell[FieldSize, FieldSize];

            InitialMapGenerate();
        }

        private void InitialMapGenerate()
        {
            var noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetSeed(new Random().Next());
            Cell[,] _cells = new Cell[FieldSize, FieldSize];
            BiomStatus biomStatus;
            for (int i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    biomStatus = GetBiomStatusForGeneration(noise.GetNoise(i, j));
                    _cells[i, j] = new Cell(biomStatus);
                }
            }
            Cells = _cells;
        }

        public void AttachPlantToCell(int x, int y, ref Plant _plant)
        {
            Cells[x, y].AttachPlant(ref _plant);
        }

        public void DetachPlantFromCell(int x, int y)
        {
            Cells[x, y].DetachPlant();
        }

        public void AttachAnimalToCell(int x, int y, ref Animal _animal)
        {
            Cells[x, y].AttachAnimal(ref(_animal));
        }

        public void DetachAnimalFromCell(int x, int y, AnimalType animalType)
        {
            Cells[x, y].DetachAnimal(animalType);
        }

        public void AttachFruitToCell(int x, int y, ref Fruit _fruit)
        {
            Cells[x, y].AttachFruit(ref _fruit);
        }

        public void DetachFruitFromCell(int x, int y)
        {
            Cells[x, y].DetachFruit();
        }

        public Fruit GetFruitFromCell(int x, int y)
        {
            return Cells[x, y].Fruit;
        }

        public Unit DoesHaveFood(Point p, FeedType feedType, AnimalType animalType, List<int> tamedAnimalsId = null)
        {
            if (Cells[p.X, p.Y].HasPlant &&
                Cells[p.X, p.Y].Plant is IEatable && 
                (Cells[p.X, p.Y].Plant.PlantStatus == PlantStatus.Sprout || Cells[p.X, p.Y].Plant.PlantStatus == PlantStatus.Adult))
            {
                if (feedType == FeedType.Herbivorous || feedType == FeedType.Omnivorous) return Cells[p.X, p.Y].Plant;
            }
            if (Cells[p.X, p.Y].HasFruit)
            {
                if (feedType == FeedType.Herbivorous || feedType == FeedType.Omnivorous) return Cells[p.X, p.Y].Fruit;
            }
            if (Cells[p.X, p.Y].HasAnimal)
            {
                foreach (Animal an in Cells[p.X, p.Y].Animals)
                {
                    if (an.AnimalType != animalType && (feedType == FeedType.Carnivorous || feedType == FeedType.Omnivorous))
                    {
                        if (tamedAnimalsId != null)
                        {
                            foreach (int id in tamedAnimalsId)
                            {
                                if (id == an.AnimalId) return null;
                            }
                        }
                        return an;
                    }
                }
            }
            return null;
        }

        public bool DoesHaveFemale(Point p, AnimalType animalType, int womanId = -1)
        {
            if (Cells[p.X, p.Y].HasAnimal)
            {
                foreach (Animal an in Cells[p.X, p.Y].Animals)
                {
                    if (an.Gender == Gender.Female && an.AnimalType == animalType)
                    {
                        if (womanId == -1) return true;
                        else if ((an as Human).AnimalId == womanId) return true;
                    }
                }
            }
            if (p.X + 1 < FieldSize && Cells[p.X + 1, p.Y].HasAnimal)
            {
                foreach (Animal an in Cells[p.X + 1, p.Y].Animals)
                {
                    if (an.Gender == Gender.Female && an.AnimalType == animalType)
                    {
                        if (womanId == -1) return true;
                        else if ((an as Human).AnimalId == womanId) return true;
                    }
                }
            }
            if (p.X - 1 >= 0 && Cells[p.X - 1, p.Y].HasAnimal)
            {
                foreach (Animal an in Cells[p.X - 1, p.Y].Animals)
                {
                    if (an.Gender == Gender.Female && an.AnimalType == animalType)
                    {
                        if (womanId == -1) return true;
                        else if ((an as Human).AnimalId == womanId) return true;
                    }
                }
            }
            if (p.Y + 1 < FieldSize && Cells[p.X, p.Y + 1].HasAnimal)
            {
                foreach (Animal an in Cells[p.X, p.Y + 1].Animals)
                {
                    if (an.Gender == Gender.Female && an.AnimalType == animalType)
                    {
                        if (womanId == -1) return true;
                        else if ((an as Human).AnimalId == womanId) return true;
                    }
                }
            }
            if (p.Y - 1 >= 0 && Cells[p.X, p.Y - 1].HasAnimal)
            {
                foreach (Animal an in Cells[p.X, p.Y - 1].Animals)
                {
                    if (an.Gender == Gender.Female && an.AnimalType == animalType)
                    {
                        if (womanId == -1) return true;
                        else if ((an as Human).AnimalId == womanId) return true;
                    }
                }
            }
            return false;
        }

        public bool DoesHaveTamableAnimal(Point p)
        {
            if (Cells[p.X, p.Y].HasAnimal)
            {
                foreach (Animal an in Cells[p.X, p.Y].Animals)
                {
                    if (an is ITamable && !(an as ITamable).IsTamed)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool DoesHaveHuman(Point p, int humanId)
        {
            if (Cells[p.X, p.Y].HasAnimal)
            {
                foreach (Animal an in Cells[p.X, p.Y].Animals)
                {
                    if (an is Human && (an as Human).AnimalId == humanId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool DoesHaveWarehouse(Point p, Type materialType)
        {
            if (Cells[p.X, p.Y].Building != null)
            {
                if (Cells[p.X, p.Y].Building is Smithy && (Cells[p.X, p.Y].Building as Smithy).ItemsType == materialType || 
                    Cells[p.X, p.Y].Building is StoneWarehouse && (Cells[p.X, p.Y].Building as StoneWarehouse).ItemsType == materialType ||
                    Cells[p.X, p.Y].Building is Treasury && (Cells[p.X, p.Y].Building as Treasury).ItemsType == materialType ||
                    Cells[p.X, p.Y].Building is WoodWarehouse && (Cells[p.X, p.Y].Building as WoodWarehouse).ItemsType == materialType)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool DoesHaveBuilding(Point p)
        {
            if (Cells[p.X, p.Y].Building != null) return true;
            return false;
        }

        public bool DoesHaveResource(Point p, Type resourceType, Tool tool)
        {
            if (resourceType != null)
            {
                if (Cells[p.X, p.Y].Resource != null && Cells[p.X, p.Y].Resource.GetType() == resourceType &&
                    Cells[p.X, p.Y].Resource.ToolType == tool.GetType()) return true;
                if (Cells[p.X, p.Y].HasPlant && Cells[p.X, p.Y].Plant is Tree &&
                    (Cells[p.X, p.Y].Plant as Tree).ToolType == tool.GetType() && 
                    Cells[p.X, p.Y].Plant.GetType() == resourceType &&
                    (Cells[p.X, p.Y].Plant.PlantStatus == PlantStatus.Adult ||
                    Cells[p.X, p.Y].Plant.PlantStatus == PlantStatus.Rotted)) return true;
                return false;
            }
            else
            {
                if (Cells[p.X, p.Y].Resource != null && Cells[p.X, p.Y].Resource.ToolType == tool.GetType()) return true;
                if (Cells[p.X, p.Y].HasPlant && Cells[p.X, p.Y].Plant is Tree &&
                    (Cells[p.X, p.Y].Plant as Tree).ToolType == tool.GetType() &&
                    (Cells[p.X, p.Y].Plant.PlantStatus == PlantStatus.Adult ||
                    Cells[p.X, p.Y].Plant.PlantStatus == PlantStatus.Rotted)) return true;
                return false;
            }
        }

        public bool DoesHaveHerbFood(Point p)
        {
            if (Cells[p.X, p.Y].HasPlant &&
                Cells[p.X, p.Y].Plant is IEatable &&
                (Cells[p.X, p.Y].Plant.PlantStatus == PlantStatus.Sprout || Cells[p.X, p.Y].Plant.PlantStatus == PlantStatus.Adult))
            {
                return true;
            }
            if (Cells[p.X, p.Y].HasFruit)
            {
                return true;
            }
            return false;
        }



        public Animal GetAnimal(Point p)
        {
            if (Cells[p.X, p.Y].HasAnimal) return Cells[p.X, p.Y].Animals[0];
            return null;
        }

        public Human ExchangePartnersInfo(Point p, int manId, Human man)
        {
            if (Cells[p.X, p.Y].HasAnimal)
            {
                foreach (var animal in Cells[p.X, p.Y].Animals)
                {
                    if (animal is Human && animal.Gender == Gender.Female)
                    {
                        (animal as Human).Partner = man;
                        (animal as Human).PartnerId = manId;
                        return animal as Human;
                    }
                }
            }
            if (p.X + 1 < FieldSize && Cells[p.X + 1, p.Y].HasAnimal)
            {
                foreach (var animal in Cells[p.X + 1, p.Y].Animals)
                {
                    if (animal is Human && animal.Gender == Gender.Female)
                    {
                        (animal as Human).Partner = man;
                        (animal as Human).PartnerId = manId;
                        return animal as Human;
                    }
                }
            }
            if (p.X - 1 >= 0 && Cells[p.X - 1, p.Y].HasAnimal)
            {
                foreach (var animal in Cells[p.X - 1, p.Y].Animals)
                {
                    if (animal is Human && animal.Gender == Gender.Female)
                    {
                        (animal as Human).Partner = man;
                        (animal as Human).PartnerId = manId;
                        return animal as Human;
                    }
                }
            }
            if (p.Y + 1 < FieldSize && Cells[p.X, p.Y + 1].HasAnimal)
            {
                foreach (var animal in Cells[p.X, p.Y + 1].Animals)
                {
                    if (animal is Human && animal.Gender == Gender.Female)
                    {
                        (animal as Human).Partner = man;
                        (animal as Human).PartnerId = manId;
                        return animal as Human;
                    }
                }
            }
            if (p.Y - 1 >= 0 && Cells[p.X, p.Y - 1].HasAnimal)
            {
                foreach (var animal in Cells[p.X, p.Y - 1].Animals)
                {
                    if (animal is Human && animal.Gender == Gender.Female)
                    {
                        (animal as Human).Partner = man;
                        (animal as Human).PartnerId = manId;
                        return animal as Human;
                    }
                }
            }
            return null;
        }

        public bool IsFoodPoisonous(Point p)
        {
            if (Cells[p.X, p.Y].HasFruit && Cells[p.X, p.Y].Fruit.IsPoisonous) return true;
            if (Cells[p.X, p.Y].HasPlant && Cells[p.X, p.Y].Plant is IEatable)
            {
                IEatable plant = Cells[p.X, p.Y].Plant as IEatable;
                if (plant.IsPlantPoisonous) return true;
            }
            return false;
        }

        private bool IsCellFree(Point p)
        {
            if (Cells[p.X, p.Y].HasPlant || Cells[p.X, p.Y].HasFruit || Cells[p.X, p.Y].Resource != null || Cells[p.X, p.Y].Building != null)
            {
                return false;
            }
            return true;
        }

        private BiomStatus GetBiomStatusForGeneration(float noiseRatio)
        {
            if (noiseRatio < -0.6) return BiomStatus.Water;
            if (noiseRatio < 0.4) return BiomStatus.Grass;
            if (noiseRatio < 0.8) return BiomStatus.Desert;
            return BiomStatus.Hill;
        }

        public BiomStatus GetBiomStatusCell(int x, int y)
        {
            return Cells[x, y].BiomStatus;
        }

        public Point GetFreeCell(BiomStatus[] list)
        {
            var rnd = new Random();
            int randX = rnd.Next(0, FieldSize);
            int randY = rnd.Next(0, FieldSize);
            if (!IsCellFree(new Point(randX, randY)))
            {
                return GetFreeCell(list);
            }
            foreach (BiomStatus biomStatus in list)
            {
                if (GetBiomStatusCell(randX, randY) == biomStatus)
                {
                    return new Point(randX, randY);
                }
            }
            return GetFreeCell(list);
        }

        public Point GetFreeCellCloseToPlant(Point start)
        {
            var q = new Queue<Point>();
            q.Enqueue(start);
            Point curPoint;
            var rand = new Random();
            while (q.Count > 0)
            {
                curPoint = q.Dequeue();
                if (GetBiomStatusCell(curPoint.X, curPoint.Y) == BiomStatus.Grass && IsCellFree(curPoint))
                {
                    return curPoint;
                }
                int addedPoints = 0;
                while (addedPoints == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int randX = curPoint.X + rand.Next(-3, 4);
                        int randY = curPoint.Y + rand.Next(-3, 4);
                        if (IsAvailable(randX, randY))
                        {
                            q.Enqueue(new Point(randX, randY));
                            addedPoints++;
                        }
                    }
                }
            }
            return new Point(-1, -1);
        }

        public Point GetFreeCloseCell(Point start, BiomStatus[] list, int radius)
        {
            var q = new Queue<Point>();
            q.Enqueue(start);
            Point curPoint;
            var rand = new Random();
            while (q.Count > 0)
            {
                curPoint = q.Dequeue();
                if (curPoint != start && IsCellFree(curPoint))
                {
                    foreach (var bs in list)
                    {
                        if (bs == GetBiomStatusCell(curPoint.X, curPoint.Y))
                        {
                            return curPoint;
                        }
                    }
                }
                int addedPoints = 0;
                while (addedPoints == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int randX = curPoint.X + rand.Next(-radius, radius + 1);
                        int randY = curPoint.Y + rand.Next(-radius, radius + 1);
                        if (IsAvailable(randX, randY))
                        {
                            q.Enqueue(new Point(randX, randY));
                            addedPoints++;
                        }
                    }
                }
            }
            return new Point(-1, -1);
        }

        public bool IsAvailable(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < FieldSize && y < FieldSize)
            {
                if (GetBiomStatusCell(x, y) == BiomStatus.Grass || GetBiomStatusCell(x, y) == BiomStatus.Desert)
                {
                    return true;
                }
            }
            return false;
        }

        public void Build(Point coords, Type materialType, bool isHouse = false, bool isBarn = false)
        {
            Building building = null;
            if (isHouse)
            {
                building = new House(coords);
            }
            else if (isBarn)
            {
                building = new Barn(coords);
            }
            else if (materialType == typeof(Gold))
            {
                building = new Treasury(coords);
            }
            else if (materialType == typeof(Iron))
            {
                building = new Smithy(coords);
            }
            else if (materialType == typeof(Stone))
            {
                building = new StoneWarehouse(coords);
            }
            else if (materialType == typeof(Wood))
            {
                building = new WoodWarehouse(coords);
            }
            for (int i = -8 + coords.X; i < coords.X + 8; i++)
            {
                for (int j = -8 + coords.Y; j < coords.Y + 8; j++)
                {
                    if (i > 0 && j > 0 && i < FieldSize && j < FieldSize)
                    {
                        if (Cells[i, j].Building != null)
                        {
                            Cells[i, j].Building.Village.Buildings.Add(building);
                            building.AddToVillage(Cells[i, j].Building.Village);
                            Cells[coords.X, coords.Y].Building = building;
                            return;
                        }
                    }
                }
            }
            var village = new Village();
            village.Buildings.Add(building);
            Villages.Add(village);
            building.AddToVillage(village);

            Cells[coords.X, coords.Y].Building = building;
        }
    }
}
