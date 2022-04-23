using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LifeSimulation
{
    class Engine
    {
        private readonly Drawer drawer;
        private readonly Map map;
        private readonly UnitsHandler unitsHandler;
        private readonly Statistic statistic;
        private readonly int initialAnimalsNumber = 1000;
        private readonly int initialPlantsNumber = 5000;
        private readonly int initialResourcesNumber = 5000;
        private int secondsCounter = 0;

        private int animalIdForInfoForm = -1;
        private Animal animalForInfoForm = null;

        public Engine(GameForm _gameForm, int _fieldSize)
        {
            map = new Map(_fieldSize);
            drawer = new Drawer(_gameForm, _fieldSize, map);
            map.Drawer = drawer;
            drawer.PaintMap();
            InitialResourcesGenerate(initialResourcesNumber);
            unitsHandler = new UnitsHandler(InitialAnimalsGenerate(initialAnimalsNumber), InitialPlantsGenerate(initialPlantsNumber));
            statistic = new Statistic(_gameForm);

            drawer.UpdatePictureBox();
        }



        #region Updates
        public void Update(object sender, EventArgs e, int pictureBoxWidth)
        {
            if (secondsCounter > 0 && secondsCounter % 5000 == 0)
            {
                UpdateSeason();
            }
            UpdateAnimals();
            UpdatePlants();

            drawer.UpdatePictureBox();

            secondsCounter++;
        }

        private void UpdateAnimals()
        {
            int okAnimals = 0;
            int hungryAnimals = 0;
            int deadAnimals = 0;

            AnimalResult[] animalResults = unitsHandler.AnimalsUpdate();
            var animalsToDie = new List<int>();
            for (int i = 0; i < animalResults.Length; i++)
            {
                Animal curAnimal = unitsHandler.GetAnimal(i);
                Point prevCoords = curAnimal.PrevCoords;
                Point coords = curAnimal.Coords;
                if (animalResults[i] == AnimalResult.Dead)
                { 
                    drawer.PaintOverAnimal(new Point(prevCoords.X, prevCoords.Y));
                    animalsToDie.Add(i);
                    map.DetachAnimalFromCell(prevCoords.X, prevCoords.Y, curAnimal.AnimalType);
                    map.DetachAnimalFromCell(coords.X, coords.Y, curAnimal.AnimalType);

                    deadAnimals++;

                    if (curAnimal.AnimalId == animalIdForInfoForm)
                    {
                        animalForInfoForm = null;
                        animalIdForInfoForm = -1;
                    }
                }
                else if (animalResults[i] != AnimalResult.Dead)
                {
                    if (prevCoords.X != coords.X || prevCoords.Y != coords.Y) 
                    {
                        map.DetachAnimalFromCell(prevCoords.X, prevCoords.Y, curAnimal.AnimalType);
                        map.AttachAnimalToCell(coords.X, coords.Y, ref curAnimal);
                        drawer.AnimalMove(prevCoords.X, prevCoords.Y, coords.X, coords.Y, curAnimal.AnimalType, curAnimal.Gender, curAnimal.HP, curAnimal.SP);

                        RedrawPlant(curAnimal.PrevCoords);
                        RedrawPlant(new Point(curAnimal.PrevCoords.X, curAnimal.PrevCoords.Y - 1));
                    }
                    
                    if (animalResults[i] == AnimalResult.SearchingForFood) hungryAnimals++;
                    else if (animalResults[i] == AnimalResult.Nothing) okAnimals++;

                    if (curAnimal.AnimalId == animalIdForInfoForm)
                    {
                        animalForInfoForm = curAnimal;
                    }
                }
            }

            unitsHandler.RemoveDeadAnimals(animalsToDie);
            deadAnimals += unitsHandler.FeedAnimals();

            okAnimals += unitsHandler.ReproduceAnimals();

            statistic.UpdateStatistic(okAnimals, hungryAnimals, deadAnimals);


            void RedrawPlant(Point p)
            {
                if (p.X < 0 || p.Y < 0) return;
                // redrawing plants or fruits if they haven't been eaten
                if (map.Cells[p.X, p.Y].HasPlant)
                {
                    Plant plant = map.Cells[p.X, p.Y].Plant;
                    drawer.DrawPlant(p.X, p.Y, plant.PlantType, plant.PlantStatus);
                }
                else if (map.Cells[p.X, p.Y].HasFruit)
                {
                    Fruit fruit = map.Cells[p.X, p.Y].Fruit;
                    drawer.DrawFruit(p.X, p.Y, fruit.PlantType);
                }
            }
        }

        private void UpdatePlants()
        {
            // growing
            var plantResults = unitsHandler.GrowPlants();
            Plant curPlant;
            var plantsToDie = new List<int>();
            for (int i = 0; i < plantResults.Count; i++)
            {
                if (plantResults[i] == PlantResult.HasGrown)
                {
                    curPlant = unitsHandler.GetPlant(i);
                    drawer.RedrawPlant(
                        curPlant.Coords.X, curPlant.Coords.Y, 
                        curPlant.PlantType, curPlant.PlantStatus, 
                        map.GetBiomStatusCell(curPlant.Coords.X, curPlant.Coords.Y)
                    );
                }
                else if (plantResults[i] == PlantResult.Dead)
                {
                    curPlant = unitsHandler.GetPlant(i);
                    plantsToDie.Add(i);
                    map.DetachPlantFromCell(curPlant.Coords.X, curPlant.Coords.Y);
                    drawer.PaintOverAnimal(curPlant.Coords);
                }
            }

            // removing dead plants from PlantList in UnitsHandler
            unitsHandler.RemoveDeadPlants(plantsToDie);

            // generating seeds
            var seeds = unitsHandler.GenerateSeeds();
            seeds.ForEach(delegate (Plant seed)
            {
                map.AttachPlantToCell(seed.Coords.X, seed.Coords.Y, ref seed);
                drawer.DrawPlant(seed.Coords.X, seed.Coords.Y, seed.PlantType, seed.PlantStatus);
            });

            unitsHandler.GenerateFruits();
            // growing fruits
            var fruits = unitsHandler.GrowFruits();
            fruits.ForEach(delegate (Fruit fruit)
            {
                map.AttachFruitToCell(fruit.Coords.X, fruit.Coords.Y, ref fruit);
                drawer.DrawFruit(fruit.Coords.X, fruit.Coords.Y, fruit.PlantType);
            });
        }

        public void UpdateZoom()
        {
            drawer.UpdateZoom();
        }

        private void UpdateSeason()
        {
            if (map.CurSeason == Season.Summer)
            {
                map.CurSeason = Season.Winter;
            }
            else if (map.CurSeason == Season.Winter)
            {
                map.CurSeason = Season.Summer;
            }
            drawer.PaintMap();
            foreach (Plant plant in unitsHandler.plants)
            {
                drawer.DrawPlant(plant.Coords.X, plant.Coords.Y, plant.PlantType, plant.PlantStatus);
            }
            Fruit curFruit;
            for (int i = 0; i < map.FieldSize; i++)
            {
                for (int j = 0; j < map.FieldSize; j++)
                {
                    curFruit = map.GetFruitFromCell(i, j);
                    if (curFruit != null)
                    {
                        drawer.DrawFruit(i, j, curFruit.PlantType);
                    }
                }
            }
        }
        #endregion



        #region InitialGeneration
        private List<Plant> InitialPlantsGenerate(int plantsNumber)
        {
            var plants = new List<Plant>();
            Point coords;
            PlantType plantType;
            PlantStatus plantStatus;
            for (int i = 0; i < plantsNumber; i++)
            {
                coords = map.GetFreeCell(new BiomStatus[] { BiomStatus.Grass });
                int randInt;

                randInt = new Random().Next(0, 4);
                plantStatus = randInt switch
                {
                    0 => PlantStatus.Seeds,
                    1 => PlantStatus.Sprout,
                    2 => PlantStatus.Adult,
                    3 => PlantStatus.Rotted,
                    _ => PlantStatus.Seeds,
                };

                Plant curPlant;
                randInt = new Random().Next(0, 7);
                switch (randInt)
                {
                    case 0: plantType = PlantType.AppleTree; curPlant = new AppleTree(coords.X, coords.Y, plantStatus, map); break;
                    case 1: plantType = PlantType.Bush; curPlant = new Bush(coords.X, coords.Y, plantStatus, map); break;
                    case 2: plantType = PlantType.Carrot; curPlant = new Carrot(coords.X, coords.Y, plantStatus, map); break;
                    case 3: plantType = PlantType.Peas; curPlant = new Peas(coords.X, coords.Y, plantStatus, map); break;
                    case 4: plantType = PlantType.PoisonousBerryBush; curPlant = new PoisonousBerryBush(coords.X, coords.Y, plantStatus, map); break;
                    case 5: plantType = PlantType.PoisonousMushroom; curPlant = new PoisonousMushroom(coords.X, coords.Y, plantStatus, map); break;
                    case 6: plantType = PlantType.Tree; curPlant = new Tree(coords.X, coords.Y, plantStatus, map); break;
                    default: plantType = PlantType.AppleTree; curPlant = new AppleTree(coords.X, coords.Y, plantStatus, map); break;
                }

                plants.Add(curPlant);
                map.AttachPlantToCell(coords.X, coords.Y, ref curPlant);
                drawer.DrawPlant(coords.X, coords.Y, plantType, plantStatus);
            }
            return plants;
        }

        private List<Animal> InitialAnimalsGenerate(int animalsNumber)
        {
            var animals = new List<Animal>();
            var rnd = new Random();
            Point coords;
            /*            animals.Add(new Human(30, 30, map, 1));
                        animals.Add(new Human(35, 35, map, 2));
                        drawer.DrawAnimal(30, 30, animals[^1].AnimalType, 100, 100, animals[^1].Gender);
                        drawer.DrawAnimal(35, 35, animals[^1].AnimalType, 100, 100, animals[^1].Gender);*/
            /*for (int i = 0; i < animalsNumber; i++)
            {
                coords = map.GetFreeCell(new BiomStatus[] { BiomStatus.Grass, BiomStatus.Desert });
                int value = rnd.Next(0, 10);
                switch (value)
                {
                    case 0: animals.Add(new Bear(coords.X, coords.Y, map, i)); break;
                    case 1: animals.Add(new Elephant(coords.X, coords.Y, map, i)); break;
                    case 2: animals.Add(new Hedgehog(coords.X, coords.Y, map, i)); break;
                    case 3: animals.Add(new Horse(coords.X, coords.Y, map, i)); break;
                    case 4: animals.Add(new Leopard(coords.X, coords.Y, map, i)); break;
                    case 5: animals.Add(new Monkey(coords.X, coords.Y, map, i)); break;
                    case 6: animals.Add(new Rabbit(coords.X, coords.Y, map, i)); break;
                    case 7: animals.Add(new Tiger(coords.X, coords.Y, map, i)); break;
                    case 8: animals.Add(new Wolf(coords.X, coords.Y, map, i)); break;
                    case 9: animals.Add(new Human(coords.X, coords.Y, map, i)); break;
                }
                drawer.DrawAnimal(coords.X, coords.Y, animals[^1].AnimalType, 100, 100, animals[^1].Gender);
            }*/
            for (int i = 0; i < animalsNumber; i++)
            {
                coords = map.GetFreeCell(new BiomStatus[] { BiomStatus.Grass, BiomStatus.Desert });
                int value = rnd.Next(0, 1);
                switch (value)
                {
                    case 0: animals.Add(new Human(coords.X, coords.Y, map, i, false)); break;
                        // case 1: animals.Add(new Horse(coords.X, coords.Y, map, i)); break;
                }
                drawer.DrawAnimal(coords.X, coords.Y, animals[^1].AnimalType, 100, 100, animals[^1].Gender);
                var curAnimal = animals[^1];
                map.AttachAnimalToCell(coords.X, coords.Y, ref curAnimal);
            }
            /*animals.Add(new Human(30, 30, map, 0));
            drawer.DrawAnimal(30, 30, animals[^1].AnimalType, 100, 100, animals[^1].Gender);*/
            /*animals.Add(new Human(33, 33, map, 5));
            drawer.DrawAnimal(33, 33, animals[^1].AnimalType, 100, 100, animals[^1].Gender);*/
            return animals;
        }

        private void InitialResourcesGenerate(int resourcesNumber)
        {
            Point coords;
            var rnd = new Random();
            for (int i = 0; i < resourcesNumber; i++)
            {
                coords = map.GetFreeCell(new BiomStatus[] { BiomStatus.Grass, BiomStatus.Desert });
                int rndValue = rnd.Next(0, 3);
                Resource curResource = rndValue switch
                {
                    0 => new StoneRock(coords),
                    1 => new IronOre(coords),
                    2 => new GoldMine(coords),
                    _ => new StoneRock(coords)
                };
                map.Cells[coords.X, coords.Y].Resource = curResource;
                drawer.DrawResource(coords, curResource.GetType());
            }
        }
        #endregion


        public List<string> GetAnimalInfo(Point p)
        {
            var animal = map.GetAnimal(p);
            if (animal == null) animal = map.GetAnimal(new Point(p.X - 1, p.Y));
            if (animal == null) animal = map.GetAnimal(new Point(p.X + 1, p.Y));
            if (animal == null) animal = map.GetAnimal(new Point(p.X, p.Y - 1));
            if (animal == null) animal = map.GetAnimal(new Point(p.X, p.Y + 1));
            if (animal != null)
            {
                animalIdForInfoForm = animal.AnimalId;
                return GetDataList(animal);
            }
            return new List<string>();
        }

        public List<string> GetUpdatedAnimalInfo()
        {
            if (animalForInfoForm != null)
            {
                return GetDataList(animalForInfoForm);
            }
            return new List<string>();
        }

        private List<string> GetDataList(Animal animal)
        {
            var outputList = new List<string>
            {
                animal.AnimalType.ToString(),
                "X = " + animal.Coords.X + "   " + "Y = " + animal.Coords.Y,
                animal.HP.ToString(),
                animal.SP.ToString()
            };
            if (animal is HerbivorousAnimal) outputList.Add("Herbivorous");
            else if (animal is CarnivorousAnimal) outputList.Add("Carnivorous");
            else outputList.Add("Omnivorous");

            if (animal is Human)
            {
                if ((animal as Human).Partner != null)
                {
                    outputList.Add("X = " + (animal as Human).Partner.Coords.X + "   " + "Y = " + (animal as Human).Partner.Coords.Y);
                }
                else outputList.Add("None");

                int plantsCount = 0;
                int fruitsCount = 0;
                int meatCount = 0;
                foreach (var item in (animal as Human).HumanInventory.Storage.Items)
                {
                    if (item is Plant) plantsCount++;
                    else if (item is Fruit) fruitsCount++;
                    else if (item is Animal) meatCount++;
                }
                int sum = plantsCount + fruitsCount + meatCount;
                outputList.Add(plantsCount + " plants, " + fruitsCount + " fruits, " + meatCount + " animals (" 
                    + sum + "/" + (animal as Human).HumanInventory.InventorySize + ")");

                if ((animal as Human).TamedAnimal != null)
                {
                    var tamedAnimal = (animal as Human).TamedAnimal;
                    outputList.Add((tamedAnimal as Animal).AnimalType.ToString() + 
                        " X = " + (tamedAnimal as Animal).Coords.X + "   " + "Y = " + (tamedAnimal as Animal).Coords.Y);
                }
                else outputList.Add("None");
            }

            return outputList;
        }
    }
}
