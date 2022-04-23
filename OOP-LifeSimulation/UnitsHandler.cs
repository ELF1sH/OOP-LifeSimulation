using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OOP_LifeSimulation
{
    class UnitsHandler
    {
        private readonly List<Animal> animals;
        public readonly List<Plant> plants;

        public UnitsHandler(List<Animal> _animals, List<Plant> _plant)
        {
            animals = _animals;
            plants = _plant;
        }



        #region Animals
        public AnimalResult[] AnimalsUpdate()
        {
            AnimalResult[] resultArray = new AnimalResult[animals.Count];
            for (int i = 0; i < animals.Count; i++)
            {
                resultArray[i] = animals[i].Update();
            }
            return resultArray;
        }

        public int FeedAnimals()
        {
            int counterOfDeadAnimals = 0;
            foreach (var animal in animals.ToList())
            {
                if (animal.HasBeenEaten) continue;
                var animalEatingResult = animal.TryToEat();
                if (animalEatingResult == AnimalResult.EatedPlant || animalEatingResult == AnimalResult.EatedPoisonousPlant)
                {
                    RemovePlant(animal.Coords.X, animal.Coords.Y);
                }
                else if (animalEatingResult == AnimalResult.EatedAnimal)
                {
                    bool res = SetHasBeenEaten(animal.Coords, animal.AnimalType);
                    if (!res) res = SetHasBeenEaten(animal.PrevCoords, animal.AnimalType);
                    if (res) counterOfDeadAnimals++;
                }
            }
            return counterOfDeadAnimals;
        }

        public int ReproduceAnimals()
        {
            int numberOfAddAnimals = 0;
            foreach (var animal in animals.ToList())
            {
                var newAnimal = animal.TryToReproduce();
                if (newAnimal != null)
                {
                    animals.Add(newAnimal);
                    numberOfAddAnimals++;
                }
            }
            return numberOfAddAnimals;
        }

        public Animal GetAnimal(int index)
        {
            return animals[index];
        }

        public void RemoveDeadAnimals(List<int> indices)
        {
            int counter = 0;
            indices.ForEach(x =>
            {
                if (animals[x - counter] is Human)
                {
                    // working with partner
                    int partnerId = (animals[x - counter] as Human).PartnerId;
                    if (partnerId != -1)
                    {
                        (animals[x - counter] as Human).Partner.Partner = null;
                        (animals[x - counter] as Human).Partner.PartnerId = -1;
                    }

                    // working with tamed animal
                    if ((animals[x - counter] as Human).TamedAnimal != null)
                    {
                        (animals[x - counter] as Human).TamedAnimal.RemoveOwner();
                    }
                }
                if (animals[x - counter] is ITamable)
                {
                    if ((animals[x - counter] as ITamable).IsTamed)
                    {
                        (animals[x - counter] as ITamable).Owner.RemoveTamedAnimal();
                    }
                }
                animals.RemoveAt(x - counter);
                counter++;
            });
        }

        public bool SetHasBeenEaten(Point p, AnimalType at)
        {
            foreach (var an in animals)
            {
                if (an.AnimalType != at)
                {
                    if (Math.Abs(an.Coords.X - p.X) <= 1 && Math.Abs(an.Coords.Y - p.Y) <= 1 || 
                        Math.Abs(an.PrevCoords.X - p.X) <= 1 && Math.Abs(an.PrevCoords.Y - p.Y) <= 1)
                    {
                        an.HasBeenEaten = true;
                        animals.Remove(an);
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion



        #region Plants
        public List<PlantResult> GrowPlants()
        {
            var plantResults = new List<PlantResult>();
            foreach (Plant plant in plants)
            {
                plantResults.Add(plant.Grow());
            }
            return plantResults;
        }

        public Plant GetPlant(int index)
        {
            return plants[index];
        }

        public List<Plant> GenerateSeeds()
        {
            var seedsOutput = new List<Plant>();
            foreach (Plant plant in plants)
            {
                var curSeeds = plant.GenerateSeeds();
                foreach(Plant seed in curSeeds)
                {
                    seedsOutput.Add(seed);
                }
            }
            AddNewPlants(seedsOutput);
            return seedsOutput;
        }

        private void RemovePlant(int x, int y)
        {
            foreach (Plant plant in plants)
            {
                if (plant.Coords.X == x && plant.Coords.Y == y)
                {
                    plants.Remove(plant);
                    break;
                }
            }
        }

        private void AddNewPlants(List<Plant> plantsInput)
        {
            foreach (Plant plant in plantsInput)
            {
                plants.Add(plant);
            }
        }

        public void GenerateFruits()
        {
            foreach (Plant plant in plants)
            if (plant is ProlificPlant prolificPlant)
            {
                if (prolificPlant.Fruits.Count == 0)
                {
                    prolificPlant.GenerateFruits();
                }  
            }
        }

        public void RemoveDeadPlants(List<int> indices)
        {
            int counter = 0;
            indices.ForEach(x =>
            {
                plants.RemoveAt(x - counter);
                counter++;
            });
        }

        public List<Fruit> GrowFruits()
        {
            var outputFruits = new List<Fruit>();
            foreach (Plant plant in plants)
            {
                if (plant is ProlificPlant prolificPlant)
                {
                    var curFruits = prolificPlant.GrowFruits();
                    foreach (Fruit fruit in curFruits)
                    {
                        outputFruits.Add(fruit);
                    }
                }
            }
            return outputFruits;
        }
        #endregion
    }
}
