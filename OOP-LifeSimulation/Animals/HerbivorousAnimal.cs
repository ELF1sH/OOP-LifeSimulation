using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LifeSimulation
{
    abstract class HerbivorousAnimal : Animal
    {
        public HerbivorousAnimal(int _x, int _y, Map _map, int _animalId) : base(_x, _y, _map, _animalId)
        {
            FeedType = FeedType.Herbivorous;
        }

        protected override AnimalResult CheckFood(Unit food)
        {
            if (food == null) return AnimalResult.Nothing;
            AnimalResult animalResult = AnimalResult.Nothing;
            if (food is Plant || food is Fruit)
            {
                if (map.IsFoodPoisonous(Coords))
                {
                    animalResult = AnimalResult.EatedPoisonousPlant;
                    UpdateHP__PoisonousPlant();
                }
                else
                {
                    animalResult = AnimalResult.EatedPlant;
                    UpdateHP__HealthyFood();
                }
            }
            // Detaching eaten units from map
            if (food is Plant) map.DetachPlantFromCell(Coords.X, Coords.Y);
            else if (food is Fruit) map.DetachFruitFromCell(Coords.X, Coords.Y);

            return animalResult;
        }
    }
}
